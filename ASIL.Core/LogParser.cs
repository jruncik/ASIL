using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ASIL.Core
{
    public class LogParser
    {
        private readonly IList<LogEntryBase> _logEntries = new List<LogEntryBase>(1024);

        private char _itemSeparator = ',';
        private StringBuilder _itemStrBuilder = new StringBuilder(512);
        private LogEntryItemsHelper _logEntryItemHelper;

        public char ItemSeparator
        {
            get { return _itemSeparator; }
            set { _itemSeparator = value; }
        }

        public IList<LogEntryBase> LogEntries
        {
            get { return _logEntries; }
        }

        public void ParseStream(StreamReader fileStream)
        {
            if (fileStream.EndOfStream)
            {
                return;
            }

            Task<string> lineResult = fileStream.ReadLineAsync();

            ParseLogItemTipes(SplitToItems(lineResult.Result));

            while (!fileStream.EndOfStream)
            {
                lineResult = fileStream.ReadLineAsync();
                ParseLine(SplitToItems(lineResult.Result));
            }
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

        private void ParseLogItemTipes(IList<string> logLine)
        {
            _logEntryItemHelper = new LogEntryItemsHelper(logLine.Count);

            foreach (string logItemStrType in logLine)
            {
                switch (logItemStrType)
                {
                    case "Application":
                    {
                        _logEntryItemHelper.AddItemType(new Applications(), typeof(Application));
                    }
                    break;

                    case "Component":
                    {
                        _logEntryItemHelper.AddItemType(new Components(), typeof(Component));
                    }
                    break;

                    case "ComponentId":
                    {
                        _logEntryItemHelper.AddItemType(new ComponentIds(), typeof(ComponentId));
                    }
                    break;

                    case "EntryType":
                    {
                        _logEntryItemHelper.AddItemType(new EntryTypes(), typeof(ASIL.Core.EntryType));
                    }
                    break;

                    case "EventType":
                    {
                        _logEntryItemHelper.AddItemType(new EventTypes(), typeof(EventType));
                    }
                    break;

                    case "InstanceId":
                    {
                        _logEntryItemHelper.AddItemType(new InstanceIds(), typeof(InstanceId));
                    }
                    break;

                    case "Level":
                    {
                        _logEntryItemHelper.AddItemType(new Levels(), typeof(Level));
                    }
                    break;

                    case "ProcessId":
                    {
                        _logEntryItemHelper.AddItemType(new ProcessIds(), typeof(ProcessId));
                    }
                    break;

                    case "SessionId":
                    {
                        _logEntryItemHelper.AddItemType(new SessionIds(), typeof(SessionId));
                    }
                    break;

                    case "Tenant":
                    {
                        _logEntryItemHelper.AddItemType(new Tenants(), typeof(Tenant));
                    }
                    break;

                    case "UserId":
                    {
                        _logEntryItemHelper.AddItemType(new UserIds(), typeof(UserId));
                    }
                    break;

                    case "Message":
                    {
                        _logEntryItemHelper.AddItemType(new Messages(), typeof(MessageBase));
                    }
                    break;

                    default:
                    {
                        if (logItemStrType.StartsWith("AddedDate"))
                        {
                            int hours = 0;
                            int minutes = 0;

                            LogTimes logTimes = new LogTimes();
                            logTimes.SetTimeShift(hours, minutes);

                            _logEntryItemHelper.AddItemType(logTimes, typeof(LogTime));
                        }
                        else
                        {
                            Debug.Fail($"Unsupported log '{logItemStrType}' type detected! Please Add it!");
                            _logEntryItemHelper.AddItemType(new UnknowLogItems(), typeof(object));
                        }
                    }
                    break;
                }
            }
        }

        private void ParseLine(IList<string> logEntryLine)
        {
            LogEntryBase logEntry = _logEntryItemHelper.CreateCurrentLogEntry(logEntryLine);
            _logEntries.Add(logEntry);
        }
    }
}
