using Game.Core;
using UnityEngine;

namespace Game.Gameplay.Work
{
    public sealed class CafeWork : MonoBehaviour
    {
        [SerializeField] private StatsManager stats = null!;
        [SerializeField] private int basePay = 25;
        [SerializeField] private int fatigueGain = 15;
        [SerializeField] private int suspicionGainIfFakeId = 5;

        // Flag link: using found_false_id implies player has been dabbling with papers
        [SerializeField] private string fakeIdFlag = Flags.FoundFalseId;

        private GameFlagManager flags = new();

        private void Awake()
        {
            Game.Systems.Dialog.YarnBridge.Init(flags);
        }

        public bool TryStartShift()
        {
            if (!stats.CanStartWork())
            {
                Debug.Log("Too tired to work.");
                return false;
            }

            // Prototype: instant resolve. In real game, start mini-game and pay out on success.
            stats.AddMoney(basePay);
            stats.AddFatigue(fatigueGain);

            if (flags.Get(fakeIdFlag))
            {
                stats.AddSuspicion(suspicionGainIfFakeId);
            }

            Debug.Log($"Shift finished. Earned {basePay}.");
            return true;
        }
    }
}
