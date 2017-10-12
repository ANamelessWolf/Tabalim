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
                Double.TryParse(cboItem, out f))
            {
                KeyValuePair<string, object>[] updateData = new KeyValuePair<string, object>[]
                {
                      new KeyValuePair<string, object>("longitud", l),
                      new KeyValuePair<string, object>("fac_agrup", f)
                };
                SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                {
                    TransactionTask = (SQLite_Connector conn, Object input) =>
                    {
                        try
                        {
                            Object[] data = input as Object[];
                            var cto = data[0] as Circuito;
                            var uData = data[1] as KeyValuePair<string, object>[];
                            return cto.Update(conn, uData);
                        }
                        catch (Exception exc)
                        {
                            return exc.Message;
                        }
                    },
                    TaskCompleted = async (Object result) =>
                    {
                        if (result is bool && (Boolean)result)
                        {
                            this.SelectedCircuit.Longitud = l;
                            this.SelectedCircuit.FactorAgrupacion = f;
                            this.DialogResult = true;
                            this.Close();
                        }
                        else
                            await this.ShowMessageAsync("Error al actualizar", result.ToString());
                    }
                };
                tr.Run(new Object[]{ this.SelectedCircuit, updateData});
            }
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
            this.txtCto.Text = String.Format("Cto. [{0}]", this.SelectedCircuit.ToString());
            this.tboLong.Text = this.SelectedCircuit.LongitudAsString;
            string cboInput = ((ComboBoxItem)this.cboFactAgrup.Items[0]).Content.ToString();
            if (double.Parse(cboInput) == this.SelectedCircuit.FactorAgrupacion)
                this.cboFactAgrup.SelectedIndex = 0;
            else
                this.cboFactAgrup.SelectedIndex = 1;
        }
    }
}
