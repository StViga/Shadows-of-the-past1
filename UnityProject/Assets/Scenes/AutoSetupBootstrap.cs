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
        if (Object.FindObjectOfType<Yarn.Unity.DialogueRunner>() != null) return; // already set up

        var root = new GameObject("_AutoBootstrap");

        // Core managers
        var stats = root.AddComponent<StatsManager>();
        var cycle = root.AddComponent<DayNightCycle>();
        var flags = new GameFlagManager();
        GameState.Init(flags, stats, null);
        Game.Systems.Dialog.YarnBridge.Init(flags);
        var invGO = new GameObject("Inventory");
        invGO.transform.SetParent(root.transform);
        var inventory = invGO.AddComponent<Game.Gameplay.Crafting.InventoryManager>();
        var craftGO = new GameObject("Crafting");
        craftGO.transform.SetParent(root.transform);
        var crafting = craftGO.AddComponent<Game.Gameplay.Crafting.CraftingSystem>();
        crafting.GetType().GetField("inventory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(crafting, inventory);
        crafting.GetType().GetField("stats", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(crafting, stats);
        var sample = root.AddComponent<SampleSetup>();

        // HUD overlay with F5/F6 save-load
        var hudGO = new GameObject("HUD");
        hudGO.transform.SetParent(root.transform);
        var hud = hudGO.AddComponent<Game.UI.HUDOverlay>();
        hud.GetType().GetField("stats", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(hud, stats);
        hud.GetType().GetField("inventory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(hud, inventory);

        // Dialogue runner
        var runnerGO = new GameObject("DialogueRunner");
        var runner = runnerGO.AddComponent<Yarn.Unity.DialogueRunner>();
        runnerGO.AddComponent<Yarn.Unity.InMemoryVariableStorage>();
        runnerGO.AddComponent<YarnRuntimeBinder>();
        runner.startNode = "start"; // you can change to "main_game"

        // Try to find a YarnProject in Resources/Yarn/Project.asset
        var project = Resources.Load<Yarn.Unity.YarnProject>("Yarn/Project");
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
