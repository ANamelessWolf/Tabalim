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
    /// Interaction logic for NewMotor.xaml
    /// </summary>
    public partial class NewMotor : UserControl
    {
        public NewMotor()
        {
            InitializeComponent();
        }

        public BigMotor GetMotor()
        {
            return new BigMotor()
            {
                Fases = (int)fasesTbo.SelectedItem,
                Tension = tensionTbo.SelectedItem as Tension,
                Potencia = powerSelector.SelectedPower,
                Hilos = (int)hilosCbo.SelectedItem
            };
        }

        public bool IsValid()
        {
            if (fasesTbo.SelectedIndex == -1)
                throw new Exception("Falta definir las fases del motor.");
            else if (tensionTbo.SelectedIndex == -1)
                throw new Exception("Falta definir la tensión.");
            else if (hilosCbo.SelectedIndex == -1)
                throw new Exception("Falta definir el numero de hilos.");
            else if (powerSelector.SelectedPower == null)
                throw new Exception("Falta definir la potencia.");
            else
                return true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fasesTbo.ItemsSource = new int[] { 2, 3 };
            SistemaBifasico sistema = new SistemaBifasico();
            tensionTbo.ItemsSource = Enum.GetValues(typeof(TensionVal)).Cast<TensionVal>().Select(x => new Tension(x, sistema.Fases));
            powerSelector.Power = PowerType.HP;
            hilosCbo.ItemsSource = new int[] { 2, 3, 4 };
        }

        private void fasesTbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fasesTbo.SelectedIndex != -1)
                powerSelector.Fases = (int)fasesTbo.SelectedItem;
        }
    }
}
