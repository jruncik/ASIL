﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ASIL.Core
{
    internal class LogEntryItemsHelper
    {
        private class MessageTimeInfo
        {
            internal string MessageText { get; private set; }
            internal TimeSpan Time { get; private set; }

            internal MessageTimeInfo(string message, TimeSpan time)
            {
                MessageText = message;
                Time = time;
            }
        }

        private readonly int _itemsCount;
        private readonly IList<IItemsCollection> _typedCollections;
        private readonly object[] _currentLogEntryItems;

        private int _logTimeIdx = -1;
        private int _applicationIdx = -1;
        private int _componentIdx = -1;
        private int _componentIdIdx = -1;
        private int _entryTypeIdx = -1;
        private int _eventTypeIdx = -1;
        private int _instanceIdIdx = -1;
        private int _levelIdx = -1;
        private int _processIdIdx = -1;
        private int _sessionIdIdx = -1;
        private int _tenantIdx = -1;
        private int _userIdIdx = -1;
        private int _messageBaseIdx = -1;

        internal LogEntryItemsHelper(int itemsCount)
        {
            _itemsCount = itemsCount;
            _typedCollections = new List<IItemsCollection>(_itemsCount);
            _currentLogEntryItems = new object[_itemsCount];
        }

        internal void ClearCurrentEntryItems()
        {
            for (int i = 0; i < _itemsCount; ++i)
            {
                _currentLogEntryItems[i] = null;
            }
        }

        internal void AddItemType(IItemsCollection typedCollection, Type itemType)
        {
            int idx = _typedCollections.Count;
            _typedCollections.Add(typedCollection);

            if (itemType == typeof(LogTime))
            {
                _logTimeIdx = idx;
            }
            else if (itemType == typeof(Application))
            {
                _applicationIdx = idx;
            }
            else if (itemType == typeof(Component))
            {
                _componentIdx = idx;
            }
            else if (itemType == typeof(ComponentId))
            {
                _componentIdIdx = idx;
            }
            else if (itemType == typeof(ASIL.Core.EntryType))
            {
                _entryTypeIdx = idx;
            }
            else if (itemType == typeof(EventType))
            {
                _eventTypeIdx = idx;
            }
            else if (itemType == typeof(InstanceId))
            {
                _instanceIdIdx = idx;
            }
            else if (itemType == typeof(Level))
            {
                _levelIdx = idx;
            }
            else if (itemType == typeof(ProcessId))
            {
                _processIdIdx = idx;
            }
            else if (itemType == typeof(SessionId))
            {
                _sessionIdIdx = idx;
            }
            else if (itemType == typeof(Tenant))
            {
                _tenantIdx = idx;
            }
            else if (itemType == typeof(UserId))
            {
                _userIdIdx = idx;
            }
            else if (itemType == typeof(MessageBase))
            {
                _messageBaseIdx = idx;
            }
        }

        internal LogEntryBase CreateCurrentLogEntry(IList<string> logEntryLine)
        {
            if (!ReadCurrentLogEntryItem(logEntryLine))
            {
                return null;
            }

            EntryType entryType = (EntryType)_currentLogEntryItems[_entryTypeIdx];
            switch (entryType.Value)
            {
                case Infor.BI.Log.EntryType.TimeStart:
                {
                    return new LogEntry(LogTime, Application, Component, EngineId, EntryType, EventType, InstanceId, Level, ProcessId, SessionId, Tenant, UserId, Message);
                }

                case Infor.BI.Log.EntryType.TimeEnd:
                {
                    return new LogEntry(LogTime, Application, Component, EngineId, EntryType, EventType, InstanceId, Level, ProcessId, SessionId, Tenant, UserId, Message);
                }
            }
            return new LogEntry(LogTime, Application, Component, EngineId, EntryType, EventType, InstanceId, Level, ProcessId, SessionId, Tenant, UserId, Message);
        }

        private bool ReadCurrentLogEntryItem(IList<string> logEntryLine)
        {
            Debug.Assert(logEntryLine.Count == _itemsCount, "Mishmash in log entries detected!");
            Debug.Assert(logEntryLine.Count == _typedCollections.Count, "Mishmash in log entries detected!");

            if (logEntryLine.Count != _itemsCount || logEntryLine.Count != _typedCollections.Count)
            {
                return false;
            }

            for (int i = 0; i < _itemsCount; ++i)
            {
                string logEntryItem = logEntryLine[i];
                IItemsCollection itemsCollection = _typedCollections[i];
                if (i != _messageBaseIdx)
                {
                    _currentLogEntryItems[i] = itemsCollection.GetOrAddAsObject(logEntryItem);
                    continue;
                }

                // Handle message

                Messages messagesCollection = (Messages)itemsCollection;

                if (logEntryItem.EndsWith(" ms"))
                {
                    MessageTimeInfo msgWithTime = TryGetMessageWithTime(logEntryItem);
                    if (msgWithTime != null)
                    {
                        // Message without miliseconds doesn't exist. Store it as it was.
                        Message parentMessage = (Message)messagesCollection.GetAsObject(msgWithTime.MessageText);
                        if (parentMessage != null)
                        {
                            MeasuredMessage measuredMsg = new MeasuredMessage(parentMessage, msgWithTime.Time);
                            _currentLogEntryItems[i] = messagesCollection.AddTimeMessage(measuredMsg);
                            continue;
                        }
                    }
                }

                if (messagesCollection.GetAsObject(logEntryLine[i]) == null)
                {
                    _currentLogEntryItems[i] = messagesCollection.GetOrAddAsObject(logEntryItem);
                    continue;
                }
            }
            return true;
        }

        private LogTime LogTime { get { return (LogTime)_currentLogEntryItems[_logTimeIdx]; } }
        private Application Application { get { return (Application)_currentLogEntryItems[_applicationIdx]; } }
        private Component Component { get { return (Component)_currentLogEntryItems[_componentIdx]; } }
        private ComponentId EngineId { get { return (ComponentId)_currentLogEntryItems[_componentIdIdx]; } }
        private EntryType EntryType { get { return (EntryType)_currentLogEntryItems[_entryTypeIdx]; } }
        private EventType EventType { get { return (EventType)_currentLogEntryItems[_eventTypeIdx]; } }
        private InstanceId InstanceId { get { return (InstanceId)_currentLogEntryItems[_instanceIdIdx]; } }
        private Level Level { get { return (Level)_currentLogEntryItems[_levelIdx]; } }
        private ProcessId ProcessId { get { return (ProcessId)_currentLogEntryItems[_processIdIdx]; } }
        private SessionId SessionId { get { return (SessionId)_currentLogEntryItems[_sessionIdIdx]; } }
        private Tenant Tenant { get { return (Tenant)_currentLogEntryItems[_tenantIdx]; } }
        private UserId UserId { get { return (UserId)_currentLogEntryItems[_userIdIdx]; } }
        private MessageBase Message { get { return (MessageBase)_currentLogEntryItems[_messageBaseIdx]; } }

        private MessageTimeInfo TryGetMessageWithTime(string logEntryItem)
        {
            int milisStartIdx = logEntryItem.LastIndexOf(':');
            if (milisStartIdx <= 0)
            {
                return null;
            }

            string message = logEntryItem.Substring(0, milisStartIdx);
            string milsiTime = logEntryItem.Substring(milisStartIdx + 1, logEntryItem.Length - 3 - milisStartIdx).Trim();

            int miliseconds = 0;
            if (!Int32.TryParse(milsiTime, out miliseconds))
            {
                return null;
            }

            return new MessageTimeInfo(message, new TimeSpan(0, 0, 0, 0, miliseconds));
        }
    }
}