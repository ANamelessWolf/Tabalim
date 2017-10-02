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
using Tabalim.Core.model;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para PlantillaNuevoTablero.xaml
    /// </summary>
    public partial class PlantillaNuevoTablero : UserControl
    {
        /// <summary>
        /// Devuelve el sistema seleccionado
        /// </summary>
        /// <value>
        /// El tipo de sistema seleccionado
        /// </value>
        public SistemaFases SelectedSystem
        {
            get
            {
                var sys = this.cboSistemas.SelectedIndex != -1 ? 
                    (SistemaFases)this.cboSistemas.SelectedItem : null;
                return sys;
            }
        }
        /// <summary>
        /// Devuelve el tipo de alimentación seleccionado, ya
        /// sea interruptor o Zapata
        /// </summary>
        /// <value>
        /// El tipo de alimentador
        /// </value>
        public TipoAlimentacion AlimType
        {
            get
            {
                if (optInterruptor.IsChecked.Value)
                    return TipoAlimentacion.Interruptor;
                else if (optZapata.IsChecked.Value)
                    return TipoAlimentacion.Zapata;
                else
                    return TipoAlimentacion.Ninguno;
            }
        }
        /// <summary>
        /// Inicializa una instancia de la clase <see cref="PlantillaNuevoTablero"/>.
        /// </summary>
        public PlantillaNuevoTablero()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.cboSistemas.ItemsSource = SistemaFaseUtils.GetApplicationSystemPhases();
            this.cboSistemas.SelectFirst();
        }
        /// <summary>
        /// Maneja el evento que realizá la carga de los polos segun la fase seleccionada
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="SelectionChangedEventArgs"/> que contienen la información del evento.</param>
        private void cboSistemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboSistemas.SelectedIndex != -1)
                this.cboPolos.ItemsSource = this.SelectedSystem.Polos;
            this.cboPolos.SelectFirst();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedSystem.Polo =(int) this.cboPolos.SelectedItem;
            this.SelectedSystem.TpAlimentacion = this.AlimType;
            this.SelectedSystem.Temperatura = this.slidTemperature.Value;
        }
    }
}
