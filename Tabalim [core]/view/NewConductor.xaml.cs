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
    /// Interaction logic for NewConductor.xaml
    /// </summary>
    public partial class NewConductor : UserControl
    {
        public Linea SelectedLinea;
        double Caida;
        public NewConductor()
        {
            InitializeComponent();
        }

        public bool IsValid(bool partial = true)
        {
            double i;
            if (calibreCbo.SelectedIndex == -1)
                throw new Exception("Debe elegir unos de los conductores disponibles.");
            else if (numberCbo.SelectedIndex == -1)
                throw new Exception("Debe elegir una opcion válida.");
            else if (!double.TryParse(longitudTbo.Text.Trim(), out i))
                throw new Exception("Debe ingresar una longitud valida.");
            else if (partial && Caida > slidCaida.Value)
                throw new Exception("La caida de voltaje debe ser menor a " + slidCaida.Value + "%");
            else return true;
        }

        public Linea GetLinea()
        {
            SelectedLinea.Conductor = Conductor.GetConductor(calibreCbo.SelectedItem as string, SelectedLinea.CorrienteCorregida, SelectedLinea.Destination.Hilos, (int)numberCbo.SelectedItem, SelectedLinea.IsCobre);
            SelectedLinea.SelectedConductor = calibreCbo.SelectedIndex;
            SelectedLinea.Longitud = double.Parse(longitudTbo.Text.Trim());
            return SelectedLinea;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //conductorCbo.ItemsSource = Conductor.GetConductorOptions(SelectedLinea.Destination.Fases, SelectedLinea.CorrienteCorregida, SelectedLinea.IsCobre, SelectedLinea.Destination.Hilos);
            //conductorCbo.SelectedIndex = SelectedLinea.SelectedConductor;
            var list = Conductor.GetAvailableCalibres(SelectedLinea.CorrienteCorregida, SelectedLinea.IsCobre);
            calibreCbo.ItemsSource = list;
            calibreCbo.SelectedItem = SelectedLinea.Conductor == null ? list.First() : SelectedLinea.Conductor.Calibre;
            longitudTbo.Text = SelectedLinea.Longitud.ToString();
        }

        private void calibreCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calibreCbo.SelectedIndex != -1)
            {
                numberCbo.ItemsSource = Conductor.GetAllowedPipes(SelectedLinea.CorrienteCorregida, calibreCbo.SelectedItem as string, SelectedLinea.IsCobre);
                numberCbo.SelectedIndex = 0;
                //CalculateCaida();
            }
        }
        private async void CalculateCaida()
        {
            try
            {
                if (IsValid(false))
                {
                    GetLinea();
                    Caida = SelectedLinea.CaidaVoltaje;
                    caidaLbl.Text = Caida.ToString("0.00") + "%";
                }
                else caidaLbl.Text = String.Empty;
            }
            catch(Exception exc)
            {
                caidaLbl.Text = String.Empty;
                await this.ShowMessageDialog("Alimentador Incompleto", "Debe seleccionar todos los campos del alimentador para poder crearlo.\n" + exc.Message);
            }
        }

        private void numberCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if(numberCbo.SelectedIndex != -1)
            //    CalculateCaida();
        }

        private void longitudTbo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(longitudTbo.Text != String.Empty)
                CalculateCaida();
        }
    }
}
