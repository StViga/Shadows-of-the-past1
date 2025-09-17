using Game.Core;
using Game.Gameplay.Interactions;
using Game.Systems.DayNight;
using UnityEngine;

namespace Game.Gameplay.Interactions
{
    public sealed class NightDoorMorgue : Interactable
    {
        [SerializeField] private StatsManager stats = null!;
        [SerializeField] private DayNightCycle cycle = null!;
        [SerializeField] private int suspicionThreshold = 40;
        [SerializeField] private string falseIdFlag = Flags.CafeFalseId;

        private GameFlagManager flags = new();

        public override string Prompt => "Enter morgue [E]";

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public override bool Interact()
        {
            if (cycle.CurrentPhase != Phase.Night) return false;
            if (stats.IsSuspiciousAbove(suspicionThreshold)) return false;
            if (!flags.Get(falseIdFlag)) return false;

            Debug.Log("Entered the morgue");
            return true;
        }
    }
}
