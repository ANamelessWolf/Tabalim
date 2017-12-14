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
    /// Interaction logic for NewAlimentador.xaml
    /// </summary>
    public partial class NewAlimentador : UserControl
    {
        private List<BigMotor> Motors { get { return motors; } }
        private List<BigMotor> motors;
        public List<TableroItem> Tableros { get { return tableros; } }
        List<TableroItem> tableros;
        public List<Tablero> listOfTableros { get; set; }
        DestinationType SelectedType;
        internal Linea ExistantLinea;

        public NewAlimentador()
        {
            InitializeComponent();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(motorList.SelectedIndex != -1)
            {
                motors.RemoveAt(motorList.SelectedIndex);
                ((CollectionView)CollectionViewSource.GetDefaultView(this.motorList.ItemsSource)).Refresh();
                ProcessType();
            }
        }

        private void addMotorBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MotorPicker();
            if(dialog.ShowDialog() == true)
            {
                motors.Add(dialog.SelectedMotor);
                motorList.ItemsSource = Motors;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.motorList.ItemsSource);
                view.Refresh();
                ProcessType();
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.motors = new List<BigMotor>();
            this.tableros = new List<TableroItem>();
            this.toTypeCbo.ItemsSource = DestinationType.Types;
            this.listOfTableros = runtime.TabalimApp.CurrentProject.Tableros.Values.ToList();
            this.fasesCbo.ItemsSource = new int[] { 2, 3 };
            this.tensionCbo.ItemsSource = Enum.GetValues(typeof(TensionVal)).Cast<TensionVal>().Select(x => new Tension(x, new SistemaBifasico()));
            if (ExistantLinea != null)
            {
                this.fromTbo.Text = ExistantLinea.From;
                this.toTypeCbo.SelectedItem = ExistantLinea.Type;
                this.toNameTbo.Text = ExistantLinea.To;
                this.toDescTbo.Text = ExistantLinea.ToDesc;
                //Destination
                this.motors = ExistantLinea.Destination.Motors.ToList();
                motorList.ItemsSource = this.motors;
                this.tableros = ExistantLinea.Destination.Cargas.Select(x => new TableroItem(x)).ToList();
                tablerosList.ItemsSource = this.tableros;
                if (ExistantLinea.Destination.ExtraData != null)
                {
                    this.kvarTbo.Text = ExistantLinea.Destination.ExtraData.KVar.ToString();
                    this.fasesCbo.SelectedItem = ExistantLinea.Destination.ExtraData.Fases;
                    this.tensionCbo.SelectedItem = ExistantLinea.Destination.ExtraData.Tension;
                }
                //Conductor
                this.isCopper.IsChecked = ExistantLinea.IsCobre;
                //Factors
                this.slidDemanda.Value = ExistantLinea.Destination.FactorDemanda;
                this.slidGroup.Value = ExistantLinea.FactorAgrupamiento;
                this.slidPower.Value = ExistantLinea.FactorPotencia;
                this.slidTemp.Value = ExistantLinea.Temperatura;
                //UI
                this.toTypeCbo.IsEnabled = false;
                addMotorBtn.Visibility = Visibility.Collapsed;
                addCargaBtn.Visibility = Visibility.Collapsed;
                deleteBtn.Visibility = Visibility.Collapsed;
                deleteCargaBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void addCargaBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CargaPicker();
            if (dialog.ShowDialog() == true)
            {
                tableros.Add(new TableroItem(dialog.SelectedTablero));
                tablerosList.ItemsSource = Tableros;
                ((CollectionView)CollectionViewSource.GetDefaultView(this.tablerosList.ItemsSource)).Refresh();
                ProcessType();
            }
        }

        private void deleteCargaBtn_Click(object sender, RoutedEventArgs e)
        {
            if(tablerosList.SelectedIndex != -1)
            {
                tableros.RemoveAt(tablerosList.SelectedIndex);
                ((CollectionView)CollectionViewSource.GetDefaultView(this.tablerosList.ItemsSource)).Refresh();
                ProcessType();
            }
        }

        private void toTypeCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(toTypeCbo.SelectedIndex != -1)
            {
                SelectedType = toTypeCbo.SelectedItem as DestinationType;
                ProcessType();
            }
        }
        public bool IsValid()
        {
            if (this.fromTbo.Text.Trim() == "")
                throw new Exception("Falta definir el orígen.");
            else if (this.toTypeCbo.SelectedIndex == -1)
                throw new Exception("Falta definir el tipo de destino.");
            else if (this.toNameTbo.Text.Trim() == "")
                throw new Exception("Falta definir el nombre del destino.");
            else if (this.toDescTbo.Text.Trim() == "")
                throw new Exception("Falta definir la descripción del destino.");
            else if (DestinationIsValid())
                return true;
            else
                return false;
        }

        private bool DestinationIsValid()
        {
            double d;
            if (SelectedType != null)
            {
                if (SelectedType.OnlyOneMotor == true && motors.Count != 1)
                    throw new Exception("Este tipo de destino sólo permite un motor.");
                else if (SelectedType.OnlyOneMotor == false && motors.Count == 0)
                    throw new Exception("Este tipo de destino requiere al menos un motor.");
                else if (SelectedType.OnlyOneCarga == true && tableros.Count != 1)
                    throw new Exception("Este tipo de destino sólo permite un tablero.");
                else if (SelectedType.OnlyOneCarga == false && tableros.Count == 0)
                    throw new Exception("Este tipo de destino requiere al menos un motor.");
                else if (SelectedType.UseExtraData)
                {
                    if (kvarTbo.Text.Trim() == String.Empty || !double.TryParse(kvarTbo.Text.Trim(), out d))
                        throw new Exception("Falta el campo KVAR o se intridujo un valor inválido.");
                    else if (fasesCbo.SelectedIndex == -1)
                        throw new Exception("Falta seleccionar las fases.");
                    else if (tensionCbo.SelectedIndex == -1)
                        throw new Exception("Falta selleccionar la tensión.");
                    else return true;
                }
                else return true;
            }
            else return false;
        }

        public Linea GetLinea()
        {
            Linea linea = new Linea();
            linea.From = fromTbo.Text.Trim();
            linea.Type = SelectedType;
            linea.To = toNameTbo.Text.Trim();
            linea.ToDesc = toDescTbo.Text.Trim();
            ExtraData extraData = null;
            if (SelectedType.UseExtraData)
            {
                extraData = new ExtraData();
                extraData.Fases = (int)fasesCbo.SelectedItem;
                extraData.Tension = tensionCbo.SelectedItem as Tension;
                extraData.KVar = double.Parse(kvarTbo.Text.Trim());
            }
            linea.Destination = new Destination(SelectedType, slidDemanda.Value, motors, tableros.Select(x => x.Tablero), extraData);
            linea.IsCobre = isCopper.IsChecked == true;
            linea.FactorAgrupamiento = slidGroup.Value;
            linea.FactorPotencia = slidPower.Value;
            linea.FactorTemperartura = Temperatura.GetFactor((int)slidTemp.Value);
            linea.Temperatura = (int)slidTemp.Value;
            if(ExistantLinea != null)
            {
                linea.Id = ExistantLinea.Id;
                linea.Conductor = ExistantLinea.Conductor;
                linea.Longitud = ExistantLinea.Longitud;
            }
            return linea;
        }
        private void ProcessType()
        {
            if(SelectedType != null)
            {
                tabMotor.Visibility = SelectedType.OnlyOneMotor != null ? Visibility.Visible : Visibility.Collapsed;
                tabCargas.Visibility = SelectedType.OnlyOneCarga != null ? Visibility.Visible : Visibility.Collapsed;
                tabDatos.Visibility = SelectedType.UseExtraData ? Visibility.Visible : Visibility.Collapsed;
                tabs.SelectedItem = tabs.Items.OfType<TabItem>().First(x => x.Visibility == Visibility.Visible);
                addMotorBtn.Visibility = SelectedType.OnlyOneMotor == true && motors.Count == 1 ? Visibility.Collapsed : Visibility.Visible;
                addCargaBtn.Visibility = SelectedType.OnlyOneCarga == true && tableros.Count == 1 ? Visibility.Collapsed : Visibility.Visible;
                deleteBtn.Visibility = motors.Count == 1 ? Visibility.Visible : Visibility.Collapsed;
                deleteCargaBtn.Visibility = tableros.Count == 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
