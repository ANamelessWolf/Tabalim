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
using System.Windows.Shapes;
using Tabalim.Core.model;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Interaction logic for ConductorPicker.xaml
    /// </summary>
    public partial class ConductorPicker : MetroWindow
    {
        Linea Linea;
        public ConductorPicker()
        {
            InitializeComponent();
        }
        public ConductorPicker(Linea linea) :
            this()
        {
            this.Linea = linea;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.conductorPicker.SelectedLinea = Linea;
        }
    }
}
