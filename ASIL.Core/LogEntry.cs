using System;
namespace ASIL.Core
{
    public class LogEntryBase
    {
        private readonly LogTime _logTime;

        public LogTime LogTime { get { return _logTime; } }

        public LogEntryBase(LogTime logTime)
        {
            _logTime = logTime;
        }
    }

    public class LogEntry : LogEntryBase
    {
        private readonly Application _application;
        private readonly Component _component;
        private readonly ComponentId _componentId;
        private readonly EntryType _entryType;
        private readonly EventType _eventType;
        private readonly InstanceId _instanceId;
        private readonly Level _level;
        private readonly ProcessId _processId;
        private readonly SessionId _sessionId;
        private readonly Tenant _tenant;
        private readonly UserId _userId;
        private readonly MessageBase _message;

        public Application Application { get { return _application; } }
        public Component Component { get { return _component; } }
        public ComponentId EngineId { get { return _componentId; } }
        public EntryType EntryType { get { return _entryType; } }
        public EventType EventType { get { return _eventType; } }
        public InstanceId InstanceId { get { return _instanceId; } }
        public Level Level { get { return _level; } }
        public ProcessId ProcessId { get { return _processId; } }
        public SessionId SessionId { get { return _sessionId; } }
        public Tenant Tenant { get { return _tenant; } }
        public UserId UserId { get { return _userId; } }
        public MessageBase Message { get { return _message; } }

        public LogEntry(LogTime logTime, Application application, Component component, ComponentId componentId,
            EntryType entryType, EventType eventType, InstanceId instanceId, Level level, ProcessId processId,
            SessionId sessionId, Tenant tenant, UserId userId, MessageBase message) :
            base(logTime)
        {
            _application = application;
            _component = component;
            _componentId = componentId;
            _entryType = entryType;
            _eventType = eventType;
            _instanceId = instanceId;
            _level = level;
            _processId = processId;
            _sessionId = sessionId;
            _tenant = tenant;
            _userId = userId;
            _message = message;
        }
    }

    public class LogEntryTimeEnd : LogEntryBase
    {
        private readonly TimeSpan _realTime;
        private readonly LogEntry _parentEntry;

        public Application Application { get { return _parentEntry.Application; } }
        public Component Component { get { return _parentEntry.Component; } }
        public ComponentId EngineId { get { return _parentEntry.EngineId; } }
        public EntryType EntryType { get { return _parentEntry.EntryType; } }
        public EventType EventType { get { return _parentEntry.EventType; } }
        public InstanceId InstanceId { get { return _parentEntry.InstanceId; } }
        public Level Level { get { return _parentEntry.Level; } }
        public ProcessId ProcessId { get { return _parentEntry.ProcessId; } }
        public SessionId SessionId { get { return _parentEntry.SessionId; } }
        public Tenant Tenant { get { return _parentEntry.Tenant; } }
        public UserId UserId { get { return _parentEntry.UserId; } }
        public TimeSpan TimeSpan { get { return _realTime; } }

        public LogEntryTimeEnd(LogTime logTime, LogEntry parentEntry, TimeSpan realTime) :
            base(logTime)
        {
            _parentEntry = parentEntry;
            _realTime = realTime;
        }
    }
}