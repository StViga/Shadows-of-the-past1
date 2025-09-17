#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using Yarn.Unity;
using Game.Core;

namespace Game.Systems.Dialog
{
    [RequireComponent(typeof(DialogueRunner))]
    public sealed class YarnRuntimeBinder : MonoBehaviour
    {
        private DialogueRunner _runner = null!;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();

            // Functions
            // In Yarn 2.x, generic order is <TParam1, TResult>
            _runner.AddFunction<string, bool>("HasFlag", YarnBridge.HasFlag);

            // Commands
            _runner.AddCommandHandler<string, bool>("SetFlag", YarnBridge.SetFlag);
            _runner.AddCommandHandler<string, int>("GrantItem", YarnBridge.GrantItem);
            _runner.AddCommandHandler("StartRitual", YarnBridge.StartRitual);
            _runner.AddCommandHandler<string>("FindClue", YarnBridge.FindClue);
        }
    }
}
#endif
