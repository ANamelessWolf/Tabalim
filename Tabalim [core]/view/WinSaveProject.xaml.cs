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
using System.Xml.Linq;
using Tabalim.Core.runtime;
using IoPath = System.IO.Path;
namespace Tabalim.Core.view
{
    /// <summary>
    /// Interaction logic for WinSaveProject.xaml
    /// </summary>
    public partial class WinSaveProject : MetroWindow
    {
        XDocument Doc;
        public String Description
        {
            get { return this.txtDexc.Text; }
            set
            {
                this.txtDexc.Text = value;
                this.Doc.Descendants("alim_desc").FirstOrDefault().Value = value;
            }
        }
        public String AlimTitle
        {
            get { return this.txtName.Text; }
            set
            {
                this.txtName.Text = value;
                this.Doc.Descendants("alim_name").FirstOrDefault().Value = value;
            }
        }
        public WinSaveProject()
        {
            this.Doc = XDocument.Load(IoPath.Combine(IoPath.GetDirectoryName(TabalimApp.AppDBPath), "config_alim.xml"), LoadOptions.None);
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Doc.Save(IoPath.Combine(IoPath.GetDirectoryName(TabalimApp.AppDBPath), "config_alim.xml"));
            this.Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Description = this.Doc.Descendants("alim_desc").FirstOrDefault().Value;
            this.AlimTitle = this.Doc.Descendants("alim_name").FirstOrDefault().Value;
        }
    }
}
