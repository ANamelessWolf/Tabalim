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
using Tabalim.Addin.Model;

namespace Tabalim.Addin.View
{
    /// <summary>
    /// Interaction logic for WinBlockInsert.xaml
    /// </summary>
    public partial class WinBlockInsert : MetroWindow
    {
        public ComponentGalleryItem SelectedBlock { get { return this.gallery.SelectedItem; } }

        public WinBlockInsert()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
