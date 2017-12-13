using MahApps.Metro.Controls;
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
using Tabalim.Core.model;
using Tabalim.Core.runtime;
using Tabalim.Core.view;

namespace Tabalim.App
{
    /// <summary>
    /// Interaction logic for AlimWindow.xaml
    /// </summary>
    public partial class AlimWindow : MetroWindow
    {
        public AlimWindow()
        {
            InitializeComponent();
        }

        private void CreateLinea_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AlimentadorPicker();
            dialog.ShowDialog();
            if (dialog.DialogResult.Value)
            {
                TabalimApp.CurrentProject.Lineas.Add(int.Parse(dialog.SelectedLinea.No.Substring(1)), dialog.SelectedLinea);
                alimTable.SetItemSource(TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Abrir_Click(object sender, RoutedEventArgs e)
        {

        }




        //         AlimInput alim;
        //Destination dest;
        //App.Tabalim.CreateAlimentadorTr(alim, dest,
        //    (Object result) => 
        //        {
        //            IEnumerable<AlimInput> rows;
        //alimTable.SetItemSource(rows);
        //        });
    }
}
