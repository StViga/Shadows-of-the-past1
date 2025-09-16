using System.Collections.Generic;
using UnityEngine;

namespace Game.Localization
{
    public sealed class LocalizationManager : MonoBehaviour
    {
        [SerializeField] private string currentLanguage = "ru"; // "en" or "ru"
        private Dictionary<string, string> table = new();

        private void Awake()
        {
            LoadLanguage(currentLanguage);
        }

        public void LoadLanguage(string lang)
        {
            currentLanguage = lang;
            var asset = Resources.Load<TextAsset>($"Localization/{lang}");
            if (asset != null)
            {
                table = JsonUtility.FromJson<Wrapper>(Wrap(asset.text)).ToDictionary();
            }
            else
            {
                table = new Dictionary<string, string>();
            }
        }

        public string Tr(string key)
        {
            if (table.TryGetValue(key, out var v)) return v;
            return key;
        }

        // Helpers to parse simple key-value JSON into Dictionary using JsonUtility
        [System.Serializable]
        private class Pair { public string key; public string value; }
        [System.Serializable]
        private class Wrapper { public List<Pair> items = new(); public Dictionary<string, string> ToDictionary(){ var d=new Dictionary<string,string>(); foreach(var p in items) d[p.key]=p.value; return d; } }

        private static string Wrap(string rawJson)
        {
            // Expect rawJson like { "k":"v", ... } and convert to Wrapper form
            var dict = MiniJson.Deserialize(rawJson) as Dictionary<string, object>;
            var list = new List<Pair>();
            if (dict != null)
            {
                foreach (var kv in dict)
                {
                    list.Add(new Pair{ key = kv.Key, value = kv.Value?.ToString() ?? string.Empty });
                }
            }
            var wrapper = new Wrapper{ items = list };
            return JsonUtility.ToJson(wrapper);
        }
    }

    // Minimal JSON parser to avoid external dependencies
    public static class MiniJson
    {
        public static object Deserialize(string json)
        {
            return UnityEngine.JsonUtility.FromJson<Intermediate>(JsonToIntermediate(json)).ToObject();
        }

        [System.Serializable]
        private class KV { public string k; public string v; }
        [System.Serializable]
        private class Intermediate { public List<KV> items = new(); public object ToObject(){ var d=new Dictionary<string,object>(); foreach(var kv in items) d[kv.k]=kv.v; return d; } }

        private static string JsonToIntermediate(string json)
        {
            // Very naive converter for flat object json. For production, replace with robust JSON.
            var dict = new Dictionary<string, string>();
            var trimmed = json.Trim().TrimStart('{').TrimEnd('}');
            var parts = trimmed.Split(',');
            foreach(var p in parts)
            {
                var idx = p.IndexOf(":");
                if (idx <= 0) continue;
                var key = p.Substring(0, idx).Trim().Trim('"');
                var val = p.Substring(idx+1).Trim().Trim('"');
                dict[key] = val;
            }
            var kvs = new List<KV>();
            foreach(var kv in dict) kvs.Add(new KV{ k = kv.Key, v = kv.Value });
            var inter = new Intermediate{ items = kvs };
            return JsonUtility.ToJson(inter);
        }
    }
}
