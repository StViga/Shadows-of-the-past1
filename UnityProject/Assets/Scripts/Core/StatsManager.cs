using System;
using UnityEngine;

namespace Game.Core
{
    public sealed class StatsManager : MonoBehaviour
    {
        [Header("Economy")] public int money;
        [Header("Core Meters")] [Range(0,100)] public int suspicion;
        [Range(0,100)] public int sanity = 100;
        [Range(0,100)] public int fatigue;

        public event Action<int> OnMoneyChanged;
        public event Action<int> OnSuspicionChanged;
        public event Action<int> OnSanityChanged;
        public event Action<int> OnFatigueChanged;

        public void AddMoney(int amount)
        {
            money += amount;
            if (money < 0) money = 0;
            OnMoneyChanged?.Invoke(money);
        }

        public void AddSuspicion(int delta)
        {
            suspicion = Mathf.Clamp(suspicion + delta, 0, 100);
            OnSuspicionChanged?.Invoke(suspicion);
        }

        public void AddSanity(int delta)
        {
            sanity = Mathf.Clamp(sanity + delta, 0, 100);
            OnSanityChanged?.Invoke(sanity);
        }

        public void AddFatigue(int delta)
        {
            fatigue = Mathf.Clamp(fatigue + delta, 0, 100);
            OnFatigueChanged?.Invoke(fatigue);
        }

        public bool CanStartWork() => fatigue <= 80;
        public bool IsSuspiciousAbove(int threshold) => suspicion >= threshold;
    }
}
