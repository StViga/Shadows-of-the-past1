#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.EditorTools
{
    public static class CreateCityPrototype
    {
        [MenuItem("Tools/World/Build City Prototype")]
        public static void Build()
        {
            var go = new GameObject("SceneBuilder");
            var builder = go.AddComponent<Game.Scenes.SceneBuilder>();
            builder.Build();
            Debug.Log("City prototype built. Player, cafe, altar, morgue door placed.");
        }
    }
}
#endif