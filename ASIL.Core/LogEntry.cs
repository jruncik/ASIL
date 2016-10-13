using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ASIL.Core
{
    public class LogEntries : INotifyPropertyChanged
    {
        private readonly IList<LogEntryBase> _logEntries = new List<LogEntryBase>(1024);

        public IList<LogEntryBase> Entries
        {
            get { return _logEntries; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    }

    public abstract class LogEntryBase
    {
        private readonly LogTime _logTime;

        public LogTime LogTime { get { return _logTime; } }
        public abstract Application Application { get; }
        public abstract Component Component { get; }
        public abstract ComponentId EngineId { get; }
        public abstract EntryType EntryType { get; }
        public abstract EventType EventType { get; }
        public abstract InstanceId InstanceId { get; }
        public abstract Level Level { get; }
        public abstract ProcessId ProcessId { get; }
        public abstract SessionId SessionId { get; }
        public abstract Tenant Tenant { get; }
        public abstract UserId UserId { get; }
        public abstract MessageBase Message { get; }
        public abstract TimeSpan TimeSpan { get; }

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

        public override Application Application { get { return _application; } }
        public override Component Component { get { return _component; } }
        public override ComponentId EngineId { get { return _componentId; } }
        public override EntryType EntryType { get { return _entryType; } }
        public override EventType EventType { get { return _eventType; } }
        public override InstanceId InstanceId { get { return _instanceId; } }
        public override Level Level { get { return _level; } }
        public override ProcessId ProcessId { get { return _processId; } }
        public override SessionId SessionId { get { return _sessionId; } }
        public override Tenant Tenant { get { return _tenant; } }
        public override UserId UserId { get { return _userId; } }
        public override MessageBase Message { get { return _message; } }
        public override TimeSpan TimeSpan { get { return TimeSpan.Zero; } }

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

        public override Application Application { get { return _parentEntry.Application; } }
        public override Component Component { get { return _parentEntry.Component; } }
        public override ComponentId EngineId { get { return _parentEntry.EngineId; } }
        public override EntryType EntryType { get { return _parentEntry.EntryType; } }
        public override EventType EventType { get { return _parentEntry.EventType; } }
        public override InstanceId InstanceId { get { return _parentEntry.InstanceId; } }
        public override Level Level { get { return _parentEntry.Level; } }
        public override ProcessId ProcessId { get { return _parentEntry.ProcessId; } }
        public override SessionId SessionId { get { return _parentEntry.SessionId; } }
        public override Tenant Tenant { get { return _parentEntry.Tenant; } }
        public override UserId UserId { get { return _parentEntry.UserId; } }
        public override MessageBase Message { get { return _parentEntry.Message; } }
        public override TimeSpan TimeSpan { get { return _realTime; } }

        public LogEntryTimeEnd(LogTime logTime, LogEntry parentEntry, TimeSpan realTime) :
            base(logTime)
        {
            _parentEntry = parentEntry;
            _realTime = realTime;
        }
    }
}