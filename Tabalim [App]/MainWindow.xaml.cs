﻿using MahApps.Metro.Controls;
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
using Tabalim.Core.view;

namespace Tabalim.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Tabalim.Core.view.ComponentGalleryPicker().ShowDialog();
        }

        private void btnPickSystem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TableroPicker();
            dialog.ShowDialog();
            if (dialog.DialogResult.Value)
                App.Tabalim.CreateTableroTr(dialog.CreateTablero());
        }

        private void btnCreateComponent_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ComponentPicker();
            dialog.ShowDialog();
            if (dialog.DialogResult.Value)
            {
                TabalimApp.CurrentTablero.AddComponentTr(dialog.SelectedComponent,
                    (Object result) =>
                    {
                        tablero.UpdateData();
                    });

            }
        }

        private void btnExportComponent_Click(object sender, RoutedEventArgs e)
        {
            this.ExporCurrentTablero();
        }
        private void btnImportComponent_Click(object sender, RoutedEventArgs e)
        {
            this.ImportTablero(TabalimApp.CurrentProject);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var win = new WinTableroSettings(TabalimApp.CurrentTablero);
            win.ShowDialog();
            if (win.DialogResult.Value)
            {

            }
        }
    }
}
