namespace ASIL.Core
{
    internal class LogEntry
    {
        private LogTime _logTime;
        private Application _application;
        private Component _component;
        private ComponentId _componentId;
        private EntryType _entryType;
        private EventType _eventType;
        private InstanceId _instanceId;
        private Level _level;
        private ProcessId _processId;
        private SessionId _sessionId;
        private Tenant _tenant;
        private UserId _userId;
    }
}