using UnityEngine;
using Yarn.Unity;
using Game.Systems.DayNight;

namespace Game.NPC
{
    [RequireComponent(typeof(DialogueRunner))]
    public sealed class NpcDialogueSwitch : MonoBehaviour
    {
        public string dayNode;
        public string nightNode;

        private DialogueRunner _runner = null!;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();
            var cycle = FindObjectOfType<DayNightCycle>();
            if (cycle != null)
            {
                cycle.OnPhaseChanged += OnPhaseChanged;
                OnPhaseChanged(cycle.CurrentPhase);
            }
        }

        private void OnPhaseChanged(Phase p)
        {
            _runner.startNode = p == Phase.Day ? dayNode : nightNode;
        }
    }
}
