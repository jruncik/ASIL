using System;

using Infor.BI.Log;
using Mis.Reporting.Common.Engine;

namespace ASIL.Core
{
    public class Application
    {
        public string Value { get; private set; }

        public static Application Empty = new Application(String.Empty);

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

        public static Component Empty = new Component(String.Empty);

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

        public static ComponentId Empty = new ComponentId(String.Empty);

        public ComponentId(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                string engineId = value;
                if (engineId.StartsWith("Engine"))
                {
                    engineId = engineId.Substring(6).Trim();
                }
                Value = new EngineId(Int32.Parse(engineId));
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

        public static EntryType Empty = new EntryType(Infor.BI.Log.EntryType.General.ToString());

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

        public static EventType Empty = new EventType();

        public EventType(string value)
        {
            Value = Int32.Parse(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private EventType()
        {
            Value = Int32.MinValue;
        }
    }

    public class InstanceId
    {
        public string Value { get; private set; }

        public static InstanceId Empty = new InstanceId(String.Empty);

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

        public static Level Empty = new Level(LogLevel.Debug.ToString());

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

        public static ProcessId Empty = new ProcessId();

        public ProcessId(string value)
        {
            Value = Int32.Parse(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private ProcessId()
        {
            Value = Int32.MinValue;
        }
    }

    public class SessionId
    {
        public string Value { get; private set; }

        public static SessionId Empty = new SessionId(String.Empty);

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

        public static Tenant Empty = new Tenant(String.Empty);

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

        public static UserId Empty = new UserId(String.Empty);

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

        public static LogTime Empty = new LogTime();

        public LogTime(string value)
        {
            Value = DateTime.Parse(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private LogTime()
        {
            Value = DateTime.MinValue;
        }
    }

    public class MessageBase
    {
    }

    public class Message : MessageBase
    {
        public string MsgText { get; private set; }

        public static MessageBase Empty = new Message(String.Empty);

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

        public string MessageText { get { return ToString(); } }
        public TimeSpan Time { get; private set; }

        public MeasuredMessage(Message message, TimeSpan time)
        {
            _parentMessage = message;
            Time = time;
        }

        public string TimeAsStr()
        {
            Int64 miliseconds = Time.Seconds * 1000 + Time.Milliseconds;
            return String.Format("[{0} ms]", miliseconds.ToString());
        }

        public override string ToString()
        {
            Int64 miliseconds = Time.Seconds * 1000 + Time.Milliseconds;
            return _parentMessage.MsgText + " - " + TimeAsStr();
        }
    }
}
