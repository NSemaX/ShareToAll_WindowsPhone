﻿#pragma checksum "C:\Users\semak\Desktop\ShareToAll\ShareToAll\About.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1201A09494C5B000DB4EFBE357114850"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace ShareToAll {
    
    
    public partial class About : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock version_lb;
        
        internal System.Windows.Controls.TextBlock desc;
        
        internal System.Windows.Controls.TextBlock mail_info;
        
        internal System.Windows.Controls.TextBlock website_info;
        
        internal System.Windows.Controls.TextBlock rate_review;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/ShareToAll;component/About.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.version_lb = ((System.Windows.Controls.TextBlock)(this.FindName("version_lb")));
            this.desc = ((System.Windows.Controls.TextBlock)(this.FindName("desc")));
            this.mail_info = ((System.Windows.Controls.TextBlock)(this.FindName("mail_info")));
            this.website_info = ((System.Windows.Controls.TextBlock)(this.FindName("website_info")));
            this.rate_review = ((System.Windows.Controls.TextBlock)(this.FindName("rate_review")));
        }
    }
}
