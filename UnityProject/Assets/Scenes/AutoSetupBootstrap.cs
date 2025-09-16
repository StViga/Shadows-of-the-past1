#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Game.Content;
using Game.Core;
using Game.Systems.DayNight;
using Game.Systems.Dialog;

public sealed class AutoSetupBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        if (Object.FindObjectOfType<DialogueRunner>() != null) return; // already set up

        var root = new GameObject("_AutoBootstrap");

        // Core managers
        var stats = root.AddComponent<StatsManager>();
        var cycle = root.AddComponent<DayNightCycle>();
        var sample = root.AddComponent<SampleSetup>();

        // Dialog runner
        var runnerGO = new GameObject("DialogueRunner");
        var runner = runnerGO.AddComponent<DialogueRunner>();
        runner.AdditionalLanguagesToLoad = new string[] { "ru" };
        runner.startNode = "start"; // fallback
        runner.variableStorage = runnerGO.AddComponent<InMemoryVariableStorage>();
        runnerGO.AddComponent<YarnRuntimeBinder>();

        // Load all auto generated yarn files
        var textAssets = Resources.LoadAll<TextAsset>("Dialog/auto");
        foreach (var ta in textAssets)
        {
            runner.AddScript(ta);
        }

        // Make sure there is at least a Main Camera
        if (Camera.main == null)
        {
            var camGO = new GameObject("Main Camera");
            camGO.AddComponent<Camera>();
            camGO.tag = "MainCamera";
        }

        Object.DontDestroyOnLoad(root);
        Object.DontDestroyOnLoad(runnerGO);
    }
}
#endif
