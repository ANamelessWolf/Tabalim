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
using Tabalim.Core.runtime;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Interaction logic for AlimentadorPicker.xaml
    /// </summary>
    public partial class AlimentadorPicker : MetroWindow
    {
        public Linea SelectedLinea;
        public AlimentadorPicker()
        {
            InitializeComponent();
        }
        public AlimentadorPicker(Linea existantLinea) :
            this()
        {
            this.alimentadorPicker.ExistantLinea = existantLinea;
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (alimentadorPicker.IsValid())
                {
                    SelectedLinea = alimentadorPicker.GetLinea();
                    //if (SelectedLinea.No == "")
                    //    SelectedLinea.GetNumber();
                    System.Diagnostics.Debug.WriteLine("1ER DIALOGO");
                    foreach (Linea l in TabalimApp.CurrentProject.Lineas.Values)
                        System.Diagnostics.Debug.WriteLine(l);
                    var dialog = new ConductorPicker(SelectedLinea);
                    //this.Hide();
                    dialog.ShowDialog();
                    
                    if (dialog.DialogResult.Value)
                    {
                        System.Diagnostics.Debug.WriteLine("2o");
                        foreach (Linea l in TabalimApp.CurrentProject.Lineas.Values)
                            System.Diagnostics.Debug.WriteLine(l);
                        SelectedLinea = dialog.Linea;
                        this.DialogResult = true;
                        this.Close();
                    }
                }
            } catch(Exception exc)
            {
                await this.ShowMessageAsync("Alimentador Incompleto", "Debe seleccionar todos los campos del alimentador para poder crearlo.\n" + exc.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
