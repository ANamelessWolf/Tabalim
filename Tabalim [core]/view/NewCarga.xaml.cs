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
    /// Interaction logic for NewCarga.xaml
    /// </summary>
    public partial class NewCarga : UserControl
    {
        public NewCarga()
        {
            InitializeComponent();
        }

        //private void tablerosTbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (tablerosTbo.SelectedIndex != -1)
        //    {
        //        Tablero tablero = (tablerosTbo.SelectedItem as TableroItem).Tablero;
        //        SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
        //        {
        //            TransactionTask = LoadTablero,
        //            TaskCompleted = TableroReady
        //        };
        //        tr.Run(tablero);
        //    }
        //}

        private void TableroReady(object result)
        {
            Tablero t = result as Tablero;
            //tempTbo.Text = t.Sistema.Temperatura.ToString();
            var componentes = t.Componentes;
            pAlumbTbo.Text = componentes.Values.Where(x => x is Alumbrado).Sum(x => x.Potencia.PotenciaAparente * x.Count).ToString();
            pContTbo.Text = componentes.Values.Where(x => x is Contacto).Sum(x => x.Potencia.PotenciaAparente * x.Count).ToString();
            pMotorTbo.Text = componentes.Values.Where(x => x is Motor).Sum(x => x.Potencia.PotenciaAparente * x.Count).ToString();
        }

        private object LoadTablero(SQLite_Connector conn, object input)
        {
            Tablero t = input as Tablero;
            var componentes = t.Componentes;
            if (componentes.Count == 0)
                t.LoadComponentesAndCircuits(conn);
            return t;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //tablerosTbo.ItemsSource = runtime.TabalimApp.CurrentProject.Tableros.Values.Select(x => new TableroItem(x)).ToList();
            fasesCbo.ItemsSource = new int[] { 1, 2, 3 };
        }
        public TableroMock GetTablero()
        {
            //if (tablerosTbo.SelectedIndex != -1)
            //    return (tablerosTbo.SelectedItem as TableroItem).Tablero;
            if (TableroIsValid())
            {
                return new TableroMock()
                {
                    Name = nameTbo.Text,
                    Fases = (int)fasesCbo.SelectedItem,
                    Tension = (Tension)tensionCbo.SelectedItem,
                    PotenciaAlumbrado = pAlumbTbo.Text == String.Empty ? 0 : Double.Parse(pAlumbTbo.Text),
                    PotenciaContactos = pContTbo.Text == String.Empty ? 0 : Double.Parse(pContTbo.Text),
                    PotenciaMotores = pMotorTbo.Text == String.Empty ? 0 : Double.Parse(pMotorTbo.Text),
                    //Temperatura = int.Parse(tempTbo.Text)
                };
            }
            return null;
        }

        private bool TableroIsValid()
        {
            double d;
            int i;
            if (nameTbo.Text.Trim() == String.Empty)
                throw new Exception("El nombre no de estar vacío.");
            else if (fasesCbo.SelectedIndex == -1)
                throw new Exception("Debe elegir numero de fases.");
            else if (tensionCbo.SelectedIndex == -1)
                throw new Exception("Debe seleccionar una tension para el tablero.");
            else if (pAlumbTbo.Text.Trim() != String.Empty && (!Double.TryParse(pAlumbTbo.Text.Trim(), out d) || d < 0))
                throw new Exception("La potencia de alumbrados debe ser numérica y con valor positivo.");
            else if (pContTbo.Text.Trim() != String.Empty && (!Double.TryParse(pContTbo.Text.Trim(), out d) || d < 0))
                throw new Exception("La potencia de contactos debe ser numérica y con valor positivo.");
            else if (pMotorTbo.Text.Trim() != String.Empty && (!Double.TryParse(pMotorTbo.Text.Trim(), out d) || d < 0))
                throw new Exception("La potencia de motores debe ser numérica y con valor positivo.");
            //else if (!int.TryParse(pAlumbTbo.Text.Trim(), out i) || d <= 0)
            //    throw new Exception("La temperatura debe ser numérica y mayor a cero.");
            else return true;
        }

        private void fasesCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fasesCbo.SelectedIndex != -1)
                tensionCbo.ItemsSource = Enum.GetValues(typeof(TensionVal)).Cast<TensionVal>().Select(x => new Tension(x, (int)fasesCbo.SelectedItem));
        }
    }
}
