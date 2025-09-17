#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Game.Gameplay.World;
using Game.Gameplay.CraftingStations;

namespace Game.EditorTools
{
    public static class PlaceWorldContent
    {
        [MenuItem("Tools/World/Place Resource and Clue Nodes")] 
        public static void Place()
        {
            MakeResource("FatNode", new Vector2(-2, -20), "fat");
            MakeResource("ThreadNode", new Vector2(2, -20), "thread");
            MakeResource("ScrapNode", new Vector2(-2, 0), "scrap");
            MakeResource("WaterNode", new Vector2(2, 0), "water");

            MakeClue("ArticleClue", new Vector2(-12, 12), "found_article");
            MakeClue("BlueprintClue", new Vector2(-12, 10), "west_found_blueprint");
            MakeClue("MarketOccultClue", new Vector2(24, 12), "east_market_occult");

            MakeWorkbench(new Vector2(-1, -1));
            MakeKitchen(new Vector2(1, -1));
            Debug.Log("Placed resource nodes, clues, and stations.");
        }

        private static void MakeResource(string name, Vector2 pos, string id)
        {
            var go = new GameObject(name);
            go.transform.position = pos;
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<Game.Gameplay.Interactions.PlayerInteractor>();
            var node = go.AddComponent<ResourceNode>();
            node.resourceId = id;
        }

        private static void MakeClue(string name, Vector2 pos, string flag)
        {
            var go = new GameObject(name);
            go.transform.position = pos;
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<Game.Gameplay.Interactions.PlayerInteractor>();
            var node = go.AddComponent<ClueNode>();
            node.clueFlag = flag;
        }

        private static void MakeWorkbench(Vector2 pos)
        {
            var go = new GameObject("Workbench");
            go.transform.position = pos;
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<Game.Gameplay.Interactions.PlayerInteractor>();
            go.AddComponent<WorkbenchStation>();
        }

        private static void MakeKitchen(Vector2 pos)
        {
            var go = new GameObject("Kitchen");
            go.transform.position = pos;
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<Game.Gameplay.Interactions.PlayerInteractor>();
            go.AddComponent<KitchenStation>();
        }
    }
}
#endif