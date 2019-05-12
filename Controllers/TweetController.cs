using System.Collections.Generic;
using System.Web.Mvc;
using Tweetinvi;
using Umbraco.Web.Mvc;
using H5YR.Core.Models;

using System.Linq;
using System.Web.Configuration;

namespace H5YR.Core.Controllers
{
    public class TweetController : SurfaceController
    {



        public ActionResult Index()
        {

            var consumerKey = WebConfigurationManager.AppSettings["twitterConsumerKey"];
            var consumerSecret = WebConfigurationManager.AppSettings["twitterConsumerSecret"];
            var accessToken = WebConfigurationManager.AppSettings["twitterAccessToken"];
            var accessTokenSecret = WebConfigurationManager.AppSettings["twitterAccessTokenSecret"];


            // You need to make sure your app on dev.twitter.com has read and write permissions if you wish to tweet!
            var creds = Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            Tweetinvi.User.GetAuthenticatedUser(creds);

            var searchResults = Search.SearchTweets("#h5yr");
 

            List<TweetModel> FetchTweets = new List<TweetModel>();


            foreach (var tweet in searchResults)
            {
                FetchTweets.Add(new TweetModel()
                {

                    Username = tweet.CreatedBy.ToString(),
                    Avatar = tweet.CreatedBy.ProfileImageUrlHttps,
                    Twit = tweet.Text,
                    ScreenName = tweet.CreatedBy.ScreenName.ToString(),
                    TweetedOn = tweet.CreatedAt,
                    NumberOfTweets = FetchTweets.Count(),
                    ReplyToTweet = tweet.IdStr

            });

               
            };

            return PartialView("~/Views/Tweets/Index.cshtml", FetchTweets);
        }


    }


}
