using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Examine;
using H5YR.Core.Models;

namespace H5YR.Core.Examine
{
    public class TweetIndexPopulator : IndexPopulator
    {
        private readonly TweetIndexValueSetBuilder _tweetValueSetBuilder;
        private readonly ITweetService _tweetService;

        public TweetIndexPopulator(TweetIndexValueSetBuilder tweetValueSetBuilder, ITweetService tweetService)
        {
            _tweetValueSetBuilder = tweetValueSetBuilder;
            _tweetService = tweetService;

            RegisterIndex("TweetIndex");
        }

        protected override void PopulateIndexes(IReadOnlyList<IIndex> indexes)
        {
            var tweets = _tweetService.GetValueSets();


            if (tweets != null && tweets.Any())
            {
                foreach (var index in indexes)
                {
                    index.IndexItems(_tweetValueSetBuilder.GetValueSets(tweets.ToArray()));
                }
            }
        }
    }
}