﻿#pragma checksum "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9415266EA15F68DE0AF36638A28DB7B9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace MEDICS2014.controls.treamentsConrols {
    
    
    /// <summary>
    /// treatmentsBreathing
    /// </summary>
    public partial class treatmentsBreathing : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button o2Button;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button needleDButton;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button chestTubeButoon;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button chestSealButton;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button doneButton;
        
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
            System.Uri resourceLocater = new System.Uri("/MEDICS2014;component/controls/treamentsconrols/treatmentsbreathing.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
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
            this.o2Button = ((System.Windows.Controls.Button)(target));
            
            #line 9 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
            this.o2Button.Click += new System.Windows.RoutedEventHandler(this.breathingButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.needleDButton = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
            this.needleDButton.Click += new System.Windows.RoutedEventHandler(this.breathingButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.chestTubeButoon = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
            this.chestTubeButoon.Click += new System.Windows.RoutedEventHandler(this.breathingButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.chestSealButton = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
            this.chestSealButton.Click += new System.Windows.RoutedEventHandler(this.breathingButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.doneButton = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\..\controls\treamentsConrols\treatmentsBreathing.xaml"
            this.doneButton.Click += new System.Windows.RoutedEventHandler(this.doneButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

