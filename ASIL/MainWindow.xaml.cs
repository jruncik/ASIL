using System.Collections.Generic;
using System.IO;
using System.Windows;

using Microsoft.Win32;

using ASIL.Core;

namespace ASIL
{
    public partial class MainWindow : Window
    {
        private IList<LogEntryBase> _logEntries = new List<LogEntryBase>(0);

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

            LogParser logParser = new LogParser();
            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                logParser.ParseStream(sr);
            }
            _logEntries = logParser.LogEntries;
        }
    }
}
