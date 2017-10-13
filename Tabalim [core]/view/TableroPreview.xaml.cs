using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public TableroPreview()
        {
            InitializeComponent();
        }

        public void UpdateData()
        {
            if (Tablero != null)
            {
                this.dataCollection = new ObservableCollection<CtoRow>(Tablero.Circuitos.Select(x => new CtoRow(x.Value)));
                var components = Tablero.Componentes.Values.GroupBy(x => x.Key).Select(y => y.First());
                int i = 0;
                
                foreach (Componente c in components) {
                    var v =new GridViewColumn() { HeaderTemplate = CreateDataTemplate(c), DisplayMemberBinding = new Binding("Componentes[" + c.Key + "]") };
                    v.SetValue(UserControl.NameProperty, c.XamlKey);
                    if ((circuitos.View as GridView).Columns.Count(x => x.GetValue(UserControl.NameProperty).ToString().CompareTo(c.XamlKey) == 0) == 0 )
                        (circuitos.View as GridView).Columns.Insert(++i, v);
                }

                circuitos.ItemsSource = DataCollection;
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }
        private DataTemplate CreateDataTemplate(Componente c)
        {
            StringReader data = new StringReader(
                @"<DataTemplate 
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Grid>
                        <Grid.RowDefinitions>
                            <RowDefinition Height=""32"" />
                            <RowDefinition />
                        </Grid.RowDefinitions>
                        <Image Width=""32"" Height=""32"" Source=""/elekid;component/img/componentes/no_img_32x32.png""/>
                        <TextBlock Grid.Row=""1"" Text=""" + (c is Motor ? runtime.TabalimApp.Motores.FirstOrDefault(x => x.HP == c.Potencia.HP )?.HPFormat : c.Potencia.ToString()) +  @""" />
                    </Grid>
                </DataTemplate>"
            );
            return XamlReader.Load(XmlReader.Create(data)) as DataTemplate;
        }
    }
}
