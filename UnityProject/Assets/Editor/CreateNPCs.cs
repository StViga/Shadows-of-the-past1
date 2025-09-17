#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Game.NPC;

namespace Game.EditorTools
{
    public static class CreateNPCs
    {
        [MenuItem("Tools/NPC/Place Core NPCs")] 
        public static void Place()
        {
            MakeNPC("Barista", new Vector2(0, -24), day:"scene_south_cafe", night:"scene_south_cafe_false_identity");
            MakeNPC("Priest", new Vector2(12, 12), day:"scene_east_chapel", night:"scene_east_chapel_prayer");
            MakeNPC("Guard", new Vector2(15, -12), day:"scene_east_morgue", night:"scene_east_morgue_body");
            MakeNPC("Librarian", new Vector2(-12, 12), day:"scene_west_library", night:"scene_west_library_search");
            MakeNPC("Photographer", new Vector2(24, 0), day:"scene_south_studio", night:"scene_south_studio_memory_photo");
            MakeNPC("Neighbor", new Vector2(-24, 0), day:"scene_west_neighbor", night:"scene_west_neighbor_childhood");
            MakeNPC("MarketWoman", new Vector2(24, 12), day:"scene_east_market", night:"scene_east_market_table");
            MakeNPC("Coroner", new Vector2(12, -12), day:"scene_east_morgue", night:"scene_east_morgue_dna");
            Debug.Log("Core NPCs placed with day and night nodes.");
        }

        private static void MakeNPC(string name, Vector2 pos, string day, string night)
        {
            var go = new GameObject(name);
            go.transform.position = pos;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = Random.ColorHSV(0f,1f,0.6f,1f,0.8f,1f);
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            go.AddComponent<Game.Gameplay.Interactions.PlayerInteractor>();
            var talk = go.AddComponent<NpcInteract>();
            talk.dayNode = day;
            talk.nightNode = night;
            go.AddComponent<NpcSchedule>();
        }
    }
}
#endif