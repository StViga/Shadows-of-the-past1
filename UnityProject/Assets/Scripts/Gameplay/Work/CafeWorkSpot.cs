using Game.Core;
using Game.Systems.DayNight;
using UnityEngine;

namespace Game.Gameplay.Work
{
    public sealed class CafeWorkSpot : Game.Gameplay.Interactions.Interactable
    {
        [SerializeField] private StatsManager stats = null!;
        [SerializeField] private DayNightCycle cycle = null!;
        [SerializeField] private int basePay = 25;
        [SerializeField] private int fatigueGain = 15;
        [SerializeField] private int suspicionGainIfFakeId = 5;
        [SerializeField] private string fakeIdFlag = Flags.FoundFalseId;

        private GameFlagManager flags = new();

        public override string Prompt => cycle != null && cycle.CurrentPhase == Phase.Day ? "Start shift [E]" : "Closed for the night";

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public override bool Interact()
        {
            if (cycle.CurrentPhase != Phase.Day) return false;
            if (!stats.CanStartWork()) return false;

            stats.AddMoney(basePay);
            stats.AddFatigue(fatigueGain);
            if (flags.Get(fakeIdFlag)) stats.AddSuspicion(suspicionGainIfFakeId);
            Debug.Log($"Cafe shift complete. +{basePay}.");
            return true;
        }
    }
}
