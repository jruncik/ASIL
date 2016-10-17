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

                using (StreamReader sr = new StreamReader(ofd.FileName))
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
                    data.Add(new Record(
                        new Property("Log Time", logEntry.LogTime.ToString()),
                        new Property("ComponentId", logEntry.EngineId.ToString()),
                        new Property("Message", message)
                        ));
                }
            }

            return data;
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
            GenerateColumnas(data);
            dataGrid.ItemsSource = data;
            dataGrid.Items.Refresh();
        }

        private void GenerateColumnas(ObservableCollection<Record> data)
        {
            dataGrid.Columns.Clear();

            if (data.Count == 0)
            {
                return;
            }

            var columns = data.First().Properties.Select((x, i) => new { Name = x.Name, Index = i }).ToArray();
            foreach (var item in columns)
            {
                var binding = new Binding(string.Format("Properties[{0}].Value", item.Index));
                dataGrid.Columns.Add(new DataGridTextColumn() { Header = item.Name, Binding = binding });
            }
        }
    }
}
