using UnityEngine;
using Game.Gameplay.Crafting;
using Game.Systems.DayNight;

namespace Game.Gameplay.World
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ResourceNode : Game.Gameplay.Interactions.Interactable
    {
        public string resourceId = ResourceIds.Fat;
        public int amount = 1;
        public bool dayOnly = true;
        public int fatigueCost = 5;
        private bool _depleted;

        public override string Prompt => _depleted ? "Depleted" : $"Gather {resourceId} [E]";

        private void Awake()
        {
            var cycle = Object.FindObjectOfType<DayNightCycle>();
            if (cycle != null) cycle.OnPhaseChanged += phase => { if (phase == Phase.Day) _depleted = false; };
        }

        public override bool Interact()
        {
            if (_depleted) return false;
            var cycle = Object.FindObjectOfType<DayNightCycle>();
            if (dayOnly && cycle != null && cycle.CurrentPhase != Phase.Day) return false;
            var stats = Object.FindObjectOfType<Game.Core.StatsManager>();
            if (stats == null || !stats.CanStartWork()) return false;
            var inv = Object.FindObjectOfType<Game.Gameplay.Crafting.InventoryManager>();
            if (inv == null) return false;
            inv.Add(resourceId, amount);
            stats.AddFatigue(fatigueCost);
            _depleted = true;
            Debug.Log($"Gathered {amount}x {resourceId}");
            return true;
        }
    }
}
