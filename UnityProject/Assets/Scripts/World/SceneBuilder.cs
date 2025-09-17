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
        public GameObject entrancePrefab;

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
            CreateZone(root.transform, ZoneType.Library, new Vector2(-cell, cell));
            CreateZone(root.transform, ZoneType.School, new Vector2(-cell, -cell));
            CreateZone(root.transform, ZoneType.Cafe, new Vector2(0, -cell*2));
            CreateZone(root.transform, ZoneType.PhotoStudio, new Vector2(cell*2, 0));
            CreateZone(root.transform, ZoneType.Neighbor, new Vector2(-cell*2, 0));

            // Place interactives
            SpawnCafe(new Vector2(0, -cell)); // south district
            SpawnAltar(new Vector2(cell, cell)); // chapel
            SpawnMorgueDoor(new Vector2(cell + 3, -cell));

            // Entrances for all zones
            SpawnEntrance(ZoneType.Library, new Vector2(-cell, cell + 2), "Library", "scene_west_library");
            SpawnEntrance(ZoneType.School, new Vector2(-cell, -cell + 2), "School", "scene_west_school");
            SpawnEntrance(ZoneType.Cafe, new Vector2(0, -cell*2 + 2), "Cafe", "scene_south_cafe");
            SpawnEntrance(ZoneType.PhotoStudio, new Vector2(cell*2 - 2, 0), "Photo Studio", "scene_south_studio");
            SpawnEntrance(ZoneType.Neighbor, new Vector2(-cell*2 + 2, 0), "Neighbor", "scene_west_neighbor");
            SpawnEntrance(ZoneType.Chapel, new Vector2(cell, cell + 2), "Chapel", "scene_east_chapel");
            SpawnEntrance(ZoneType.Market, new Vector2(cell*2, cell), "Market", "scene_east_market");

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

        private void SpawnEntrance(ZoneType type, Vector2 pos, string label, string yarnNode)
        {
            var go = new GameObject($"Entrance_{type}");
            go.transform.position = pos;
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<PlayerInteractor>();
            var e = go.AddComponent<Game.World.Entrance>();
            e.entranceName = label;
            e.yarnNode = yarnNode;
            e.spawnOffset = new Vector3(0.5f, 0.5f, 0);
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
