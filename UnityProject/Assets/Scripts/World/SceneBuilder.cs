using System.Collections.Generic;
using UnityEngine;
using Game.World;
using Game.Gameplay.Player;
using Game.Gameplay.Interactions;
using Game.Gameplay.Work;
using Game.Gameplay.CraftingStations;
using Game.Systems.DayNight;
using Game.Core;

namespace Game.Scenes
{
    public sealed class SceneBuilder : MonoBehaviour
    {
        [Header("Tile Prefabs and Props")] public GameObject floorTile;
        public GameObject wallTile;
        public GameObject doorPrefab;

        [Header("Interactive Prefabs")] public GameObject cafeSpotPrefab;
        public GameObject altarPrefab;
        public GameObject morgueDoorPrefab;

        [Header("Player")]
        public GameObject playerPrefab;

        private readonly List<GameObject> spawned = new();

        [ContextMenu("Build City Prototype")]
        public void Build()
        {
            Clear();
            var root = new GameObject("CityRoot");
            spawned.Add(root);

            // Simple block grid 3x2 of zones
            float cell = 12f;
            CreateZone(root.transform, ZoneType.CityNorth, new Vector2(0, cell));
            CreateZone(root.transform, ZoneType.CitySouth, new Vector2(0, -cell));
            CreateZone(root.transform, ZoneType.CityWest, new Vector2(-cell, 0));
            CreateZone(root.transform, ZoneType.CityEast, new Vector2(cell, 0));
            CreateZone(root.transform, ZoneType.Chapel, new Vector2(cell, cell));
            CreateZone(root.transform, ZoneType.Morgue, new Vector2(cell, -cell));

            // Place interactives
            SpawnCafe(new Vector2(0, -cell)); // south district
            SpawnAltar(new Vector2(cell, cell)); // chapel
            SpawnMorgueDoor(new Vector2(cell + 3, -cell));

            // Player spawn
            var p = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            spawned.Add(p);
            var rb = p.GetComponent<Rigidbody2D>();
            if (rb != null) rb.gravityScale = 0f;
            var col = p.GetComponent<Collider2D>();
            if (col != null) col.isTrigger = false;

            // Global systems if missing
            if (FindObjectOfType<DayNightCycle>() == null)
            {
                var go = new GameObject("DayNight");
                go.AddComponent<DayNightCycle>();
                spawned.Add(go);
            }
            if (FindObjectOfType<StatsManager>() == null)
            {
                var go = new GameObject("Stats");
                go.AddComponent<StatsManager>();
                spawned.Add(go);
            }
        }

        private void CreateZone(Transform parent, ZoneType type, Vector2 center)
        {
            var z = new GameObject(type.ToString());
            z.transform.SetParent(parent);
            z.transform.position = center;
            var zone = z.AddComponent<Zone>();
            zone.type = type;
            // Simple floor
            for (int x = -2; x <= 2; x++)
            for (int y = -2; y <= 2; y++)
            {
                var t = GameObject.CreatePrimitive(PrimitiveType.Quad);
                t.transform.SetParent(z.transform);
                t.transform.localScale = new Vector3(1, 1, 1);
                t.transform.localPosition = new Vector3(x, y, 0);
                var mr = t.GetComponent<MeshRenderer>();
                mr.sharedMaterial = new Material(Shader.Find("Unlit/Color")) { color = new Color(0.18f, 0.18f, 0.2f) };
            }
        }

        private void SpawnCafe(Vector2 pos)
        {
            var go = new GameObject("CafeWorkSpot");
            go.transform.position = pos + new Vector2(0, 0);
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<PlayerInteractor>();
            var spot = go.AddComponent<CafeWorkSpot>();
        }

        private void SpawnAltar(Vector2 pos)
        {
            var go = new GameObject("Altar");
            go.transform.position = pos + new Vector2(0, 0);
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<PlayerInteractor>();
            go.AddComponent<AltarStation>();
        }

        private void SpawnMorgueDoor(Vector2 pos)
        {
            var go = new GameObject("MorgueDoor");
            go.transform.position = pos;
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<PlayerInteractor>();
            go.AddComponent<NightDoorMorgue>();
        }

        private void Clear()
        {
            foreach (var g in spawned)
            {
                if (g != null) DestroyImmediate(g);
            }
            spawned.Clear();
        }
    }
}
