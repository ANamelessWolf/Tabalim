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
using Tabalim.Core.runtime;
using Tabalim.Core.view;

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
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Tabalim.Core.view.ComponentGalleryPicker().ShowDialog();
        }

        private void btnPickSystem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TableroPicker();
            dialog.ShowDialog();
            if (dialog.DialogResult.Value)
            {
                TabalimApp.CurrentTablero = dialog.CreateTablero();
                App.Tabalim.Tableros.Add(TabalimApp.CurrentTablero);
            }
        }

        private void btnCreateComponent_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ComponentPicker();
            dialog.ShowDialog();
            if (dialog.DialogResult.Value)
            {
                
            }
        }
    }
}
