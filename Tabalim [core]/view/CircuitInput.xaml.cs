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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tabalim.Core.controller;
using Tabalim.Core.model;
using Tabalim.Core.runtime;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para CircuitInput.xaml
    /// </summary>
    public partial class CircuitInput : MetroWindow
    {
        const string CORRIENTE = "Corriente corregida: {0:0.00} A";
        const string CAIDA = "Caida de tensión: {0:0.00%}";
        /// <summary>
        /// El circuito seleccionado
        /// </summary>
        public Circuito SelectedCircuit;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitInput"/>.
        /// </summary>
        public CircuitInput(Circuito c)
        {
            this.SelectedCircuit = c;
            InitializeComponent();
        }
        /// <summary>
        /// Maneja el evento que realizá la acción de dar clic en el boton de OK
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private async void btnClick(object sender, RoutedEventArgs e)
        {
            double l, f;
            string cboItem = ((ComboBoxItem)this.cboFactAgrup.SelectedItem).Content.ToString();
            if (double.TryParse(this.tboLong.Text, out l) && this.cboFactAgrup.SelectedIndex != -1 &&
                Double.TryParse(cboItem, out f) && this.cboCalibre.SelectedIndex != -1)
                if (this.SelectedCircuit.CaidaVoltaje <= 2)
                    this.SelectedCircuit.UpdateCircuitTr(async (Object result) =>
                    {
                        if (result is bool && (Boolean)result)
                        {
                            this.SelectedCircuit.Longitud = l;
                            this.SelectedCircuit.FactorAgrupacion = f;
                            this.SelectedCircuit.Calibre = this.cboCalibre.SelectedItem as Calibre;
                            this.DialogResult = true;
                            this.Close();
                        }
                        else
                            await this.ShowMessageAsync("Error al actualizar", result.ToString());
                    }, l, f, calibre: (this.cboCalibre.SelectedItem as Calibre).AWG);
                else
                    await this.ShowMessageAsync("Datos inválidos", "La caida de tensión debe ser menor a 2%");
            else
                await this.ShowMessageAsync("Información incompleta", "Seleccione una longitud y factor de agrupamiento válido");
        }
        /// <summary>
        /// Maneja el evento que realizá la acción de dar clic en el boton de Cancel
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        /// <summary>
        /// Validates the input pasted string
        /// </summary>
        void OnInput_Changed(object sender, TextCompositionEventArgs e)
        {
            Double num;
            if (!Double.TryParse(e.Text, out num))
                e.Handled = true;
        }
        /// <summary>
        /// Validates the input pasted string
        /// </summary>
        void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                Double num;
                TextBox txt = sender as TextBox;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!Double.TryParse(txt.Text, out num))
                    e.CancelCommand();
            }
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (this.SelectedCircuit != null)
            {
                this.cboCalibre.ItemsSource = Calibre.GetTableroCalibres(this.SelectedCircuit.CorrienteContinua);
                this.cboCalibre.SelectedItem = this.SelectedCircuit.Calibre;
                this.txtCto.Text = String.Format("Cto. [{0}]", this.SelectedCircuit.ToString());
                this.tboLong.Text = this.SelectedCircuit.LongitudAsString;
                var options = this.cboFactAgrup.Items.OfType<ComboBoxItem>().Select(x => x.Content.ToString());
                this.cboFactAgrup.SelectedIndex = options.Contains(this.SelectedCircuit.FactorAgrupacion.ToString()) ? options.ToList().IndexOf(this.SelectedCircuit.FactorAgrupacion.ToString()) : 4;
                //string cboInput = ((ComboBoxItem)this.cboFactAgrup.Items[0]).Content.ToString();
                //if (double.Parse(cboInput) == this.SelectedCircuit.FactorAgrupacion)
                //    this.cboFactAgrup.SelectedIndex = 0;
                //else
                //    this.cboFactAgrup.SelectedIndex = 1;
                this.cboCalibre.SelectedItem = this.SelectedCircuit.Calibre;
                CalculateCaida();
            }
        }

        private void CalculateCaida()
        {
            if (cboFactAgrup.SelectedIndex != -1 && cboCalibre.SelectedIndex != -1)
            {
                double l, f;
                string cboItem = ((ComboBoxItem)this.cboFactAgrup.SelectedItem).Content.ToString();
                this.SelectedCircuit.Longitud = Double.TryParse(tboLong.Text, out l) ? l : 0;
                this.SelectedCircuit.FactorAgrupacion = Double.TryParse(cboItem, out f) ? f : 0;
                this.SelectedCircuit.Calibre = cboCalibre.SelectedItem as Calibre;
                this.corrienteTxt.Text = String.Format(CORRIENTE, this.SelectedCircuit.CorrienteCorregida);
                this.caidaTxt.Text = String.Format(CAIDA, this.SelectedCircuit.CaidaVoltaje / 100);
            }
        }

        private void TboLong_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateCaida();
        }

        private void CboFactAgrup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateCaida();
        }

        private void CboCalibre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateCaida();
        }
    }
}
