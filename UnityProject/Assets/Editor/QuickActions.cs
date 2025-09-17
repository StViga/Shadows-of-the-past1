#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Game.Gameplay.Systems;
using Game.Gameplay.Crafting;
using Game.Core;

public static class QuickActions
{
    [MenuItem("Tools/Gameplay/Grant Fat & Thread")]
    public static void GrantBasics()
    {
        var inv = Object.FindObjectOfType<InventoryManager>();
        if (inv == null) return;
        inv.Add(ResourceIds.Fat, 2);
        inv.Add(ResourceIds.Thread, 2);
        Debug.Log("Granted basic resources.");
    }

    [MenuItem("Tools/Gameplay/Perform Candle Ritual")] 
    public static void DoRitual()
    {
        var night = Object.FindObjectOfType<NightActivitiesSystem>();
        if (night == null) return;
        if (!night.UseCandleForRitual()) Debug.Log("Need a candle.");
    }

    [MenuItem("Tools/Gameplay/Find Morgue DNA clue")] 
    public static void ClueDNA()
    {
        var day = Object.FindObjectOfType<DayActivitiesSystem>();
        if (day == null) return;
        day.FindClue(Flags.MorgueFatherExperiments);
    }
}
#endif