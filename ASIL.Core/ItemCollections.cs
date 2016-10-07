﻿using System;
using System.Collections.Generic;

namespace ASIL.Core
{
    //"AddedDate(GMT+2)","ComponentId","Tenant","UserId","Message"

    internal class ItemsCollections<T>
    {
        private readonly Func<string, T> _creator;
        private IDictionary<string, T> _items = new Dictionary<string, T>();

        internal ItemsCollections(Func<string, T> creator)
        {
            _creator = creator;
        }

        internal T GetOrAdd(string itemValue)
        {
            if (!_items.ContainsKey(itemValue))
            {
                _items[itemValue] = _creator(itemValue);
            }

            return _items[itemValue];
        }

        internal void Clear()
        {
            _items.Clear();
        }
    }

    internal class LogTimes : ItemsCollections<LogTime>
    {
        private int _timeShiftHours;
        private int _timeShiftMinutes;

        internal LogTimes() :
            base((itemValue) => {
                DateTime logTime = DateTime.Parse(itemValue);
                logTime = DateTime.Parse(itemValue);
                return new LogTime(logTime);
            })
        {
        }

        internal void SetTimeShift(int hours, int minutes)
        {
            _timeShiftHours = hours;
            _timeShiftMinutes = minutes;
        }
    }

    internal class Applications : ItemsCollections<Application>
    {
        internal Applications() :
            base((itemValue) => { return new Application(itemValue); })
        {
        }
    }

    internal class Components : ItemsCollections<Component>
    {
        internal Components() :
            base((itemValue) => { return new Component(itemValue); })
        {
        }
    }

    internal class ComponentIds : ItemsCollections<ComponentId>
    {
        internal ComponentIds() :
            base((itemValue) => { return new ComponentId(itemValue); })
        {
        }
    }

    internal class EntryTypes : ItemsCollections<EntryType>
    {
        internal EntryTypes() :
            base((itemValue) => { return new EntryType(itemValue); })
        {
        }
    }

    internal class EventTypes : ItemsCollections<EventType>
    {
        internal EventTypes() :
            base((itemValue) => { return new EventType(itemValue); })
        {
        }
    }

    internal class InstanceIds : ItemsCollections<InstanceId>
    {
        internal InstanceIds() :
            base((itemValue) => { return new InstanceId(itemValue); })
        {
        }
    }

    internal class Levels : ItemsCollections<Level>
    {
        internal Levels() :
            base((itemValue) => { return new Level(itemValue); })
        {
        }
    }

    internal class ProcessIds : ItemsCollections<ProcessId>
    {
        internal ProcessIds() :
            base((itemValue) => { return new ProcessId(itemValue); })
        {
        }
    }

    internal class SessionIds : ItemsCollections<SessionId>
    {
        internal SessionIds() :
            base((itemValue) => { return new SessionId(itemValue); })
        {
        }
    }

    internal class Tenants : ItemsCollections<Tenant>
    {
        internal Tenants() :
            base((itemValue) => { return new Tenant(itemValue); })
        {
        }
    }

    internal class UserIds : ItemsCollections<UserId>
    {
        internal UserIds() :
            base((itemValue) => { return new UserId(itemValue); })
        {
        }
    }
}