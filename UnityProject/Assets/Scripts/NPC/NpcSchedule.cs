using System;
using UnityEngine;
using Game.Systems.DayNight;

namespace Game.NPC
{
    public enum NpcPresence { Hidden, Present }

    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class NpcSchedule : MonoBehaviour
    {
        [Header("Hours (24h)")] public int dayStart = 8; public int dayEnd = 18;
        [Header("Overrides")] public bool presentAtNight;

        private SpriteRenderer _sr = null!;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            var cycle = FindObjectOfType<DayNightCycle>();
            if (cycle != null)
            {
                cycle.OnPhaseChanged += OnPhase;
                OnPhase(cycle.CurrentPhase);
            }
        }

        private void OnPhase(Phase p)
        {
            bool show = p == Phase.Day ? true : presentAtNight;
            _sr.enabled = show;
            foreach (var c in GetComponentsInChildren<Renderer>()) c.enabled = show;
            foreach (var c in GetComponentsInChildren<Collider2D>()) c.enabled = show;
        }
    }
}
