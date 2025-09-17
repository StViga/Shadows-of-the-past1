using UnityEngine;
using Game.Gameplay.Interactions;

namespace Game.World
{
    public sealed class Entrance : Interactable
    {
        [Tooltip("Human-readable name for UI")] public string entranceName;
        [Tooltip("Optional Yarn node to start when entering")] public string yarnNode;
        [Tooltip("World position to place player when entering")] public Vector3 spawnOffset;

        public override string Prompt => string.IsNullOrEmpty(entranceName) ? "Enter [E]" : $"Enter {entranceName} [E]";

        public override bool Interact()
        {
            if (!string.IsNullOrEmpty(yarnNode))
            {
                var runner = Object.FindObjectOfType<Yarn.Unity.DialogueRunner>();
                if (runner != null)
                {
                    runner.StartDialogue(yarnNode);
                }
            }
            var player = GameObject.FindObjectOfType<Game.Gameplay.Player.PlayerController2D>();
            if (player != null)
            {
                player.transform.position += spawnOffset;
            }
            Debug.Log($"Entered {entranceName}");
            return true;
        }
    }
}
