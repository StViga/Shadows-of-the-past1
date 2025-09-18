using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Core
{
    [Serializable]
    public sealed class SaveData
    {
        public List<string> flags = new List<string>();
        public int money;
        public int suspicion;
        public int sanity;
        public int fatigue;
        public Dictionary<string, int> inventory = new Dictionary<string, int>();
    }

    public static class SaveSystem
    {
        private static string Path => System.IO.Path.Combine(Application.persistentDataPath, "save.json");

        public static void Save(GameFlagManager flags, StatsManager stats, Game.Gameplay.Crafting.InventoryManager inv)
        {
            var data = new SaveData
            {
                flags = new List<string>(flags.Enumerate()),
                money = stats.money,
                suspicion = stats.suspicion,
                sanity = stats.sanity,
                fatigue = stats.fatigue,
                inventory = new Dictionary<string, int>(inv.Dump())
            };
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(Path, json);
            Debug.Log($"Saved to {Path}");
        }

        public static bool Load(GameFlagManager flags, StatsManager stats, Game.Gameplay.Crafting.InventoryManager inv)
        {
            if (!File.Exists(Path)) return false;
            var json = File.ReadAllText(Path);
            var data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) return false;

            // restore flags
            foreach (var f in new List<string>(flags.Enumerate())) flags.Set(f, false);
            foreach (var f in data.flags) flags.Set(f, true);

            // restore stats
            stats.money = data.money;
            stats.suspicion = data.suspicion;
            stats.sanity = data.sanity;
            stats.fatigue = data.fatigue;

            // restore inventory
            foreach (var kv in data.inventory)
                inv.Add(kv.Key, kv.Value);

            Debug.Log($"Loaded from {Path}");
            return true;
        }
    }
}
