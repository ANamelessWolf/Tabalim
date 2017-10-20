using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
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
using Tabalim.Core.model.raw;
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
            tableros.SelectedIndex = tableros.Items.Count - 1;
        }

        private void tabItem_Clicked(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((sender as Button).FindName("id") as TextBlock).Text);
            
        }

        private void TableroLoaded(object result)
        {
            TableroChanged?.Invoke(this, new RoutedEventArgs());
        }

        private object LoadTablero(SQLite_Connector conn, object input)
        {
            Tablero t = input as Tablero;
            if(t.Componentes.Count == 0)
                t.LoadComponentesAndCircuits(conn);
            return t;
        }

        private void copyBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse((sender as Button).Tag.ToString());
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = LoadTablero,
                TaskCompleted = TableroReady
            };
            tr.Run(TabalimApp.CurrentProject.Tableros[id]);
        }

        private void TableroReady(object result)
        {
            Clipboard.SetText(JsonConvert.SerializeObject(new TableroRaw(result as Tablero)));
            ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync("", "Tablero guardado en portapapeles.");
        }

        private void tableros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabalimApp.CurrentTablero = TabalimApp.CurrentProject.Tableros[(tableros.SelectedItem as TableroItem).Id];
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = LoadTablero,
                TaskCompleted = TableroLoaded
            };
            tr.Run(TabalimApp.CurrentTablero);
        }

        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse((sender as Button).Tag.ToString());
            if(await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync("", "¿Desea eliminar el tablero?", MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
            {
                TabalimApp.CurrentProject.Tableros[id].DeleteTableroTr((object result) =>
                {
                    if ((bool)result) TabalimApp.CurrentProject.Tableros.Remove(id);
                    this.UpdateList();
                });               
            }

        }
    }
}
