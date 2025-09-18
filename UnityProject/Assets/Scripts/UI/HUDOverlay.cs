using System.Text;
using Game.Core;
using Game.Gameplay.Crafting;
using UnityEngine;

namespace Game.UI
{
    public sealed class HUDOverlay : MonoBehaviour
    {
        [SerializeField] private StatsManager stats = null;
        [SerializeField] private InventoryManager inventory = null;
        [SerializeField] private Game.Core.GameFlagManager flags = new();

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 320, 150), "Status");
            GUILayout.BeginArea(new Rect(20, 40, 300, 120));
            GUILayout.Label($"Money: {stats.money}");
            GUILayout.Label($"Suspicion: {stats.suspicion}");
            GUILayout.Label($"Sanity: {stats.sanity}");
            GUILayout.Label($"Fatigue: {stats.fatigue}");
            GUILayout.EndArea();

            GUI.Box(new Rect(10, 170, 320, 220), "Inventory");
            GUILayout.BeginArea(new Rect(20, 200, 300, 180));
            var sb = new StringBuilder();
            foreach (var kv in inventory.Dump())
            {
                sb.AppendLine($"{kv.Key}: {kv.Value}");
            }
            GUILayout.Label(sb.ToString());
            GUILayout.EndArea();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveSystem.Save(flags, stats, inventory);
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                SaveSystem.Load(flags, stats, inventory);
            }
        }
    }
}
