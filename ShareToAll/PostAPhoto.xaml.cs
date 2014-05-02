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
using Facebook;
using System.IO;
using System.Windows.Media.Imaging;
using TweetSharp;
using Hammock.Authentication.OAuth;
using Hammock;
using Hammock.Web;
using System.Net.Http;
using System.Windows.Controls.Primitives;

namespace ShareToAll
{
    public partial class PostAPhoto : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;
        private MemoryStream photoStream;
        string fileName;
        string ffid = " ";
        string fftoken = " ";

        string tokenx = " ";
        string tokensecx = " ";
        WriteableBitmap img;

        public Popup buyNowScreen1;
        public Popup buyNowScreen;

        public PostAPhoto()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.ShowCamera = true;
            photoChooserTask.Completed += photoChooserTask_Completed;
            this.Loaded += PostAPhoto_Loaded;
        }

        void PostAPhoto_Loaded(object sender, RoutedEventArgs e)
        {
            ffid = IsolatedSettingsHelper.GetValue<string>("fid_sharetoall");
            fftoken = IsolatedSettingsHelper.GetValue<string>("ftoken_sharetoall");

            tokenx = IsolatedSettingsHelper.GetValue<string>("ttoken");
            tokensecx = IsolatedSettingsHelper.GetValue<string>("ttokensec");
            photoChooserTask.Show();
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {

                // initialize the result photo stream
                photoStream = new MemoryStream();
                // Save the stream result (copying the resulting stream)
                e.ChosenPhoto.CopyTo(photoStream);
                // save the original file name
                fileName = e.OriginalFileName;
                // display the chosen picture
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(photoStream);
                img = new WriteableBitmap(bitmapImage);
                
                post_image.Source = bitmapImage;
            }

            
        }
        public  void ShareOnFacebook()
        {
            string post = "";
            post = "";

            if (ffid == " " && fftoken == " " || ffid == null && fftoken == null)
            {
                NavigationService.Navigate(new Uri(string.Format("/FacebookLoginPage.xaml?vpost={0}", post), UriKind.Relative));
            }
            else
            {


                var fb = new FacebookClient(fftoken);

              fb.PostCompleted += (o, e) =>
                {
                    if (e.Cancelled || e.Error != null)
                    {
                        Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                        return;
                    }

                    var result = e.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    progbar1.IsIndeterminate = false;
                    progbar1.Visibility = Visibility.Collapsed;
                   // MessageBox.Show("Successfully shared on Facebook");
                    //if (NavigationService.CanGoBack)
                    //    NavigationService.GoBack();

                    buyNowScreen = new Popup();
                    buyNowScreen.Child =
                        new NotifyAlert
                            ("Successfully shared on Facebook");
                    buyNowScreen.IsOpen = true;
                    buyNowScreen.VerticalOffset = 0;
                    buyNowScreen.HorizontalOffset = 0;

                });
                };

              //IsolatedSettingsHelper.SetValue<bool>("sharedonfacebook", true);

              //Dispatcher.BeginInvoke(() =>
              //{
              //    buyNowScreen = new Popup();
              //    buyNowScreen.Child =
              //        new NotifyAlert
              //            ("Successfully shared on Facebook");
              //    buyNowScreen.IsOpen = true;
              //    buyNowScreen.VerticalOffset = 0;
              //    buyNowScreen.HorizontalOffset = 0;
              //});

                var parameters = new Dictionary<string, object>();
                parameters["name"] = photo_text.Text;

                var resized = img.Resize(img.PixelWidth / 2, img.PixelHeight / 2, WriteableBitmapExtensions.Interpolation.Bilinear);

                var fileStream = new MemoryStream();
                resized.SaveJpeg(fileStream, resized.PixelWidth, resized.PixelHeight, 100, 100);
                fileStream.Seek(0, SeekOrigin.Begin);

                parameters["TestPic"] = new FacebookMediaObject
                {
                    ContentType = "image/jpeg",
                    FileName = fileName + ".jpg"

                }.SetValue(fileStream.ToArray());//.SetValue(photoStream.ToArray());

                //fb.PostCompleted += fb_PostCompleted;

             fb.PostAsync("me/Photos", parameters);



                string channel = IsolatedSettingsHelper.GetValue<string>("latest_update_channel");

                IsolatedSettingsHelper.SetValue<string>("latest_update", "You shared a photo");
                IsolatedSettingsHelper.SetValue<string>("latest_update_channel", channel + " Facebook");
                IsolatedSettingsHelper.SetValue<string>("latest_update_time", DateTime.Now.ToString());
            }

        }

        //void fb_PostCompleted(object sender, FacebookApiEventArgs e)
        //{

        //    if (e.Error != null)
        //    {
        //        Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
        //        return;
        //    }
        //    else
        //    {
        //        Dispatcher.BeginInvoke(() =>
        //        {
        //            progbar1.IsIndeterminate = false;
        //            progbar1.Visibility = Visibility.Collapsed;
        //            MessageBox.Show("Successfully shared on Facebook");
        //            //if (NavigationService.CanGoBack)
        //            //    NavigationService.GoBack();

        //        });

        //    }
        //}


        public  void ShareOnTwitter()
        {
            OAuthCredentials credentials = new OAuthCredentials();

            credentials.Type = OAuthType.ProtectedResource;
            credentials.SignatureMethod = OAuthSignatureMethod.HmacSha1;
            credentials.ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader;
            credentials.ConsumerKey = TwitterSettings.ConsumerKey;
            credentials.ConsumerSecret = TwitterSettings.ConsumerKeySecret;

            credentials.Token = tokenx;
            credentials.TokenSecret = tokensecx;
            credentials.Version = "1.0";
            //credentials.ClientUsername = "";
            //credentials.CallbackUrl = "www.google.com";

            //var service = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);

            //service.AuthenticateWith(tokenx, tokensecx);

            //var resized = img.Resize(img.PixelWidth / 4, img.PixelHeight / 4, WriteableBitmapExtensions.Interpolation.Bilinear);

            //var fileStream = new MemoryStream();
            //resized.SaveJpeg(fileStream, resized.PixelWidth, resized.PixelHeight, 100, 100);
            //fileStream.Seek(0, SeekOrigin.Begin);

            //SendTweetWithMediaOptions msgMedia = new SendTweetWithMediaOptions();
            //msgMedia.Status = photo_text.Text;
            //Dictionary<string, Stream> imageDict = new Dictionary<string, Stream> { { fileName, fileStream } };
            ////imageDict.Add(imagePath, imageStream);
            //msgMedia.Images = imageDict;
            ////status = service.SendTweetWithMedia(new SendTweetWithMediaOptions() { Status = readerMsg.Message, Images = imageDict });
            //service.SendTweetWithMedia(msgMedia, tweetResponse);


            //other method

            RestClient _client = new RestClient
            {
                Authority = "https://api.twitter.com",
                HasElevatedPermissions = true,

            };

            var twitterRequest = new RestRequest
            {
                Credentials = credentials,
                Path = "/1.1/statuses/update_with_media.json",
                Method = WebMethod.Post
            };

            //twitterRequest.AddHeader("content-type", "application/x-www-form-urlencoded");

            //twitterRequest.AddHeader("content-type", "multipart/form-data");


            //twitterRequest.AddField("consumer_token", TwitterSettings.ConsumerKey);
            //twitterRequest.AddField("consumer_secret", TwitterSettings.ConsumerKeySecret);
            //twitterRequest.AddField("oauth_token", tokenx);
            //twitterRequest.AddField("oauth_secret", tokensecx);



            var resized = img.Resize(img.PixelWidth / 4, img.PixelHeight / 4, WriteableBitmapExtensions.Interpolation.Bilinear);

            var fileStream = new MemoryStream();
            resized.SaveJpeg(fileStream, resized.PixelWidth, resized.PixelHeight, 100, 100);
            fileStream.Seek(0, SeekOrigin.Begin);


            //twitterRequest.AddParameter("status", photo_text.Text);
            twitterRequest.AddField("status", photo_text.Text); 
            twitterRequest.AddFile("media[]", "test", fileStream, "image/jpeg");


            _client.BeginRequest(twitterRequest, NewTweetCompleted);

            // another method
           // var resized = img.Resize(img.PixelWidth / 4, img.PixelHeight / 4, WriteableBitmapExtensions.Interpolation.Bilinear);

           // var fileStream = new MemoryStream();
           // resized.SaveJpeg(fileStream, resized.PixelWidth, resized.PixelHeight, 100, 100);
           // fileStream.Seek(0, SeekOrigin.Begin);


           // string fileUploadUrl = "http://api.twitter.com/1.1/statuses/update_with_media.json";

           // HttpClientHandler handler = new HttpClientHandler();
           //// handler.Credentials = new NetworkCredential("username", "password");
           // handler.Credentials = new NetworkCredential(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);
           // HttpClient client = new HttpClient(handler);

           // fileStream.Position = 0;
           // MultipartFormDataContent content = new MultipartFormDataContent();
           // content.Add(new StreamContent(fileStream), "media[]", fileName);

           // await client.PostAsync(fileUploadUrl, content)
           //     .ContinueWith((postTask) =>
           //     {
           //         postTask.Result.EnsureSuccessStatusCode();
           //     });

        }

        private void NewTweetCompleted(RestRequest request, RestResponse response, object userstate)
        {
            // We want to ensure we are running on our thread UI
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //MessageBox.Show("Successfully shared on Twitter");
                    progbar1.IsIndeterminate = false;
                    progbar1.Visibility = Visibility.Collapsed;

                   // IsolatedSettingsHelper.SetValue<bool>("sharedontwitter", true);

                    buyNowScreen1 = new Popup();
                    buyNowScreen1.Child =
                        new NotifyAlert
                            ("Successfully shared on Twitter");
                    buyNowScreen1.IsOpen = true;
                    buyNowScreen1.VerticalOffset = 0;
                    buyNowScreen1.HorizontalOffset = 0;

                    string channel = IsolatedSettingsHelper.GetValue<string>("latest_update_channel");

                    IsolatedSettingsHelper.SetValue<string>("latest_update", "You shared a photo");
                    IsolatedSettingsHelper.SetValue<string>("latest_update_channel", channel + " Twitter");
                    IsolatedSettingsHelper.SetValue<string>("latest_update_time", DateTime.Now.ToString());

                    //if (NavigationService.CanGoBack)
                    //    NavigationService.GoBack();
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Authentication error"); 
                    }
                    else
                         MessageBox.Show("Error, please try again later");
                }
            });
        }
        private void tweetResponse(TwitterStatus tweet, TwitterResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Successfully shared on Twitter");
                    progbar1.IsIndeterminate = false;
                    progbar1.Visibility = Visibility.Collapsed;
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();

                });
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Dispatcher.BeginInvoke(() => { MessageBox.Show("Authentication error"); });
                }
                else
                    Dispatcher.BeginInvoke(() => { MessageBox.Show("Error, please try again later"); });
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (photoStream != null)
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
                        progbar1.IsIndeterminate = true;
                        progbar1.Visibility = Visibility.Visible;
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
                        progbar1.IsIndeterminate = true;
                        progbar1.Visibility = Visibility.Visible;
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

        private void retake_Click(object sender, EventArgs e)
        {
            photoChooserTask.Show();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            if (buyNowScreen != null)
                buyNowScreen.IsOpen = false;

            if (buyNowScreen1 != null)
                buyNowScreen1.IsOpen = false;
        }

        ///////////////////////////////////////////////////////
    }
}