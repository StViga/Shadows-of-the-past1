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
            _runner.AddFunction<bool, string>("HasFlag", YarnBridge.HasFlag);

            // Commands
            _runner.AddCommandHandler<string, bool>("SetFlag", YarnBridge.SetFlag);
        }
    }
}
#endif
