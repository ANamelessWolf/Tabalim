using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using Tabalim.Core.controller;

namespace Tabalim.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            string db = @"C:\Users\Miguel\Source\Repos\Tabalim\Tabalim [App]\bin\Debug\chinook.db";
            SQLite_Connector.Run(db, null,
                (Object input, SQLite_Connector conn) => 
                {
                    conn.SelectTables();
                    return null;
                });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Tabalim.Core.view.ComponentGalleryPicker().ShowDialog();
        }
    }
}
