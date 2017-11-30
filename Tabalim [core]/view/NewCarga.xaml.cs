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

        private void tablerosTbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tablerosTbo.SelectedIndex != -1)
            {
                Tablero tablero = (tablerosTbo.SelectedItem as TableroItem).Tablero;
                SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                {
                    TransactionTask = LoadTablero,
                    TaskCompleted = TableroReady
                };
                tr.Run(tablero);
            }
        }

        private void TableroReady(object result)
        {
            Tablero t = result as Tablero;
            tempTbo.Text = t.Sistema.Temperatura.ToString();
            pAlumbTbo.Text = t.Componentes.Values.Where(x => x is Alumbrado).Sum(x => x.Potencia.PotenciaAparente * x.Count).ToString();
            pContTbo.Text = t.Componentes.Values.Where(x => x is Contacto).Sum(x => x.Potencia.PotenciaAparente * x.Count).ToString();
            pMotorTbo.Text = t.Componentes.Values.Where(x => x is Motor).Sum(x => x.Potencia.PotenciaAparente * x.Count).ToString();
        }

        private object LoadTablero(SQLite_Connector conn, object input)
        {
            Tablero t = input as Tablero;
            if (t.Componentes.Count == 0)
                t.LoadComponentesAndCircuits(conn);
            return t;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tablerosTbo.ItemsSource = runtime.TabalimApp.CurrentProject.Tableros.Values.Select(x => new TableroItem(x)).ToList();
        }
        public Tablero GetTablero()
        {
            if (tablerosTbo.SelectedIndex != -1)
                return (tablerosTbo.SelectedItem as TableroItem).Tablero;
            return null;
        }
    }
}
