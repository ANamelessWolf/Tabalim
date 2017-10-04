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
    /// Lógica de interacción para TableroPicker.xaml
    /// </summary>
    public partial class TableroPicker : Window
    {
        /// <summary>
        /// Crea un nuevo tablero con la información del sistema actual
        /// </summary>
        /// <returns>El nuevo tablero</returns>
        public Tablero CreateTablero()
        {
            return new Tablero()
            {
                Circuitos = new Dictionary<string, Circuito>(),
                Componentes = new Dictionary<string, Componente>(),
                NombreTablero = "Tablero 1",
                ProjectId = 1,
                Sistema = tablero.SelectedSystem,
                TableroId = 1,
            };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TableroPicker"/> class.
        /// </summary>
        public TableroPicker()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            tablero.SelectedSystem.Polo = (int)tablero.cboPolos.SelectedItem;
            tablero.SelectedSystem.TpAlimentacion = tablero.AlimType;
            tablero.SelectedSystem.Temperatura = tablero.slidTemperature.Value;
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
