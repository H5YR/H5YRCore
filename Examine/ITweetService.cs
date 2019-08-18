using Examine;
using H5YR.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Examine;
using H5YR.Core.Models;
using Examine.LuceneEngine.Providers;
using System.Web.Configuration;
using Tweetinvi;

namespace H5YR.Core.Examine
{
    public class ITweetService : TweetIndexValueSetBuilder
    {
        private string consumerKey => WebConfigurationManager.AppSettings["twitterConsumerKey"];
        private string consumerSecret => WebConfigurationManager.AppSettings["twitterConsumerSecret"];
        private string accessToken => WebConfigurationManager.AppSettings["twitterAccessToken"];
        private string accessTokenSecret => WebConfigurationManager.AppSettings["twitterAccessTokenSecret"];

        public List<TweetModel> GetValueSets(params TweetModel[] content)
        {
            var controller = new TweetController();
            var viewModels = controller.GetAllTweets();

            List<TweetModel> FetchTweets = new List<TweetModel>();

            foreach (var tweet in viewModels)
            {
                FetchTweets.Add(new TweetModel()
                {

                    Username = tweet.Username,
                    Avatar = tweet.Avatar,
                    Content = tweet.Content,
                    ScreenName = tweet.ScreenName,
                    TweetedOn = tweet.TweetedOn,
                    NumberOfTweets = FetchTweets.Count(),
                    ReplyToTweet = tweet.ReplyToTweet,
                    Url = tweet.Url

                });
            };

            return FetchTweets;
        }

        private readonly TweetIndexValueSetBuilder _tweetValueSetBuilder;
        public void PopulateIndexes(IReadOnlyList<IIndex> indexes)
        {
            var tweets = GetValueSets();


            if (tweets != null && tweets.Any())
            {
                foreach (var index in indexes)
                {
                    index.IndexItems(_tweetValueSetBuilder.GetValueSets(tweets.ToArray()));
                }
            }
        }


        public IEnumerable<ValueSet> GetTweetValueSet()
        {
            var controller = new TweetController();
            var viewModels = controller.GetAllTweets();

            List<TweetModel> FetchTweets = new List<TweetModel>();

            foreach (var tweet in viewModels)
            {
                var indexValues = new Dictionary<string, object>
                {
                    ["Content"] = tweet.Content,
                    ["Username"] = tweet.Username,
                    ["Screenname"] = tweet.ScreenName,
                    ["Avatar"] = tweet.Avatar,
                    ["TweetedOn"] = tweet.TweetedOn,
                    ["NumberOfTweets"] = tweet.NumberOfTweets,
                    ["ReplyToTweet"] = tweet.ReplyToTweet,
                    ["Url"] = tweet.Url,
                };

                yield return new ValueSet(tweet.Url.ToString(), "tweet", indexValues);
            }
        }

        private readonly IEnumerable<IIndexPopulator> _populators;

        public void RebuildIndex(string indexName)
        {
            if (!ExamineManager.Instance.TryGetIndex(indexName, out var index))
                throw new InvalidOperationException($"No index found with name {indexName}");

            if (index is LuceneIndex luceneIndex)
                using (luceneIndex.ProcessNonAsync())
                    try
                    {
                        index.IndexItems(GetTweetValueSet());
                        var success = true;
                    }
                    catch (Exception ex)
                    {
                        var error = ex;
                    }
        }
    }
}