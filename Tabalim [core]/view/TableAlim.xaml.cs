using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static Tabalim.Core.controller.TransactionUtils;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para TableAlim.xaml
    /// </summary>
    public partial class TableAlim : UserControl
    {
        public event RoutedEventHandler IsRefreshed;
        public TableAlim()
        {
            InitializeComponent();//Hola
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetItemSource(runtime.TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
        }

        public void SetItemSource(IEnumerable<AlimentadorRow> rows)
        {
            foreach (AlimentadorRow r in rows)
                System.Diagnostics.Debug.WriteLine(r);
            this.listOfLines.ItemsSource = null;
            this.listOfLines.ItemsSource = new ObservableCollection<AlimentadorRow>(rows.OrderBy(x => x.Id));
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.listOfLines.ItemsSource);
            //view.Refresh();
            //IsRefreshed?.Invoke(this, new RoutedEventArgs());
        }

        private void btnEditLine_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse((((sender as Button).Parent as Viewbox).Parent as StackPanel).Children.OfType<TextBlock>().ToArray()[1].Text);
            var dialog = new AlimentadorPicker(TabalimApp.CurrentProject.Lineas[i]);
            dialog.ShowDialog();
            if (dialog.DialogResult.Value == true)
            {
                Linea linea = dialog.SelectedLinea;
                var updateData = new KeyValuePair<String, Object>[] {
                    new KeyValuePair<string, object>("label", linea.No),
                    new KeyValuePair<string, object>("dest_from", linea.From),
                    new KeyValuePair<string, object>("dest_end",linea.Type.Id),
                    new KeyValuePair<string, object>("fact_demanda", linea.Destination.FactorDemanda),
                    new KeyValuePair<string, object>("fact_temperatura", linea.Temperatura),
                    new KeyValuePair<string, object>("fac_agrupamiento", linea.FactorAgrupamiento),
                    new KeyValuePair<string, object>("fac_potencia", linea.FactorPotencia),
                    new KeyValuePair<string, object>("longitud", linea.Longitud),
                    new KeyValuePair<string, object>("is_cobre", linea.IsCobre),
                    new KeyValuePair<string, object>("dest_name", linea.To),
                    new KeyValuePair<string, object>("dest_desc", linea.ToDesc),
                    new KeyValuePair<string, object>("conductor", linea.Conductor.NoTubos),
                    new KeyValuePair<string, object>("is_charola", linea.Conductor.IsCharola),
                    new KeyValuePair<string, object>("extra", linea.Conductor.Extra)
                };
                SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                {
                    TransactionTask = (SQLite_Connector conn, Object input) =>
                     {
                         Object[] data = input as Object[];
                         int alimId = (int)data[0];
                         AlimInput inAlim = (AlimInput)data[1];
                         var uData = (KeyValuePair<String, Object>[])data[2];
                         //inAlim.Id = alimId;
                         if (inAlim.Update(conn, uData))
                             return inAlim;
                         else
                             return null;
                     },
                    TaskCompleted = (Object result) =>
                    {
                        if (result is AlimInput)
                        {
                            TabalimApp.CurrentProject.Lineas[i].Update(result as AlimInput);
                            SetItemSource(runtime.TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
                        }
                    }
                };
                tr.Run(new Object[] { i, linea.ToAlimInput(TabalimApp.CurrentProject), updateData });
            }

        }

        private void btnDeleteLine_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse((((sender as Button).Parent as Viewbox).Parent as StackPanel).Children.OfType<TextBlock>().ToArray()[1].Text);
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Object[] data = input as Object[];
                    int alimId = (int)data[0];
                    AlimInput inAlim = (AlimInput)data[1];
                    return inAlim.Delete(conn);
                },
                TaskCompleted = (Object result) =>
                {
                    if ((Boolean)result)
                    {
                        //TabalimApp.CurrentProject.Lineas.Values.ToList().ForEach(x => x.GetNumber());
                        SetItemSource(runtime.TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
                    }
                }
            };
            tr.Run(new Object[] { i, TabalimApp.CurrentProject.Lineas[i].ToAlimInput(TabalimApp.CurrentProject) });
        }

        public void Clear()
        {
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Object[] data = input as Object[];
                    List<AlimInput> rows = (List<AlimInput>)data[0];
                    Boolean flag = true;
                    foreach (AlimInput alimInput in rows)
                        flag &= alimInput.Delete(conn);
                    return flag;
                },
                TaskCompleted = (Object result) =>
                {
                    if ((Boolean)result)
                    {
                        SetItemSource(TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
                    }
                }
            };
            tr.Run(new Object[] { TabalimApp.CurrentProject.Lineas.Select(x => x.Value.ToAlimInput(TabalimApp.CurrentProject)).ToList() });
        }

        public async void Copy()
        {
            var dialog = new WinSaveProject();
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                var raw = new ProjectRaw(TabalimApp.CurrentProject, dialog.Description) { Tablero = dialog.AlimTitle };
                Clipboard.SetText(JsonConvert.SerializeObject(raw));
                await UiUtils.ShowMessageDialog(this, "", "Alimentador guardado en portapapeles.");
            }
        }

        private void btnCloneLine_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse((((sender as Button).Parent as Viewbox).Parent as StackPanel).Children.OfType<TextBlock>().ToArray()[1].Text);
            Linea linea = TabalimApp.CurrentProject.Lineas[i].Clone();
            linea.ToAlimInput(TabalimApp.CurrentProject).CreateAlimentadorTr(linea.Destination,
                (Object result, int alimId) =>
                {
                    //if (result is bool && (bool)result)
                    {
                        linea.Id = alimId;
                        TabalimApp.CurrentProject.Lineas.Add(alimId, linea);
                        SetItemSource(TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
                    }

                });
        }

        private void btnExportLine_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse((((sender as Button).Parent as Viewbox).Parent as StackPanel).Children.OfType<TextBlock>().ToArray()[1].Text);
            Linea linea = TabalimApp.CurrentProject.Lineas[i].Clone();
            Clipboard.SetText(String.Join(Environment.NewLine, linea.Conductor.NoTubos, linea.Conductor.Calibre.Replace("KCM", ""), linea.Longitud));
        }
    }
}
