// Placeholder for integrating Yarn Spinner runtime conditions with our GameFlagManager
using System.Collections.Generic;
using Game.Core;

namespace Game.Systems.Dialog
{
    public static class YarnBridge
    {
        private static GameFlagManager? _flags;
        public static void Init(GameFlagManager flags) => _flags = flags;

        // To be bound as Yarn function: hasFlag("morgue_father_experiments")
        public static bool HasFlag(string name) => _flags != null && _flags.Get(name);

        // To be bound as Yarn command: setFlag name true
        public static void SetFlag(string name, bool value)
        {
            _flags?.Set(name, value);
        }

        public static IEnumerable<string> AllFlags() => Flags.All;
    }
}
