using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Tabalim.Core.model;
using Tabalim.Core.model.raw;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Interaction logic for TableroPreview.xaml
    /// </summary>
    public partial class TableroPreview : UserControl
    {
        public Tablero Tablero { get { return Tabalim.Core.runtime.TabalimApp.CurrentTablero; } }
        ObservableCollection<CtoRow> DataCollection { get { return dataCollection; } set { dataCollection = value; } }
        ObservableCollection<CtoRow> dataCollection;
        int originalColumns;
        public TableroPreview()
        {
            InitializeComponent();
            originalColumns = (circuitos.View as GridView).Columns.Count;
        }

        public void UpdateData()
        {
            if (Tablero != null)
            {
                this.dataCollection = new ObservableCollection<CtoRow>(Tablero.Circuitos.Select(x => new CtoRow(x.Value)));
                Debug.WriteLine(JsonConvert.SerializeObject(new TableroRaw(Tablero), Newtonsoft.Json.Formatting.Indented));
                var components = Tablero.Componentes.Values.GroupBy(x => x.Key).Select(y => y.First());
                int i = 0;
                while((circuitos.View as GridView).Columns.Count > originalColumns)
                    (circuitos.View as GridView).Columns.RemoveAt(1);
                //if((circuitos.View as GridView).Columns.Count > 1)
                //    while(((circuitos.View as GridView).Columns.ElementAt(1).DisplayMemberBinding as Binding).Path.Path.CompareTo("Potencia") != 0)
                //        (circuitos.View as GridView).Columns.RemoveAt(1);
                foreach (Componente c in components) {
                    //if ((circuitos.View as GridView).Columns.Count(x => x.GetValue(UserControl.NameProperty).ToString().CompareTo(c.XamlKey) == 0) == 0)
                    //{
                        var v = new GridViewColumn() { HeaderTemplate = CreateDataTemplate(c), DisplayMemberBinding = new Binding("[" + c.Key + "]") };
                        v.SetValue(UserControl.NameProperty, c.XamlKey);
                        (circuitos.View as GridView).Columns.Insert(++i, v);
                    //}
                }

                circuitos.ItemsSource = DataCollection;
                ((CollectionView)CollectionViewSource.GetDefaultView(circuitos.ItemsSource)).Refresh();
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }
        private DataTemplate CreateDataTemplate(Componente c)
        {
            String imageName = File.Exists("img/componentes/" + c.ImageIndex + "_32x32.png") ? c.ImageIndex.ToString() : "no_img";
            StringReader data = new StringReader(
                @"<DataTemplate 
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Grid>
                        <Grid.RowDefinitions>
                            <RowDefinition Height=""32"" />
                            <RowDefinition />
                        </Grid.RowDefinitions>
                        <Image Width=""32"" Height=""32"" Source=""/elekid;component/img/componentes/" + imageName + @"_32x32.png""/>
                        <TextBlock Grid.Row=""1"" Text=""" + (c is Motor ? runtime.TabalimApp.Motores.FirstOrDefault(x => x.HP == c.Potencia.HP )?.HPFormat : c.Potencia.ToString()) +  @""" />
                    </Grid>
                </DataTemplate>"
            );
            return XamlReader.Load(XmlReader.Create(data)) as DataTemplate;
        }
    }
}
