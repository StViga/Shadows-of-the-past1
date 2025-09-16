using System;
using System.Collections.Generic;

namespace Game.Core
{
    public enum QuestState { NotStarted, Active, Completed, Failed }

    public sealed class Quest
    {
        public string Id { get; }
        public string Name { get; }
        public QuestState State { get; private set; } = QuestState.NotStarted;

        private readonly Func<bool> _canStart;
        private readonly Func<bool> _isComplete;
        private readonly Action _onStart;
        private readonly Action _onComplete;

        public Quest(string id, string name, Func<bool> canStart, Func<bool> isComplete, Action onStart, Action onComplete)
        {
            Id = id; Name = name;
            _canStart = canStart; _isComplete = isComplete;
            _onStart = onStart; _onComplete = onComplete;
        }

        public bool TryStart()
        {
            if (State != QuestState.NotStarted) return false;
            if (!_canStart()) return false;
            State = QuestState.Active;
            _onStart();
            return true;
        }

        public bool TryComplete()
        {
            if (State != QuestState.Active) return false;
            if (!_isComplete()) return false;
            State = QuestState.Completed;
            _onComplete();
            return true;
        }
    }

    public sealed class QuestLog
    {
        private readonly Dictionary<string, Quest> _quests = new();

        public void Add(Quest q) => _quests[q.Id] = q;
        public Quest? Get(string id) => _quests.TryGetValue(id, out var q) ? q : null;
        public IEnumerable<Quest> All() => _quests.Values;
    }
}
