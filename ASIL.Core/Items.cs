using System;

using Infor.BI.Log;
using Mis.Reporting.Common.Engine;

namespace ASIL.Core
{
    public class Application
    {
        public string Value { get; private set; }

        public Application(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class Component
    {
        public string Value { get; private set; }

        public Component(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class ComponentId
    {
        public EngineId Value { get; private set; }

        public ComponentId(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                Value = new EngineId(Int32.Parse(value));
            }
            else
            {
                Value = EngineId.Invalid;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class EntryType
    {
        public Infor.BI.Log.EntryType Value { get; private set; }

        public EntryType(string value)
        {
            Value = (Infor.BI.Log.EntryType)Enum.Parse(typeof(Infor.BI.Log.EntryType), value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class EventType
    {
        public int Value { get; private set; }

        public EventType(string value)
        {
            Value = Int32.Parse(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class InstanceId
    {
        public string Value { get; private set; }

        public InstanceId(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class Level
    {
        public LogLevel Value { get; private set; }

        public Level(string value)
        {
            Value = (LogLevel)Enum.Parse(typeof(LogLevel), value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ProcessId
    {
        public int Value { get; private set; }

        public ProcessId(string value)
        {
            Value = Int32.Parse(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class SessionId
    {
        public string Value { get; private set; }

        public SessionId(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class Tenant
    {
        public string Value { get; private set; }

        public Tenant(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class UserId
    {
        public string Value { get; private set; }

        public UserId(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class LogTime
    {
        public DateTime Value { get; private set; }

        public LogTime(string value)
        {
            Value = DateTime.Parse(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class MessageBase
    {
    }

    public class Message : MessageBase
    {
        public string MsgText { get; private set; }

        public Message(string msgText)
        {
            MsgText = msgText;
        }

        public override string ToString()
        {
            return MsgText;
        }
    }

    public class MeasuredMessage : MessageBase
    {
        private readonly Message _parentMessage;

        public string MessageText { get { return _parentMessage.MsgText; } }
        public TimeSpan Time { get; private set; }

        public MeasuredMessage(Message message, TimeSpan time)
        {
            _parentMessage = message;
            Time = time;
        }

        public override string ToString()
        {
            return MessageText + (Time.Milliseconds * 1000).ToString() + " ms";
        }
    }
}
