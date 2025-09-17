using UnityEngine;
using Game.Core;
using Game.Systems.DayNight;

namespace Game.Gameplay.World
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ClueNode : Game.Gameplay.Interactions.Interactable
    {
        public string clueFlag = Flags.FoundArticle;
        public bool dayOnly = true;
        private bool _found;

        public override string Prompt => _found ? "Already found" : "Investigate [E]";

        private void Awake()
        {
            var cycle = Object.FindObjectOfType<DayNightCycle>();
            if (cycle != null) cycle.OnPhaseChanged += phase => { if (phase == Phase.Day) _found = false; };
        }

        public override bool Interact()
        {
            if (_found) return false;
            var cycle = Object.FindObjectOfType<DayNightCycle>();
            if (dayOnly && cycle != null && cycle.CurrentPhase != Phase.Day) return false;
            var flags = Game.Core.GameState.Flags;
            if (flags.Set(clueFlag, true))
            {
                _found = true;
                Debug.Log($"Clue discovered: {clueFlag}");
                return true;
            }
            return false;
        }
    }
}
