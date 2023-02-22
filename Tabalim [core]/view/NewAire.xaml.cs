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
    /// Interaction logic for NewAire.xaml
    /// </summary>
    public partial class NewAire : UserControl
    {
        public NewAire()
        {
            InitializeComponent();
        }
        public BigAire GetAire()
        {
            double d;
            return new BigAire()
            {
                Fases = (int)fasesTbo.SelectedItem,
                Hilos = (int)fasesTbo.SelectedItem,
                Tension = (Tension)tensionTbo.SelectedItem,
                FLA = Double.TryParse(corrienteTbo.Text, out d) ? d : 0
            };
        }
        public bool IsValid()
        {
            double d;
            if (fasesTbo.SelectedIndex == -1)
                throw new Exception("Falta definir las fases del aire.");
            else if (tensionTbo.SelectedIndex == -1)
                throw new Exception("Falta definir la tensión.");
            else if (!Double.TryParse(corrienteTbo.Text, out d) || d <= 0)
                throw new Exception("La corriente debe ser un valor numerico mayor a 0.");
            else
                return true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fasesTbo.ItemsSource = new int[] { 2, 3 };
            tensionTbo.ItemsSource = new int[] { 600, 480, 440, 380, 220, 208 }.Select(x => new Tension(x, 2));
        }
    }
}
