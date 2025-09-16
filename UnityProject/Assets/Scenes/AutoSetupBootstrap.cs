#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
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
        root.AddComponent<StatsManager>();
        root.AddComponent<DayNightCycle>();
        root.AddComponent<SampleSetup>();

        // Dialogue runner
        var runnerGO = new GameObject("DialogueRunner");
        var runner = runnerGO.AddComponent<DialogueRunner>();
        runnerGO.AddComponent<InMemoryVariableStorage>();
        runnerGO.AddComponent<YarnRuntimeBinder>();
        runner.startNode = "start"; // you can change to "main_game"

        // Try to find a YarnProject in Resources/Yarn/Project.asset
        var project = Resources.Load<YarnProject>("Yarn/Project");
        if (project != null)
        {
            runner.yarnProject = project;
        }
        else
        {
            Debug.LogWarning("AutoSetupBootstrap: No YarnProject found at Resources/Yarn/Project. Create one and add your .yarn scripts.");
        }

        // Ensure there is a Main Camera
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
