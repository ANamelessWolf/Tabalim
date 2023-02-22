using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
        public Linea Linea;

        public ConductorPicker()
        {
            InitializeComponent();
        }
        public ConductorPicker(Linea linea) :
            this()
        {
            this.Linea = linea;
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (conductorPicker.IsValid())
                {
                    this.DialogResult = true;
                    Linea = conductorPicker.GetLinea();
                    this.Close();
                }
            }
            catch(Exception exc)
            {
                await this.ShowMessageAsync("Conductor incompleto.", "Existen datos inválidos. " + exc.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.conductorPicker.SelectedLinea = Linea;
        }
    }
}
