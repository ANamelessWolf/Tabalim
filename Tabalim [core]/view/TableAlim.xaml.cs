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

        }

        public void SetItemSource(IEnumerable<AlimentadorRow> rows)
        {
            this.listOfLines.ItemsSource = rows;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.listOfLines.ItemsSource);
            IsRefreshed?.Invoke(this, new RoutedEventArgs());
        }

        private void btnEditLine_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteLine_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
