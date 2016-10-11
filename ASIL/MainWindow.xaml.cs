using System.IO;
using System.Windows;
using ASIL.Core;
using Microsoft.Win32;

namespace ASIL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

            LogParser logParser = new LogParser();
            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                logParser.ParseStream(sr);
            }
        }
    }
}
