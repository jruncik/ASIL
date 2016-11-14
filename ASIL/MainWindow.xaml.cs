using System.Collections.Generic;
using System.IO;
using System.Windows;

using Microsoft.Win32;

using ASIL.Core;
using System.Collections.ObjectModel;
using ASIL.DynamicModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Controls;
using System;
using System.Diagnostics;

namespace ASIL
{
    [Flags]
    internal enum LogFilter
    {
        None        = 0x00,

        Load        = 0x01,
        Time        = 0x02,
        Calculate   = 0x04,

        All         = 0xFF
    }

    public partial class MainWindow : Window
    {
        private static string AppName = "ASIL";

        private LogParser _logParser = new LogParser();
        private LogFilter _logFilter = LogFilter.All;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"Log Files|*.csv";
            if (!ofd.ShowDialog(this).Value)
            {
                return;
            }

            try
            {
                _logParser.Clear();

                using (StreamReader sr = new StreamReader(new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    _logParser.ParseStream(sr);
                }

                GenerateViewData();
                Title = AppName + " - " + ofd.FileName;
            }
            catch (System.Exception)
            {
                Title = AppName;
                dataGrid.ItemsSource = null;
                _logParser.Clear();
            }

            dataGrid.Items.Refresh();
        }

        private void checkBoxAll_Checked(object sender, RoutedEventArgs e)
        {
            if (IsChecked(checkBoxAll))
            {
                _logFilter = LogFilter.All;
            }
            else
            {
                _logFilter = LogFilter.None;

                if (IsChecked(checkBoxLoad))
                {
                    _logFilter |= LogFilter.Load;
                }

                if (IsChecked(checkBoxTime))
                {
                    _logFilter |= LogFilter.Time;
                }

                if (IsChecked(checkBoxCalculte))
                {
                    _logFilter |= LogFilter.Calculate;
                }
            }

            GenerateViewData();
        }

        private void checkBoxLoad_Checked(object sender, RoutedEventArgs e)
        {
            if (IsChecked(checkBoxLoad))
            {
                _logFilter |= LogFilter.Load;
            }
            else
            {
                _logFilter &= ~LogFilter.Load;
            }

            GenerateViewData();
        }

        private void checkBoxTime_Checked(object sender, RoutedEventArgs e)
        {
            if (IsChecked(checkBoxTime))
            {
                _logFilter |= LogFilter.Time;
            }
            else
            {
                _logFilter &= ~LogFilter.Time;
            }

            GenerateViewData();
        }

        private void checkBoxCalculte_Checked(object sender, RoutedEventArgs e)
        {
            if (IsChecked(checkBoxCalculte))
            {
                _logFilter |= LogFilter.Calculate;
            }
            else
            {
                _logFilter &= ~LogFilter.Calculate;
            }

            GenerateViewData();
        }

        private bool IsChecked(CheckBox checkBox)
        {
            if (checkBox == null)
            {
                return false;
            }

            if (!checkBox.IsChecked.HasValue)
            {
                return false;
            }

            return checkBox.IsChecked.Value;
        }

        private ObservableCollection<Record> GenerateEnginesViewData(LogParser logParser)
        {
            ObservableCollection<Record> data = new ObservableCollection<Record>();

            int columnsCount = logParser.GetEnginesCount() + 1;
            object[] properties = null;

            foreach (LogEntry logEntry in logParser.LogEntries)
            {
                if (logEntry.Message == null)
                {
                    continue;
                }

                string message = logEntry.Message.ToString();
                bool addEntry = IsEntryValidForOutput(message);

                if (addEntry)
                {
                    // if doesn't exist or if the time is different create new one ...
                    if (properties == null)
                    {
                        properties = InitializeNewPropertiesRecord(columnsCount, logEntry.LogTime);
                    }

                    int engineIdx = logEntry.EngineId.Value.GetHashCode() + 2;    // Hash-code of EngineId = engineID, [0] LogTime, [1] = engine -1 (non specified Engine)

                    if (properties[engineIdx] != null || (string)(properties[0]) != logEntry.LogTime.ToString())  // already occupied
                    {
                        data.Add(new Record(properties));
                        properties = InitializeNewPropertiesRecord(columnsCount, logEntry.LogTime);
                    }

                    properties[engineIdx] = message;
                }
            }

            return data;
        }

        object[] InitializeNewPropertiesRecord(int columnsCount, LogTime entryLogTime)
        {
            object[] properties = new object[columnsCount];
            properties[0] = entryLogTime.ToString();

            return properties;
        }

        private bool IsEntryValidForOutput(string message)
        {
            if ((int)(_logFilter & LogFilter.All) == (int)LogFilter.All)
            {
                return true;
            }

            bool addEntry = false;

            if ((int)(_logFilter & LogFilter.Load) == (int)LogFilter.Load)
            {
                addEntry |= message.StartsWith(@"[ReportLoading]");
            }

            if (!addEntry && (int)(_logFilter & LogFilter.Time) == (int)LogFilter.Time)
            {
                addEntry |= message.StartsWith(@"Generating html");
                addEntry |= message.StartsWith(@"Rendering report");
            }

            if ((int)(_logFilter & LogFilter.Calculate) == (int)LogFilter.Calculate)
            {
                addEntry |= message.Contains(@"Calculating hyperblock");
            }

            return addEntry;
        }

        private void GenerateViewData()
        {
            if (dataGrid == null)
            {
                return;
            }

            ObservableCollection<Record> data = GenerateEnginesViewData(_logParser);
            GenerateColumnas(data, _logParser);
            dataGrid.ItemsSource = data;
            dataGrid.Items.Refresh();
        }

        private void GenerateColumnas(ObservableCollection<Record> data, LogParser logParser)
        {
            dataGrid.Columns.Clear();

            if (data.Count == 0)
            {
                return;
            }

            string[] columnNames = GenerateColumnNames(logParser);

            var columns = data.First().Properties.Select((x, i) => new { Index = i }).ToArray();

            foreach (var item in columns)
            {
                Binding binding = new Binding(string.Format("Properties[{0}]", item.Index));
                int width = item.Index == 0 ? 120 : 200;
                dataGrid.Columns.Add(new DataGridTextColumn() { Header = columnNames[item.Index], Binding = binding , Width = width });
            }
        }

        private static string[] GenerateColumnNames(LogParser logParser)
        {
            int columnsCount = logParser.GetEnginesCount() + 1;
            string[] columnNames = new string[columnsCount];

            columnNames[0] = "Log Time";
            columnNames[1] = "Unknown Engine";

            for (int i = 2; i < columnsCount; ++i)
            {
                columnNames[i] = String.Format("Engine_{0}", i - 1);
            }

            return columnNames;
        }
    }
}
