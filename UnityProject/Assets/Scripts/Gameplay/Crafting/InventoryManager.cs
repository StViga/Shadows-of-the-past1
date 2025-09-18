using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Crafting
{
    public sealed class InventoryManager : MonoBehaviour
    {
        private readonly Dictionary<string, int> _items = new Dictionary<string, int>();

        public int GetCount(string id) => _items.TryGetValue(id, out var c) ? c : 0;
        public bool Has(string id, int amount) => GetCount(id) >= amount;

        public void Add(string id, int amount)
        {
            if (!_items.ContainsKey(id)) _items[id] = 0;
            _items[id] += amount;
        }

        public bool Remove(string id, int amount)
        {
            if (!Has(id, amount)) return false;
            _items[id] -= amount;
            if (_items[id] <= 0) _items.Remove(id);
            return true;
        }

        public IReadOnlyDictionary<string, int> Dump() => _items;
    }
}
