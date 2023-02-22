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
using Tabalim.Core.controller;
using Tabalim.Core.model;
using Tabalim.Core.runtime;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Interaction logic for CloneCircuit.xaml
    /// </summary>
    public partial class CloneCircuit : MetroWindow
    {
        const String CIRCUITO = "Circuito: [{0}]";
        const String POLOS = "No. de polos: {0}";
        Circuito Circuito;
        public Componente[] Componentes;
        public CloneCircuit()
        {
            InitializeComponent();
        }
        public CloneCircuit(Circuito circuito) 
        {
            InitializeComponent();
            this.Circuito = circuito;
            this.Componentes = Circuito.Componentes.Values.Select(x => x.CloneGeneral()).ToArray();
        }
        public CloneCircuit(Componente componente) 
        {
            InitializeComponent();
            this.Circuito = componente.Circuito;
            this.Componentes = new Componente[] { componente };
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            circuitName.Text =String.Format(CIRCUITO, Circuito.ToString());
            numPolos.Text = String.Format(POLOS, Circuito.Polos.Length);
            var circuitos = UiUtils.GetAvailableCircuitos(TabalimApp.CurrentTablero, Circuito.Polos.Length, Circuito.HasMotor);
            circuitos = circuitos.Except(new Circuito[] { Circuito });
            listOfCircuits.ItemsSource = circuitos.Select(x => new CtoItem(x));
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if(listOfCircuits.SelectedIndex != -1)
            {
                foreach (Componente c in Componentes)
                {
                    c.Circuito = (listOfCircuits.SelectedItem as CtoItem).Circuito;
                    c.Circuito.Longitud = Circuito.Longitud;
                    if (c.Circuito.Componentes.Values.Count(x => x.Key == c.Key) > 0)
                    {
                        await this.ShowMessageAsync("Componente repetido.", "El circuito objetivo ya contiene un componente con las mismas caracteristicas.");
                        return;
                    }
                }
                this.DialogResult = true;
                this.Close();
            }
            else
            {               
                await this.ShowMessageAsync("Ningún circuito seleccionado.", "Debe seleccionar un circuito para continuar.");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
