﻿#pragma checksum "..\..\AdvSettings.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "EFFBE36F30EDC88816F215435ED4DDD0405C677B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
using location;


namespace location {
    
    
    /// <summary>
    /// AdvSettings
    /// </summary>
    public partial class AdvSettings : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Username;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Location;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NonAdvSettings;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label inputLabel;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox Debug;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Port;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IP;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Timeout;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OptionalHeaders;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SubmitButton;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\AdvSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CheckLocButton;
        
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
            System.Uri resourceLocater = new System.Uri("/location;component/advsettings.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AdvSettings.xaml"
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
            this.Username = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.Location = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.NonAdvSettings = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\AdvSettings.xaml"
            this.NonAdvSettings.Click += new System.Windows.RoutedEventHandler(this.NonAdvSetting_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.inputLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            
            #line 21 "..\..\AdvSettings.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.Checked_h9);
            
            #line default
            #line hidden
            
            #line 21 "..\..\AdvSettings.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Unchecked_h9);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 22 "..\..\AdvSettings.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.Checked_h0);
            
            #line default
            #line hidden
            
            #line 22 "..\..\AdvSettings.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Unchecked_h0);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 23 "..\..\AdvSettings.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.Checked_h1);
            
            #line default
            #line hidden
            
            #line 23 "..\..\AdvSettings.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Unchecked_h1);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Debug = ((System.Windows.Controls.CheckBox)(target));
            
            #line 26 "..\..\AdvSettings.xaml"
            this.Debug.Checked += new System.Windows.RoutedEventHandler(this.Checked_dM);
            
            #line default
            #line hidden
            
            #line 26 "..\..\AdvSettings.xaml"
            this.Debug.Unchecked += new System.Windows.RoutedEventHandler(this.Unchecked_dM);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Port = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.IP = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.Timeout = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.OptionalHeaders = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            this.SubmitButton = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\AdvSettings.xaml"
            this.SubmitButton.Click += new System.Windows.RoutedEventHandler(this.SubmitButton_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.CheckLocButton = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\AdvSettings.xaml"
            this.CheckLocButton.Click += new System.Windows.RoutedEventHandler(this.CheckLocButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
