using System.Collections.Generic;
using System.Web.Mvc;
using Tweetinvi;
using Umbraco.Web.Mvc;
using H5YR.Core.Models;

using System.Linq;
using System.Web.Configuration;
using System;

namespace H5YR.Core.Controllers
{
    public class TweetController : SurfaceController
    {
        private string consumerKey => WebConfigurationManager.AppSettings["twitterConsumerKey"];
        private string consumerSecret => WebConfigurationManager.AppSettings["twitterConsumerSecret"];
        private string accessToken => WebConfigurationManager.AppSettings["twitterAccessToken"];
        private string accessTokenSecret => WebConfigurationManager.AppSettings["twitterAccessTokenSecret"];


        public ActionResult Index()
        {
            List<TweetModel> tweets = GetAllTweets(0,12);

            Session["NumberOfTweetsDisplayed"] = 12;

            return PartialView("~/Views/Tweets/Index.cshtml", tweets);
        }

        public ActionResult GetMoreTweets()
        {
           
            var tweetsToSkip = Convert.ToInt32(Session["NumberOfTweetsDisplayed"]);

            List<TweetModel> tweets = GetAllTweets(tweetsToSkip, 12);

            Session["NumberOfTweetsDisplayed"] = tweetsToSkip + 12;

            return PartialView("~/Views/Tweets/TweetsFeed.cshtml", tweets);
        }

        public List<TweetModel> GetAllTweets(int tweetsToSkip = 0, int tweetsToReturn = 12)
        {
            // You need to make sure your app on dev.twitter.com has read and write permissions if you wish to tweet!
            var creds = Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            Tweetinvi.User.GetAuthenticatedUser(creds);

            var searchResults = Search.SearchTweets("#h5yr");


            List<TweetModel> FetchTweets = new List<TweetModel>();


            foreach (var tweet in searchResults.Skip(tweetsToSkip).Take(tweetsToReturn))
            {
                FetchTweets.Add(new TweetModel()
                {

                    Username = tweet.CreatedBy.ToString(),
                    Avatar = tweet.CreatedBy.ProfileImageUrlHttps,
                    Content = tweet.Text,
                    ScreenName = tweet.CreatedBy.ScreenName.ToString(),
                    TweetedOn = tweet.CreatedAt,
                    NumberOfTweets = FetchTweets.Count(),
                    ReplyToTweet = tweet.IdStr,
                    Url = tweet.Url

                });


            };

            return FetchTweets;
        }


    }


}
