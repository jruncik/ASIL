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

        private StringBuilder _itemStrBuilder = new StringBuilder(512);
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
            _logItemTypes = ParseLogItemTipes(SplitToItems(lineResult.Result));

            while (!fileStream.EndOfStream)
            {
                lineResult = fileStream.ReadLineAsync();
                ParseLine(SplitToItems(lineResult.Result));
            }
        }

        public void Clear()
        {
        }

        private IList<string> SplitToItems(string logLine)
        {
            IList<string> logItems = new List<string>(16);
            bool isInQuotation = false;

            int strEnd = logLine.Length;
            int strOneBeforeEndLength = strEnd - 1;

            _itemStrBuilder.Clear();
            for (int i = 0; i < strEnd; ++i)
            {
                char currChar = logLine[i];
                if (currChar == '"')
                {
                    if (i < strOneBeforeEndLength && logLine[i + 1] == '"')
                    {
                        _itemStrBuilder.Append('"');
                        ++i;
                    }
                    else
                    {
                        isInQuotation = !isInQuotation;
                    }
                }
                else if (currChar == _itemSeparator)
                {
                    if (isInQuotation)
                    {
                        _itemStrBuilder.Append(currChar);
                    }
                    else
                    {
                        logItems.Add(_itemStrBuilder.ToString());
                        _itemStrBuilder.Clear();
                    }
                }
                else
                {
                    _itemStrBuilder.Append(currChar);
                }
            }

            Debug.Assert(!isInQuotation, "Something go wrong we are still in quotation");
            if (!isInQuotation)
            {
                logItems.Add(_itemStrBuilder.ToString());
                _itemStrBuilder.Clear();
            }

            return logItems;
        }

        private IList<LogItemType> ParseLogItemTipes(IList<string> logLine)
        {
            IList<LogItemType> logItemTypes = new List<LogItemType>(16);

            foreach (string logItemStrType in logLine)
            {
                switch (logItemStrType)
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
                        if (logItemStrType.StartsWith("AddedDate"))
                        {
                            int hours = 0;
                            int minutes = 0;

                            _logTimes.SetTimeShift(hours, minutes);
                            logItemTypes.Add(LogItemType.LogTime);
                        }
                        else
                        {
                            Debug.Fail($"Unsupported log '{logItemStrType}' type detected! Please Add it!");
                            logItemTypes.Add(LogItemType.Unknown);
                        }
                    }
                    break;
                }
            }
            return logItemTypes;
        }

        // "AddedDate(GMT+2)","Application","Component","ComponentId","EntryType","EventType","InstanceId","Level","ProcessId","SessionId","Tenant","UserId","Message"
        private void ParseLine(IList<string> logLine)
        {
            Debug.Assert(logLine.Count == _logItemTypes.Count, "Mishmash in log entries detected!");

            IList<LogItemType> logItemTypes = new List<LogItemType>(16);
            int idx = 0;
            foreach (string logItemStrType in logLine)
            {
                Debug.Write(logItemStrType);
                Debug.Write(", ");
            }
            Debug.WriteLine(String.Empty);
        }

        private LogEntry CreateLogEntry()
        {
            return null;
        }
    }
}
