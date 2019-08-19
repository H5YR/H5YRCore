using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Examine;
using H5YR.Core.Models;

namespace H5YR.Core.Examine
{
    public class TweetIndexValueSetBuilder : IValueSetBuilder<TweetModel>
    {
        public IEnumerable<ValueSet> GetValueSets(params TweetModel[] tweets)
        {
            foreach (var tweet in tweets)
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

                var valueSet = new ValueSet(tweet.Url.ToString(), "tweet", indexValues);

                yield return valueSet;
            }
        }
    }
}