using Game.Core;
using UnityEngine;

namespace Game.Gameplay.Interactions
{
    public sealed class MorgueDoor : MonoBehaviour
    {
        [SerializeField] private StatsManager stats = null!;
        [SerializeField] private int suspicionThreshold = 40;
        [SerializeField] private string falseIdFlag = Flags.CafeFalseId;

        private GameFlagManager flags = new();

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public bool TryEnter()
        {
            if (stats.IsSuspiciousAbove(suspicionThreshold))
            {
                Debug.Log("Guard blocks the way due to high suspicion.");
                return false;
            }

            if (!flags.Get(falseIdFlag))
            {
                Debug.Log("Need a fake ID to enter the morgue at night.");
                return false;
            }

            Debug.Log("Entered the morgue.");
            return true;
        }
    }
}
