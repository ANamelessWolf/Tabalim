﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

namespace Tabalim.Core.view
{
    /// <summary>
    /// Interaction logic for CargaPicker.xaml
    /// </summary>
    public partial class CargaPicker : MetroWindow
    {
        public TableroMock SelectedTablero;
        public CargaPicker()
        {
            InitializeComponent();
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TableroMock t = newCarga.GetTablero();
                if (t != null)
                {
                    this.DialogResult = true;
                    this.SelectedTablero = t;
                    this.Close();
                }
            }
            catch (Exception exc)
            {
                await this.ShowMessageAsync("Tablero Incompleto", "Debe seleccionar todos los campos de tablero para poder crearlo.\n" + exc.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
