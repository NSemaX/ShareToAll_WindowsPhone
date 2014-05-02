using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TweetSharp;
using Hammock.Authentication.OAuth;

namespace ShareToAll
{
    public partial class TwitterLoginPage : PhoneApplicationPage
    {
        public string post = "";
        private OAuthRequestToken _requestToken;
        private TwitterService service;

        public TwitterLoginPage()
        {
            InitializeComponent();
            this.Loaded += TwitterLoginPage_Loaded;
        }
        void TwitterLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            post = NavigationContext.QueryString["vpost"];
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            GetTwitterToken();
        }
        private void GetTwitterToken()
        {
            service = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);
            var cb = new Action<OAuthRequestToken, TwitterResponse>(CallBackToken);
            service.GetRequestToken("oob", CallBackToken);
        }


        void CallBackToken(OAuthRequestToken rt, TwitterResponse response)
        {
            if (rt != null)
            {
                Uri uri = service.GetAuthorizationUri(rt);
                _requestToken = rt;
                BrowserControl.Dispatcher.BeginInvoke(() => BrowserControl.Navigate(uri));
            }
            else
            {
                MessageBox.Show("Sorry, can not access the data.Please try again later.");
                return;
            }
        }

        void CallBackVerifiedResponse(OAuthAccessToken at, TwitterResponse response)
        {
            if (at != null)
            {



                IsolatedSettingsHelper.SetValue<string>("ttoken", at.Token);
                IsolatedSettingsHelper.SetValue<string>("ttokensec", at.TokenSecret);

                try
                {

                    Dispatcher.BeginInvoke(() =>
                    {
                        //////////////////////////
                        OAuthCredentials credentials = new OAuthCredentials();

                        credentials.Type = OAuthType.ProtectedResource;
                        credentials.SignatureMethod = OAuthSignatureMethod.HmacSha1;
                        credentials.ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader;
                        credentials.ConsumerKey = TwitterSettings.ConsumerKey;
                        credentials.ConsumerSecret = TwitterSettings.ConsumerKeySecret;

                        credentials.Token = at.Token;
                        credentials.TokenSecret = at.TokenSecret;
                        credentials.Version = "1.1";
                        //credentials.ClientUsername = "";
                        credentials.CallbackUrl = "none";

                        var service = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);

                        service.AuthenticateWith(at.Token, at.TokenSecret);

                        SendTweetOptions st = new SendTweetOptions();
                        st.Status = "post";
                        service.SendTweet(new SendTweetOptions { Status = post }, CallBackVerifiedResponse1);

                        ///////////////////////////////
                    });
                }
                catch
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("An error occurred,please try again.");
                        return;
                    });

                }



            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Wrong pin please try again", "Error", MessageBoxButton.OK);
                    GetTwitterToken();
                });
            }
        }
        void CallBackVerifiedResponse1(TwitterStatus at, TwitterResponse response)
        {

            if (response.Error == null && response.StatusCode.ToString() == "OK")
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Successfully shared on Twitter");
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                });
            }
            else if (response.Error != null)
            {
                if (response.Error.Message.Contains("duplicate"))
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("You have already shareid it!");
                        if (NavigationService.CanGoBack)
                            NavigationService.GoBack();
                    });
                }
                else
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("An error occurred,please try again.");
                    });
                }


            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(pinText.Text))
                MessageBox.Show("Please firstly sign in, then type pin to this area.");
            else
            {
                try
                {
                    var cb = new Action<OAuthAccessToken, TwitterResponse>(CallBackVerifiedResponse);
                    service.GetAccessToken(_requestToken, pinText.Text, CallBackVerifiedResponse);

                }
                catch
                {
                    MessageBox.Show("Wrong pin please try again", "Error", MessageBoxButton.OK);
                }
            }
        }

        ////////////////////////////////////////////////////////////////
    }
}