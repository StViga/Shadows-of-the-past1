using System;
using UnityEngine;

namespace Game.Systems.DayNight
{
    public enum Phase { Day, Night }

    public sealed class DayNightCycle : MonoBehaviour
    {
        [Header("Durations in seconds for prototype")] public float dayDuration = 300f;
        public float nightDuration = 240f;

        [Header("Current time")] [SerializeField] private Phase currentPhase = Phase.Day;
        [SerializeField] private float phaseTimer;

        public event Action<Phase>? OnPhaseChanged;

        public Phase CurrentPhase => currentPhase;
        public float PhaseProgress01 => Mathf.Clamp01(phaseTimer / GetPhaseDuration(currentPhase));

        private void Start()
        {
            phaseTimer = 0f;
            RaisePhaseChanged();
        }

        private void Update()
        {
            phaseTimer += Time.deltaTime;
            var duration = GetPhaseDuration(currentPhase);
            if (phaseTimer >= duration)
            {
                SwitchPhase();
            }
        }

        private float GetPhaseDuration(Phase p) => p == Phase.Day ? dayDuration : nightDuration;

        private void SwitchPhase()
        {
            currentPhase = currentPhase == Phase.Day ? Phase.Night : Phase.Day;
            phaseTimer = 0f;
            RaisePhaseChanged();
        }

        private void RaisePhaseChanged() => OnPhaseChanged?.Invoke(currentPhase);

        // Debug helper for tests
        [ContextMenu("Force Switch Phase")]
        private void ForceSwitch()
        {
            SwitchPhase();
        }
    }
}
