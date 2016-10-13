using System.Collections.Generic;
using System.IO;
using System.Windows;

using Microsoft.Win32;

using ASIL.Core;
using System.Collections.ObjectModel;

namespace ASIL
{
    public partial class MainWindow : Window
    {
        private readonly LogParser _logParser = new LogParser();
        private ObservableCollection<LogEntryBase> _logEntries = new ObservableCollection<LogEntryBase>();

        public MainWindow()
        {
            InitializeComponent();

            dataGrid.ItemsSource = _logEntries;
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"Log Files|*.csv";
            if (!ofd.ShowDialog(this).Value)
            {
                return;
            }

            _logParser.Clear();
            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                _logParser.ParseStream(sr);
            }

            _logEntries.Clear();
            foreach (LogEntryBase logEntry in _logParser.LogEntries)
            {
                _logEntries.Add(logEntry);
            }
            dataGrid.Items.Refresh();
        }
    }
}
