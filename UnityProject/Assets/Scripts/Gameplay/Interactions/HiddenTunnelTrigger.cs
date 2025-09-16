using Game.Core;
using UnityEngine;

namespace Game.Gameplay.Interactions
{
    public sealed class HiddenTunnelTrigger : MonoBehaviour
    {
        [SerializeField] private string condA = Flags.WestFoundBlueprint;
        [SerializeField] private string condB = Flags.MorgueFatherExperiments;
        [SerializeField] private string resultFlag = Flags.FoundTruthTunnel;

        private GameFlagManager flags = new();

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public bool TryReveal()
        {
            if (flags.Get(condA) && flags.Get(condB))
            {
                flags.Set(resultFlag, true);
                Debug.Log("A hidden tunnel reveals itself.");
                return true;
            }

            Debug.Log("You sense something, but the path remains hidden.");
            return false;
        }
    }
}
