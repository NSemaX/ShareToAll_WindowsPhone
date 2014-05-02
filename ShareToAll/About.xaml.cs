using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace ShareToAll
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }
        private void mail_info_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "From Windows Phone ShareToAll App";
            // emailComposeTask.Body = sharedid;
            emailComposeTask.To = "sema@semakudu.com";
            //emailComposeTask.Cc = "cc@example.com";
            //emailComposeTask.Bcc = "bcc@example.com";
            emailComposeTask.Show();
        }

        private void website_info_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var task = new Microsoft.Phone.Tasks.WebBrowserTask
            {
                URL = "http://sharetoall1.yolasite.com/"
            };

            task.Show();
        }

        private void rate_review_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
    }
}