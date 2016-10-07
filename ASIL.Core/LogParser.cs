using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "AddedDate(GMT+2)","Application","Component","ComponentId","EntryType","EventType","InstanceId","Level","ProcessId","SessionId","Tenant","UserId","Message"

namespace ASIL.Core
{
    public class LogParser
    {
        private enum LogItemType
        {
            Unknown,

            LogTime,
            Application,
            Component,
            ComponentId,
            EntryType,
            EventType,
            InstanceId,
            Level,
            ProcessId,
            SessionId,
            Tenant,
            UserId,
            Message
        }

        private IList<LogItemType> _logItemTypes;
        private char _itemSeparator = ',';

        private LogTimes _logTimes = new LogTimes();
        private Applications _applications = new Applications();
        private Components _components = new Components();
        private ComponentIds _componentIds = new ComponentIds();
        private EntryTypes _entryTypes = new EntryTypes();
        private EventTypes _eventTypes = new EventTypes();
        private InstanceIds _instanceIds = new InstanceIds();
        private Levels _levels = new Levels();

        private ProcessIds _processIds = new ProcessIds();
        private SessionIds _sessionIds = new SessionIds();
        private Tenants _tenants = new Tenants();
        private UserIds _userIds = new UserIds();
        // private Messages _logTimes = new LogTimes();

        public char ItemSeparator
        {
            get { return _itemSeparator; }
            set { _itemSeparator = value; }
        }

        public void ParseStream(StreamReader fileStream)
        {
            if (fileStream.EndOfStream)
            {
                return;
            }

            Task<string> lineResult;

            lineResult = fileStream.ReadLineAsync();
            _logItemTypes = ParseLogItemTipes(lineResult.Result.Split(_itemSeparator));

            while (!fileStream.EndOfStream)
            {
                lineResult = fileStream.ReadLineAsync();
                ParseLine(lineResult.Result.Split(ItemSeparator));
            }
        }

        public void Clear()
        {
        }

        private IList<string> SplitToItems(string logLine)
        {
            int itemStart = 0;
            int itemEnd = 0;
            bool isInQuotation = false;

            StringBuilder logItem = new StringBuilder(512);
            IList<string> logItems = new List<string>(16);

            for (int i = 0; i < logLine.Length; ++i)
            {
                if (logLine[i] == '"' )
                {
                    // ToDo Text in "" + "" inside " " is "
                }
            }

            return logItems;
        }

        private IList<LogItemType> ParseLogItemTipes(string[] logLine)
        {
            IList<LogItemType> logItemTypes = new List<LogItemType>(16);

            foreach (string logItemStrType in logLine)
            {
                string logItemText = GetInnerLogitemText(logItemStrType);

                switch (logItemText)
                {
                    case "Application":
                    {
                        logItemTypes.Add(LogItemType.Application);
                    }
                    break;

                    case "Component":
                    {
                        logItemTypes.Add(LogItemType.Component);
                    }
                    break;

                    case "ComponentId":
                    {
                        logItemTypes.Add(LogItemType.ComponentId);
                    }
                    break;

                    case "EntryType":
                    {
                        logItemTypes.Add(LogItemType.EntryType);
                    }
                    break;

                    case "EventType":
                    {
                        logItemTypes.Add(LogItemType.EventType);
                    }
                    break;

                    case "InstanceId":
                    {
                        logItemTypes.Add(LogItemType.InstanceId);
                    }
                    break;

                    case "Level":
                    {
                        logItemTypes.Add(LogItemType.Level);
                    }
                    break;

                    case "ProcessId":
                    {
                        logItemTypes.Add(LogItemType.ProcessId);
                    }
                    break;

                    case "SessionId":
                    {
                        logItemTypes.Add(LogItemType.SessionId);
                    }
                    break;

                    case "Tenant":
                    {
                        logItemTypes.Add(LogItemType.Tenant);
                    }
                    break;

                    case "UserId":
                    {
                        logItemTypes.Add(LogItemType.UserId);
                    }
                    break;

                    case "Message":
                    {
                        logItemTypes.Add(LogItemType.Message);
                    }
                    break;

                    default:
                    {
                        if (logItemText.StartsWith("AddedDate"))
                        {
                            int hours = 0;
                            int minutes = 0;

                            _logTimes.SetTimeShift(hours, minutes);
                            logItemTypes.Add(LogItemType.LogTime);
                        }
                        else
                        {
                            Debug.Fail($"Unsupported log '{logItemText}' type detected! Please Add it!");
                            logItemTypes.Add(LogItemType.Unknown);
                        }
                    }
                    break;
                }
            }
            return logItemTypes;
        }

        // "AddedDate(GMT+2)","Application","Component","ComponentId","EntryType","EventType","InstanceId","Level","ProcessId","SessionId","Tenant","UserId","Message"
        private void ParseLine(string[] logLine)
        {
            Debug.Assert(logLine.Length == _logItemTypes.Count, "Mishmash in log entries detected!");

            IList<LogItemType> logItemTypes = new List<LogItemType>(16);
            int idx = 0;
            foreach (string logItemStrType in logLine)
            {
                string logItemText = GetInnerLogitemText(logItemStrType);
            }
        }

        private LogEntry CreateLogEntry()
        {
            return null;
        }

        private static string GetInnerLogitemText(string logItemText)
        {
            if (logItemText.StartsWith("\"") && logItemText.EndsWith("\""))
            {
                logItemText = logItemText.Substring(1, logItemText.Length - 2);
            }

            return logItemText;
        }
    }
}
