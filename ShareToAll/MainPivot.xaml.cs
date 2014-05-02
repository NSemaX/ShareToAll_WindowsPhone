using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook;
using System.Windows.Media.Imaging;
using System.Device.Location;
using System.Windows.Controls.Primitives;

namespace ShareToAll
{
    public partial class MainPivot : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher;
        public GeoCoordinate konumum = null;
        int locset_tel = 0;

        string ffid = " ";
        string fftoken = " ";

        string tokenx = " ";
        string tokensecx = " ";

        public Popup buyNowScreen1;
        public Popup buyNowScreen;

        public MainPivot()
        {
            InitializeComponent();
            pivot1.SelectionChanged += pivot1_SelectionChanged;

            locset_tel = IsolatedSettingsHelper.GetValue<int>("locsetting_tel");

            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High)
                {
                    MovementThreshold = 10
                };
                watcher.Start();
                watcher.StatusChanged += watcher_StatusChanged;
            }

            if ( locset_tel == 1)//app konum ayarı ve tel konum ayarı kapalı ise konumu bulma.
            {
                if (watcher != null)
                {
                    watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                }
            }

            this.Loaded += MainPivot_Loaded;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string latest_updatex = IsolatedSettingsHelper.GetValue<string>("latest_update");
            string latest_update_channelx = IsolatedSettingsHelper.GetValue<string>("latest_update_channel");
            string latest_update_timex = IsolatedSettingsHelper.GetValue<string>("latest_update_time");

            Helper helper = new Helper();
            if (latest_updatex != null)
            {
                if (latest_updatex.Count() > 0)
                    latest_update.Text = latest_updatex + "\n" + latest_update_channelx.Remove(0, 1);
            }
            if (latest_update_timex == "" || latest_update_timex == null)
            {
                //do nothing.
            }
            else
            {
                DateTime date = Convert.ToDateTime(latest_update_timex);
                latest_update_time.Text = helper.CalculateShareTime(date) + " ago.";
            }


  

        }

        void pivot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot1.SelectedIndex == 0)
                (ApplicationBar as ApplicationBar).IsVisible = false;
            else
                (ApplicationBar as ApplicationBar).IsVisible = true;
        }
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.NoData:
                    {
                        if (locset_tel != 2)
                        {
                            MessageBox.Show("We cant find your location.");
                            IsolatedSettingsHelper.SetValue<int>("locsetting_tel", 2);
                        }

                        break;
                    }

                case GeoPositionStatus.Disabled:
                    {
                        if (locset_tel != 2)
                        {
                            MessageBox.Show("Your location settings which is on phone settings is closed, please activate it and try again.");
                            IsolatedSettingsHelper.SetValue<int>("locsetting_tel", 2);
                        }
                        break;

                    }
                case GeoPositionStatus.Ready:
                    {
                        if (locset_tel != 1)
                        {
                            IsolatedSettingsHelper.SetValue<int>("locsetting_tel", 1);
                        }
                        break;

                    }
            }

        }



        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Dispatcher.BeginInvoke(() => Degisti(e));

        }


        void Degisti(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            konumum = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            IsolatedSettingsHelper.SetValue<string>("latitudex", konumum.Latitude.ToString());
            IsolatedSettingsHelper.SetValue<string>("longitudex", konumum.Longitude.ToString());

        }

        void MainPivot_Loaded(object sender, RoutedEventArgs e)
        {
            progbar.IsIndeterminate = true;
            progbar.Visibility = Visibility.Visible;

           ffid = IsolatedSettingsHelper.GetValue<string>("fid_sharetoall");
           fftoken = IsolatedSettingsHelper.GetValue<string>("ftoken_sharetoall");

           tokenx = IsolatedSettingsHelper.GetValue<string>("ttoken");
           tokensecx = IsolatedSettingsHelper.GetValue<string>("ttokensec");

           locset_tel = IsolatedSettingsHelper.GetValue<int>("locsetting_tel");

           if (ffid == " " && fftoken == " " || ffid == null && fftoken == null)
           {
               BitmapImage bm = new BitmapImage(new Uri("images/user_icon.png", UriKind.RelativeOrAbsolute));
               user_image.Source = bm;
               facebook_tg.IsChecked = false;
           }
           else
           {
               try
               {
                   LoadUserInfo();
               }
               catch
               {
                   BitmapImage bm = new BitmapImage(new Uri("images/user_icon.png", UriKind.RelativeOrAbsolute));
                   user_image.Source = bm;
               }
               facebook_tg.IsChecked = true;
           }


           if (tokenx == " " && tokensecx == " " || tokenx == null && tokensecx == null)
           {
               twitter_tg.IsChecked= false;
           }
           else
               twitter_tg.IsChecked = true;



           string latest_updatex = IsolatedSettingsHelper.GetValue<string>("latest_update");
           string latest_update_channelx = IsolatedSettingsHelper.GetValue<string>("latest_update_channel");
           string latest_update_timex = IsolatedSettingsHelper.GetValue<string>("latest_update_time");

           Helper helper = new Helper();
           if (latest_updatex != null)
           {
               if (latest_updatex.Count() > 0)
                   latest_update.Text = latest_updatex + "\n" + latest_update_channelx.Remove(0, 1);
           }
           if (latest_update_timex == "" || latest_update_timex == null)
           {
               //do nothing.
           }
           else
           {
               DateTime date = Convert.ToDateTime(latest_update_timex);
               latest_update_time.Text = helper.CalculateShareTime(date) + " ago.";
           }

           // get application tile
           ShellTile tile = ShellTile.ActiveTiles.First();
           if (null != tile)
           {
               // create a new data for tile
               StandardTileData data = new StandardTileData();

               // to make tile flip add data to background also

               data.BackContent = latest_updatex;
               if (latest_update_timex == "" || latest_update_timex == null)
               {
                   data.BackTitle = "";
               }
               else
               {
                   DateTime date = Convert.ToDateTime(latest_update_timex);
                   data.BackTitle = helper.CalculateShareTime(date) + " ago.";
               }
               
         
               // data.BackBackgroundImage = new Uri("/Images/Green.jpg", UriKind.Relative);

               // update tile
               tile.Update(data);
           }

           progbar.IsIndeterminate = false;
           progbar.Visibility = Visibility.Collapsed;

          // NotifyAlertControl();

           
        }

        private void facebook_tg_Checked(object sender, RoutedEventArgs e)
        {
            if (ffid == " " && fftoken == " " || ffid == null && fftoken == null)
            {
                NavigationService.Navigate(new Uri(string.Format("/FacebookLoginPage.xaml?vpost={0}", "Hi,i'm using ShareToAll: | http://sharetoall1.yolasite.com/"), UriKind.Relative));
                //MessageBox.Show("To login your facebook account please hold a post and select share on facebook menu ");
            }
        }

        private void facebook_tg_Unchecked(object sender, RoutedEventArgs e)
        {

            if (ffid != " " && fftoken != " " || ffid != null && fftoken != null)
            {
                string _loginUrl = IsolatedSettingsHelper.GetValue<string>("loginUri");

                string _accessToken = IsolatedSettingsHelper.GetValue<string>("ftoken_sharetoall");

                webrowser2.Navigate(new Uri(String.Format("https://www.facebook.com/logout.php?next={0}&access_token={1}", _loginUrl, _accessToken)));


                this.ClearCookies(FacebookCookieUri);

                IsolatedSettingsHelper.SetValue<string>("ftoken_sharetoall", null);
                IsolatedSettingsHelper.SetValue<string>("fid_sharetoall", null);

                MessageBox.Show("You have log outted from your facebook account.", "Log out", MessageBoxButton.OK);


            }
        }

        private CookieContainer _cookieContainer = new CookieContainer();
        private static readonly Uri FacebookCookieUri = new Uri("https://login.facebook.com/login.php");
        public void ClearCookies(Uri uri)
        {

            var cookies = this._cookieContainer.GetCookies(uri);

            foreach (Cookie cookie in cookies)
            {

                cookie.Discard = true;

                cookie.Expired = true;

            }

        }

        private void twitter_tg_Checked(object sender, RoutedEventArgs e)
        {
            if (tokenx == " " && tokensecx == " " || tokenx == null && tokensecx == null)
            {
                NavigationService.Navigate(new Uri(string.Format("/TwitterLoginPage.xaml?vpost={0}", "Hi,i'm using ShareToAll: | http://sharetoall1.yolasite.com/"), UriKind.Relative));
            }

        }

        private void twitter_tg_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tokenx == " " && tokensecx == " " || tokenx == null && tokensecx == null)
            {
                IsolatedSettingsHelper.SetValue<string>("ttoken", null);
                IsolatedSettingsHelper.SetValue<string>("ttokensec", null);

                MessageBox.Show("You have log outted from your twitter account.", "Log out", MessageBoxButton.OK);
            }
        }

        private void postanupdate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PostAnUpdate.xaml", UriKind.Relative));
        }

        private void postaimage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PostAPhoto.xaml", UriKind.Relative));
        }

        private void checkin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CheckIn.xaml", UriKind.Relative));
        }

        private void LoadUserInfo()
        {


            var fb = new FacebookClient(fftoken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    var profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}&width=150&height=150", ffid, "square", fftoken);

                    this.user_image.Source = new BitmapImage(new Uri(profilePictureUrl));
                    //this.MyName.Text = String.Format("{0} {1}", (string)result["first_name"], (string)result["last_name"]);
                });
            };

            fb.GetAsync("me");
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)//About Page
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            if (buyNowScreen != null)
                buyNowScreen.IsOpen = false;

            if (buyNowScreen1 != null)
                buyNowScreen1.IsOpen = false;

            //IsolatedSettingsHelper.SetValue<bool>("sharedonfacebook", false);
            //IsolatedSettingsHelper.SetValue<bool>("sharedontwitter", false);

        }
        public void NotifyAlertControl()
        {
            if (IsolatedSettingsHelper.GetValue<bool>("sharedonfacebook"))
            {
                buyNowScreen1 = new Popup();
                buyNowScreen1.Child =
                    new NotifyAlert
                        ("Successfully shared on Facebook");
                buyNowScreen1.IsOpen = true;
                buyNowScreen1.VerticalOffset = 0;
                buyNowScreen1.HorizontalOffset = 0;
            }

            if (IsolatedSettingsHelper.GetValue<bool>("sharedontwitter"))
            {
                Dispatcher.BeginInvoke(() =>
                {
                    buyNowScreen = new Popup();
                    buyNowScreen.Child =
                        new NotifyAlert
                            ("Successfully shared on Twitter");
                    buyNowScreen.IsOpen = true;
                    buyNowScreen.VerticalOffset = 0;
                    buyNowScreen.HorizontalOffset = 0;
                });
            }
        }
        ///////////////////////////////////////////////////////////////////
    }
}