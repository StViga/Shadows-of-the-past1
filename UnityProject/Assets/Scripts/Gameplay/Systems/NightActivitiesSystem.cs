using UnityEngine;
using Game.Gameplay.Crafting;
using Game.Core;

namespace Game.Gameplay.Systems
{
    // Nighttime: craft and pursue truth
    public sealed class NightActivitiesSystem : MonoBehaviour
    {
        [SerializeField] private CraftingSystem crafting = null;
        [SerializeField] private InventoryManager inventory = null;
        [SerializeField] private StatsManager stats = null;
        [SerializeField] private GameFlagManager flags = new GameFlagManager();

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public bool UseCandleForRitual()
        {
            if (!inventory.Has(ResourceIds.Candle, 1)) return false;
            inventory.Remove(ResourceIds.Candle, 1);
            stats.AddSanity(-3);
            flags.Set(Flags.ChapelShadowWhisper, true);
            Debug.Log("Ritual performed: shadow whisper acknowledged.");
            return true;
        }

        public bool UseLockpick()
        {
            if (!inventory.Has(ResourceIds.Lockpick, 1)) return false;
            inventory.Remove(ResourceIds.Lockpick, 1);
            flags.Set(Flags.HasKey, true);
            Debug.Log("Used lockpick and acquired a key flag.");
            return true;
        }
    }
}
