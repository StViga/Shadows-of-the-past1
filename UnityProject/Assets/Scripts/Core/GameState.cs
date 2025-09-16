using Game.Localization;

namespace Game.Core
{
    // Simple global state holder for prototype. In production prefer DI or a proper service locator.
    public static class GameState
    {
        public static GameFlagManager Flags { get; private set; } = new();
        public static StatsManager? Stats { get; private set; }
        public static LocalizationManager? Localization { get; private set; }

        public static void Init(GameFlagManager flags, StatsManager? stats = null, LocalizationManager? loc = null)
        {
            Flags = flags;
            if (stats != null) Stats = stats;
            if (loc != null) Localization = loc;
        }
    }
}
