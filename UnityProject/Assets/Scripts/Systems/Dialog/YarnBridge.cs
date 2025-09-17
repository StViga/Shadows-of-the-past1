// Placeholder for integrating Yarn Spinner runtime conditions with our GameFlagManager
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay.Crafting;

namespace Game.Systems.Dialog
{
    public static class YarnBridge
    {
        private static GameFlagManager? _flags;
        public static void Init(GameFlagManager flags) => _flags = flags;

        // To be bound as Yarn function: hasFlag("morgue_father_experiments")
        public static bool HasFlag(string name) => _flags != null && _flags.Get(name);

        // To be bound as Yarn command: setFlag name true
        public static void SetFlag(string name, bool value)
        {
            _flags?.Set(name, value);
        }

        // Yarn: <<GrantItem "fat" 1>>
        public static void GrantItem(string id, int amount)
        {
            var inv = UnityEngine.Object.FindObjectOfType<InventoryManager>();
            if (inv == null) return;
            inv.Add(id, amount);
        }

        // Yarn: <<StartRitual>>
        public static void StartRitual()
        {
            var night = UnityEngine.Object.FindObjectOfType<Game.Gameplay.Systems.NightActivitiesSystem>();
            if (night == null) return;
            night.UseCandleForRitual();
        }

        // Yarn: <<FindClue "found_article">>
        public static void FindClue(string flag)
        {
            if (_flags == null) return;
            _flags.Set(flag, true);
        }

        public static IEnumerable<string> AllFlags() => Flags.All;
    }
}
