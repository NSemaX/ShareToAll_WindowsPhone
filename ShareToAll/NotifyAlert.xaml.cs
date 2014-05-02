using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ShareToAll
{
    public partial class NotifyAlert : UserControl
    {
        public NotifyAlert(string message)
        {
            InitializeComponent();
            Storyboard2.Begin();

             alert_text.Text = message;

        }

        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //var frame = App.Current.RootVisual as PhoneApplicationFrame;
            //frame.Navigate(new Uri("/NotificaitonsPage.xaml", UriKind.Relative));
        }
    }
}
