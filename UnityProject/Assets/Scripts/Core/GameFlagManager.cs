using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Game.Core
{
    public class GameFlagManager
    {
        private readonly HashSet<string> _flags = new();

        public event Action<string, bool>? OnFlagChanged;

        public bool Get(string flag) => _flags.Contains(flag);

        public bool Set(string flag, bool value)
        {
            bool changed = false;
            if (value)
            {
                if (_flags.Add(flag)) changed = true;
            }
            else
            {
                if (_flags.Remove(flag)) changed = true;
            }

            if (changed) OnFlagChanged?.Invoke(flag, value);
            return changed;
        }

        public void SetMany(IEnumerable<(string flag, bool value)> pairs)
        {
            foreach (var p in pairs) Set(p.flag, p.value);
        }

        public IEnumerable<string> Enumerate() => _flags;

        // Simple JSON persistence for prototype
        public void Save(string path)
        {
            var data = new List<string>(_flags);
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public void Load(string path)
        {
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            _flags.Clear();
            foreach (var f in data) _flags.Add(f);
        }
    }
}
