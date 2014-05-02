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

namespace ShareToAll
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        private const string AppId = "1419648711631169";
        private const string ExtendedPermissions = "user_about_me,read_stream,publish_stream";
        string _post;

        private readonly FacebookClient _fb = new FacebookClient();
        public FacebookLoginPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            _post = NavigationContext.QueryString["vpost"];

        }
        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
            webBrowser1.Navigate(loginUrl);
            IsolatedSettingsHelper.SetValue<string>("loginUri", loginUrl.ToString());
        }
        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                LoginSucceded(accessToken);

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var id = (string)result["id"];

                IsolatedSettingsHelper.SetValue<string>("ftoken_sharetoall", accessToken);
                IsolatedSettingsHelper.SetValue<string>("fid_sharetoall", id);

                // string _post = NavigationContext.QueryString["vpost"];

                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Successfully shared on Facebook");

                });

                if (_post.Contains('|'))
                {
                    string[] words = _post.Split('|');

                    var parameters = new Dictionary<string, object>();
                    //parameters["message"] = txtMessage.Text;
                    parameters["message"] = words[0];
                    parameters["link"] = words[1];
                    //parameters["picture"] = "";

                    fb.PostAsync("me/feed", parameters);
                }
                else
                {
                    var parameters = new Dictionary<string, object>();
                    //parameters["message"] = txtMessage.Text;
                    parameters["message"] = _post;
                   // parameters["link"] = words[1];
                    //parameters["picture"] = "";

                    fb.PostAsync("me/feed", parameters);

                }
                //////////
            };

            fb.GetAsync("me?fields=id");
        }

        /////////////////////////////////////////////////////////////////////
    }
}