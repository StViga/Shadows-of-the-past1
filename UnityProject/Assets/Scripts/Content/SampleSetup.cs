using Game.Core;
using Game.Systems.DayNight;
using UnityEngine;

namespace Game.Content
{
    public sealed class SampleSetup : MonoBehaviour
    {
        [SerializeField] private DayNightCycle cycle = null!;
        private GameFlagManager flags = new();

        private void Awake()
        {
            // Bootstrap flags and bind to Yarn later
            Game.Systems.Dialog.YarnBridge.Init(flags);

            // Example subscriptions: doors and NPC schedules can listen here
            cycle.OnPhaseChanged += phase =>
            {
                Debug.Log($"Phase switched to {phase}");
            };
        }

        [ContextMenu("Debug Unlock Hidden Tunnel")] private void DebugUnlockTunnel()
        {
            flags.Set(Flags.WestFoundBlueprint, true);
            flags.Set(Flags.MorgueFatherExperiments, true);
            flags.Set(Flags.FoundTruthTunnel, true);
            Debug.Log("Hidden tunnel unlocked via debug.");
        }
    }
}
