using System;

namespace ASIL.Core
{
    public class Application
    {
        public string Name { get; private set; }

        public Application(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Component
    {
        public string Name { get; private set; }

        public Component(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class EntryType
    {
        public string Name { get; private set; }

        public EntryType(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class EventType
    {
        public string Name { get; private set; }

        public EventType(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class InstanceId
    {
        public string Name { get; private set; }

        public InstanceId(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Level
    {
        public string Name { get; private set; }

        public Level(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ProcessId
    {
        public string Name { get; private set; }

        public ProcessId(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class SessionId
    {
        public string Name { get; private set; }

        public SessionId(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ComponentId
    {
        public string Name { get; private set; }

        public ComponentId(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Tenant
    {
        public string Name { get; private set; }

        public Tenant(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class UserId
    {
        public string Name { get; private set; }

        public UserId(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class LogTime
    {
        public DateTime Value { get; private set; }

        public LogTime(DateTime value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
