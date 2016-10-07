using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ASIL.Core;
using Microsoft.Win32;

namespace ASIL
{

    struct LogTimeData
    {
        int year;
        int month;
        int day;
        int gour;
        int minute;
        int seconds;
        int millisecond;
    }

    class LogTimeParser
    {
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LogParser _logParser = new LogParser();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
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
        }
    }
}
