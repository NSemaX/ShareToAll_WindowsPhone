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
using Hammock.Authentication.OAuth;
using TweetSharp;
using ShareToAll.Resources;
using System.Windows.Controls.Primitives;

namespace ShareToAll
{
    public partial class PostAnUpdate : PhoneApplicationPage
    {
        private ProgressIndicator ProgressIndicator = null;
        private string _lastMessageId;
        string ffid = " ";
        string fftoken = " ";

        string tokenx = " ";
        string tokensecx = " ";

        string twitter_post = "";

        public Popup buyNowScreen;
        public Popup buyNowScreen1;


        public PostAnUpdate()
        {
            InitializeComponent();
            this.Loaded += PostAnUpdate_Loaded;
           
        }

        void PostAnUpdate_Loaded(object sender, RoutedEventArgs e)
        {
            ffid = IsolatedSettingsHelper.GetValue<string>("fid_sharetoall");
            fftoken = IsolatedSettingsHelper.GetValue<string>("ftoken_sharetoall");

            tokenx = IsolatedSettingsHelper.GetValue<string>("ttoken");
            tokensecx = IsolatedSettingsHelper.GetValue<string>("ttokensec");

            update_text.Focus();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (update_text.Text.Count() > 0)
            {
                progbar.IsIndeterminate = true;
                progbar.Visibility = Visibility.Visible;
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

                progbar.IsIndeterminate = false;
                progbar.Visibility = Visibility.Collapsed;
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
            //ShowProgressIndicator(AppResources.GettingLocationProgressText);
            string post = "";
            post = update_text.Text;

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
                    _lastMessageId = (string)result["id"];


                    string channel=  IsolatedSettingsHelper.GetValue<string>("latest_update_channel");

                    IsolatedSettingsHelper.SetValue<string>("latest_update", post);
                    IsolatedSettingsHelper.SetValue<string>("latest_update_channel", channel +" Facebook");
                    IsolatedSettingsHelper.SetValue<string>("latest_update_time", DateTime.Now.ToString());

                    //Dispatcher.BeginInvoke(() =>
                    //{
                    //   // MessageBox.Show("Successfully shared on Facebook");

                    //});
                    Dispatcher.BeginInvoke(() =>
                    {
                    buyNowScreen1 = new Popup();
                    buyNowScreen1.Child =
                        new NotifyAlert
                            ("Successfully shared on Facebook");
                    buyNowScreen1.IsOpen = true;
                    buyNowScreen1.VerticalOffset = 0;
                    buyNowScreen1.HorizontalOffset = 0;
                    });

                    //IsolatedSettingsHelper.SetValue<bool>("sharedonfacebook", true);
                };

               // string[] words = post.Split('|');

                var parameters = new Dictionary<string, object>();
                //parameters["message"] = txtMessage.Text;
                parameters["message"] = post;
                //parameters["link"] = words[1];
                //parameters["picture"] = "";

                fb.PostAsync("me/feed", parameters);

                progbar.IsIndeterminate = false;
                progbar.Visibility = Visibility.Collapsed;
               // HideProgressIndicator();
            }
        }

        public void ShareOnTwitter()
        {
            string post = "";
            post = update_text.Text;

            if (post.Length > 140)
                post = post.Substring(0, 137) + "…";
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
               // ShowProgressIndicator(AppResources.GettingLocationProgressText);

                ///////////////////////////////

            }
        }
        void CallBackVerifiedResponse1(TwitterStatus at, TwitterResponse response)
        {

            if (response.Error == null && response.StatusCode.ToString() == "OK")
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //MessageBox.Show("Successfully shared on Twitter.");

                    progbar.IsIndeterminate = false;
                    progbar.Visibility = Visibility.Collapsed;

                });


                //IsolatedSettingsHelper.SetValue<bool>("sharedontwitter", true);

                //if (IsolatedSettingsHelper.GetValue<bool>("sharedontwitter"))
                //{
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
                //}

                
                string channel = IsolatedSettingsHelper.GetValue<string>("latest_update_channel");

                IsolatedSettingsHelper.SetValue<string>("latest_update", twitter_post);
                IsolatedSettingsHelper.SetValue<string>("latest_update_channel", channel + " Twitter");
                IsolatedSettingsHelper.SetValue<string>("latest_update_time", DateTime.Now.ToString());

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
                        MessageBox.Show("An error occurred ,please login again.");
                    });
                }


            }
            //HideProgressIndicator();

        }

        private void ShowProgressIndicator(String msg)
        {
            if (ProgressIndicator == null)
            {
                ProgressIndicator = new ProgressIndicator();
                ProgressIndicator.IsIndeterminate = true;
            }
            ProgressIndicator.Text = msg;
            ProgressIndicator.IsVisible = true;
            SystemTray.SetProgressIndicator(this, ProgressIndicator);
        }

        private void HideProgressIndicator()
        {
            ProgressIndicator.IsVisible = false;
            SystemTray.SetProgressIndicator(this, ProgressIndicator);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            if (buyNowScreen != null)
                buyNowScreen.IsOpen = false;

            if (buyNowScreen1 != null)
                buyNowScreen1.IsOpen = false;
        }



        ////////////////////////////////////////////////////////////////////
    }
}