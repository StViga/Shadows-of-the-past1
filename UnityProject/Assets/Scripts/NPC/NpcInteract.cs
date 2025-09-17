using UnityEngine;
using Game.Systems.DayNight;

namespace Game.NPC
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class NpcInteract : Game.Gameplay.Interactions.Interactable
    {
        [Tooltip("Yarn node during the day")] public string dayNode;
        [Tooltip("Yarn node during the night")] public string nightNode;

        public override string Prompt => "Talk [E]";

        public override bool Interact()
        {
            var runner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            if (runner == null) return false;
            var cycle = FindObjectOfType<DayNightCycle>();
            var node = cycle != null && cycle.CurrentPhase == Phase.Night && !string.IsNullOrEmpty(nightNode)
                ? nightNode
                : dayNode;
            if (string.IsNullOrEmpty(node)) return false;
            runner.StartDialogue(node);
            return true;
        }
    }
}
