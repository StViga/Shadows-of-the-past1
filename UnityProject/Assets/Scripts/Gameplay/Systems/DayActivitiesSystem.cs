using UnityEngine;
using Game.Gameplay.Crafting;
using Game.Core;

namespace Game.Gameplay.Systems
{
    // Daytime: gather resources and clues
    public sealed class DayActivitiesSystem : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventory = null;
        [SerializeField] private StatsManager stats = null;
        [SerializeField] private GameFlagManager flags = new GameFlagManager();

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public void GatherResource(string id, int amount)
        {
            inventory.Add(id, amount);
            Debug.Log($"Gathered {amount}x {id}");
        }

        public void FindClue(string flag)
        {
            if (flags.Set(flag, true))
                Debug.Log($"Found clue: {flag}");
        }
    }
}
