using Game.Core;
using UnityEngine;

namespace Game.Gameplay.Interactions
{
    public sealed class DoorWithKey : MonoBehaviour
    {
        [SerializeField] private string requiredFlag = Flags.HasKey;
        [SerializeField] private bool consumeOnUse = false;

        private GameFlagManager flags = new();

        private void Awake()
        {
            // In a real project, this would be injected
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public bool TryOpen()
        {
            if (!flags.Get(requiredFlag))
            {
                Debug.Log("Door is locked. Need a key.");
                return false;
            }

            if (consumeOnUse)
            {
                flags.Set(requiredFlag, false);
            }

            Debug.Log("Door opened.");
            return true;
        }
    }
}
