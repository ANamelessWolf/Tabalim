﻿#pragma checksum "..\..\..\view\PlantillaNuevoTablero.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "797546C8383848054ECDA9735100CCEA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Tabalim.Core.view;


namespace Tabalim.Core.view {
    
    
    /// <summary>
    /// PlantillaNuevoTablero
    /// </summary>
    public partial class PlantillaNuevoTablero : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 41 "..\..\..\view\PlantillaNuevoTablero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboSistemas;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\view\PlantillaNuevoTablero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton optInterruptor;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\view\PlantillaNuevoTablero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton optZapata;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\view\PlantillaNuevoTablero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboPolos;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\view\PlantillaNuevoTablero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slidTemperature;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/elekid;component/view/plantillanuevotablero.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\view\PlantillaNuevoTablero.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\view\PlantillaNuevoTablero.xaml"
            ((Tabalim.Core.view.PlantillaNuevoTablero)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cboSistemas = ((System.Windows.Controls.ComboBox)(target));
            
            #line 42 "..\..\..\view\PlantillaNuevoTablero.xaml"
            this.cboSistemas.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cboSistemas_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.optInterruptor = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.optZapata = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.cboPolos = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.slidTemperature = ((System.Windows.Controls.Slider)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

