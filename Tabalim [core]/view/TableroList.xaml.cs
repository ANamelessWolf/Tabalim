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
    /// Interaction logic for TableroList.xaml
    /// </summary>
    public partial class TableroList : UserControl
    {
        public event RoutedEventHandler TableroChanged;
        public TableroList()
        {
            InitializeComponent();
        }
        public void UpdateList()
        {
            
            tableros.ItemsSource = TabalimApp.CurrentProject?.Tableros.Values.Select(x => new TableroItem(x));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateList();
        }

        private void tabItem_Clicked(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((sender as Button).FindName("id") as TextBlock).Text);
            TabalimApp.CurrentTablero = TabalimApp.CurrentProject.Tableros[id];
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = LoadTablero,
                TaskCompleted = TableroLoaded
            };
            tr.Run(null);
        }

        private void TableroLoaded(object result)
        {
            TableroChanged?.Invoke(this, new RoutedEventArgs());
        }

        private object LoadTablero(SQLite_Connector conn, object input)
        {
            if(TabalimApp.CurrentTablero.Componentes.Count == 0)
                TabalimApp.CurrentTablero.LoadComponentesAndCircuits(conn);
            return null;
        }

        private void copyBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
