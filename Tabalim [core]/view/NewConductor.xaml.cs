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
using Tabalim.Core.model;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Interaction logic for NewConductor.xaml
    /// </summary>
    public partial class NewConductor : UserControl
    {
        public Linea SelectedLinea;
        public NewConductor()
        {
            InitializeComponent();
        }

        public bool IsValid()
        {
            double i;
            if (conductorCbo.SelectedIndex == -1)
                throw new Exception("Debe elegir unos de los conductores disponibles.");
            else if (!double.TryParse(longitudTbo.Text.Trim(), out i))
                throw new Exception("Debe ingresar una longitud valida.");
            else return true;
        }

        public Linea GetLinea()
        {
            SelectedLinea.Conductor = conductorCbo.SelectedItem as Conductor;
            SelectedLinea.Longitud = double.Parse(longitudTbo.Text.Trim());
            return SelectedLinea;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            conductorCbo.ItemsSource = Conductor.GetConductorOptions(SelectedLinea.Destination.Fases, SelectedLinea.CorrienteCorregida);
        }
    }
}
