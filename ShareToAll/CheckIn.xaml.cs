using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using ShareToAll.Models;
using System.IO;
using Facebook;
using Hammock.Authentication.OAuth;
using TweetSharp;
using System.Windows.Controls.Primitives;

namespace ShareToAll
{
    public partial class CheckIn : PhoneApplicationPage
    {
        public List<MainVen.Venue> VenueList;
        string latitudex = "";
        string longitudex = "";
        string ffid = " ";
        string fftoken = " ";

        string tokenx = " ";
        string tokensecx = " ";

        string twitter_post = "";

        public Popup buyNowScreen1;
        public Popup buyNowScreen;

        public CheckIn()
        {
            InitializeComponent();
            latitudex = IsolatedSettingsHelper.GetValue<string>("latitudex");
            longitudex = IsolatedSettingsHelper.GetValue<string>("longitudex");

            ffid = IsolatedSettingsHelper.GetValue<string>("fid_sharetoall");
            fftoken = IsolatedSettingsHelper.GetValue<string>("ftoken_sharetoall");

            tokenx = IsolatedSettingsHelper.GetValue<string>("ttoken");
            tokensecx = IsolatedSettingsHelper.GetValue<string>("ttokensec");

            progbar.IsIndeterminate = true;
            progbar.Visibility = Visibility.Visible;

            GetVenues();
            this.Loaded += CheckIn_Loaded;
        }

        void CheckIn_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GetVenues()
        {
            string ProviderURI;

            string lat1;
            string lat2;
            string long1;
            string long2;


            string lat = latitudex;
            string longx = longitudex;
            if (lat.Contains(',') && longx.Contains(','))
            {
                string[] words = lat.Split(',');
                lat1 = words[0];
                lat2 = words[1];

                string[] words1 = longx.Split(',');
                long1 = words1[0];
                long2 = words1[1];

                ProviderURI = "https://api.foursquare.com/v2/venues/search?ll=" + lat1 + "." + lat2 + "," + long1 + "." + long2 + "&llAcc=1000&alt=0&altAcc=10000&limit=20&client_secret=HWLR3RXDCFTDQBJ51NS5UN5QTZ1BU3SXR12HYNGWPXSI01EZ&client_id=LQXBHR4ZVWFCIQT2RB1YJOMZGOJ0VYBM5FM5AS1LEI42GRLL&v=20130606";
            }
            else
            {
                ProviderURI = "https://api.foursquare.com/v2/venues/search?ll=" + latitudex + "," + longitudex + "&llAcc=1000&alt=0&altAcc=10000&limit=40&client_secret=HWLR3RXDCFTDQBJ51NS5UN5QTZ1BU3SXR12HYNGWPXSI01EZ&client_id=LQXBHR4ZVWFCIQT2RB1YJOMZGOJ0VYBM5FM5AS1LEI42GRLL&v=20130606";
            }
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient3_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(ProviderURI));
        }

        void webClient3_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error != null)
            {
                MessageBox.Show("Sorry, can not access the data.Please try again later.");
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            MainVen.RootObject rootObject = (MainVen.RootObject)serializer.Deserialize(new JsonTextReader(new StringReader(e.Result)), typeof(MainVen.RootObject));

            if (rootObject == null)
            {
                MessageBox.Show("Sorry, can not access the data.Please try again later.");
                return;
            }

            if (rootObject != null)
            {
                VenueList = rootObject.response.venues;
                this.post_location.ItemsSource = VenueList;

                progbar.IsIndeterminate = false;
                progbar.Visibility = Visibility.Collapsed;
            }



        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (place_text.Text.Count() > 0)
            {
                IsolatedSettingsHelper.SetValue<string>("latest_update_channel", " ");
                if (ffid == " " && fftoken == " " || ffid == null && fftoken == null)
                {
                    //do nothing
                }
                else
                {
                    try
                    {
                        progbar.IsIndeterminate = true;
                        progbar.Visibility = Visibility.Visible;
                        ShareOnFacebook();
                    }
                    catch
                    {
                    }
                }

                if (tokenx == " " && tokensecx == " " || tokenx == null && tokensecx == null)
                {//do nothing
                }
                else
                {
                    try
                    {
                        progbar.IsIndeterminate = true;
                        progbar.Visibility = Visibility.Visible;
                        ShareOnTwitter();
                    }
                    catch
                    {
                    }
                }
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        public void ShareOnFacebook()
        {
            MainVen.Venue selectedVenue = post_location.SelectedItem as MainVen.Venue;

            string post = "";
            post = "I'm at " + selectedVenue.name;

            if (ffid == " " && fftoken == " " || ffid == null && fftoken == null)
            {
                NavigationService.Navigate(new Uri(string.Format("/FacebookLoginPage.xaml?vpost={0}", post), UriKind.Relative));
            }
            else
            {
                var fb = new FacebookClient(fftoken);

                fb.PostCompleted += (o, args) =>
                {
                    if (args.Error != null)
                    {
                        if (args.Error.Message == "(OAuthException - #506) (#506) Duplicate status message")
                        {
                            Dispatcher.BeginInvoke(() => MessageBox.Show("You have already shared it!"));
                            return;
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(() => MessageBox.Show(args.Error.Message));
                            return;
                        }
                    }

                    var result = (IDictionary<string, object>)args.GetResultData();
                    //_lastMessageId = (string)result["id"];

                    //Dispatcher.BeginInvoke(() =>
                    //{
                    //   // MessageBox.Show("Successfully shared on Facebook");

                    //});
                    Dispatcher.BeginInvoke(() =>
                    {
                        buyNowScreen = new Popup();
                        buyNowScreen.Child =
                            new NotifyAlert
                                ("Successfully shared on Facebook");
                        buyNowScreen.IsOpen = true;
                        buyNowScreen.VerticalOffset = 0;
                        buyNowScreen.HorizontalOffset = 0;
                    });
                };

                //string[] words = post.Split('|');

                //IsolatedSettingsHelper.SetValue<bool>("sharedonfacebook", true);

                var parameters = new Dictionary<string, object>();
                //parameters["message"] = txtMessage.Text;
                parameters["message"] = post;
                //parameters["link"] = words[1];
                //parameters["picture"] = "";

                fb.PostAsync("me/feed", parameters);


                string channel = IsolatedSettingsHelper.GetValue<string>("latest_update_channel");

                IsolatedSettingsHelper.SetValue<string>("latest_update", post);
                IsolatedSettingsHelper.SetValue<string>("latest_update_channel", channel + " Facebook");
                IsolatedSettingsHelper.SetValue<string>("latest_update_time", DateTime.Now.ToString());

                progbar.IsIndeterminate = false;
                progbar.Visibility = Visibility.Collapsed;
            }
        }

        public void ShareOnTwitter()
        {
            MainVen.Venue selectedVenue = post_location.SelectedItem as MainVen.Venue;
            string post = "";
            post = "I'm at "+ selectedVenue.name;

            twitter_post = post;


            if (tokenx == " " && tokensecx == " " || tokenx == null && tokensecx == null)
            {
                NavigationService.Navigate(new Uri(string.Format("/TwitterLoginPage.xaml?vpost={0}", post), UriKind.Relative));
            }
            else
            {
                //////////////////////////
                OAuthCredentials credentials = new OAuthCredentials();

                credentials.Type = OAuthType.ProtectedResource;
                credentials.SignatureMethod = OAuthSignatureMethod.HmacSha1;
                credentials.ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader;
                credentials.ConsumerKey = TwitterSettings.ConsumerKey;
                credentials.ConsumerSecret = TwitterSettings.ConsumerKeySecret;

                credentials.Token = tokenx;
                credentials.TokenSecret = tokensecx;
                credentials.Version = "1.1";
                //credentials.ClientUsername = "";
                credentials.CallbackUrl = "none";

                var service = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);

                service.AuthenticateWith(tokenx, tokensecx);

                SendTweetOptions st = new SendTweetOptions();
                st.Status = "post";
                service.SendTweet(new SendTweetOptions { Status = post }, CallBackVerifiedResponse1);


                ///////////////////////////////

            }
        }
        void CallBackVerifiedResponse1(TwitterStatus at, TwitterResponse response)
        {

            if (response.Error == null && response.StatusCode.ToString() == "OK")
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                   // MessageBox.Show("Successfully shared on Twitter");

                    //IsolatedSettingsHelper.SetValue<bool>("sharedontwitter", true);


                        buyNowScreen1 = new Popup();
                        buyNowScreen1.Child =
                            new NotifyAlert
                                ("Successfully shared on Twitter");
                        buyNowScreen1.IsOpen = true;
                        buyNowScreen1.VerticalOffset = 0;
                        buyNowScreen1.HorizontalOffset = 0;
                   


                    string channel = IsolatedSettingsHelper.GetValue<string>("latest_update_channel");

                    IsolatedSettingsHelper.SetValue<string>("latest_update", twitter_post);
                    IsolatedSettingsHelper.SetValue<string>("latest_update_channel", channel + " Twitter");
                    IsolatedSettingsHelper.SetValue<string>("latest_update_time", DateTime.Now.ToString());

                    progbar.IsIndeterminate = false;
                    progbar.Visibility = Visibility.Collapsed;

                });
            }
            else if (response.Error != null)
            {
                if (response.Error.Message.Contains("duplicate"))
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("You have already shared it!");
                    });
                }
                else
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("An error occurred, please login again.");
                    });
                }


            }


        }

        private void post_location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainVen.Venue selectedVenue = post_location.SelectedItem as MainVen.Venue;
            if(selectedVenue!=null)
            place_text.Text = selectedVenue.name;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            if (buyNowScreen != null)
                buyNowScreen.IsOpen = false;

            if (buyNowScreen1 != null)
                buyNowScreen1.IsOpen = false;
        }
        //////////////////////////////////////////////////////////////////////
    }
}