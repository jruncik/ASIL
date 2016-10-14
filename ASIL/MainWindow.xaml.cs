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

namespace ASIL
{
    public partial class MainWindow : Window
    {
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

            dataGrid.ItemsSource = null;

            try
            {
                LogParser logParser = new LogParser();
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {
                    logParser.ParseStream(sr);
                }

                ObservableCollection<Record> data = GenerateEnginesViewData(logParser);
                GenerateColumnas(data);
                dataGrid.ItemsSource = data;
            }
            catch (System.Exception)
            {
                dataGrid.ItemsSource = null;
            }

            dataGrid.Items.Refresh();
        }

        private ObservableCollection<Record> GenerateEnginesViewData(LogParser logParser)
        {
            ObservableCollection<Record> data = new ObservableCollection<Record>();

            foreach (LogEntry logEntry in logParser.LogEntries)
            {
                data.Add(new Record(new Property("Log Time", logEntry.LogTime.ToString()), new Property("Message", logEntry.Message.ToString())));
            }

            return data;
        }

        private void GenerateColumnas(ObservableCollection<Record> data)
        {
            var columns = data.First().Properties.Select((x, i) => new { Name = x.Name, Index = i }).ToArray();
            foreach (var item in columns)
            {
                var binding = new Binding(string.Format("Properties[{0}].Value", item.Index));
                dataGrid.Columns.Add(new DataGridTextColumn() { Header = item.Name, Binding = binding });
            }
        }
    }
}
