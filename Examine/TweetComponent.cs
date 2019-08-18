using Examine;
using Examine.LuceneEngine.Providers;
using Lucene.Net.Index;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;

namespace H5YR.Core.Examine
{
    public class TweetComponent : IComponent
    {
        private readonly IExamineManager _examineManager;
        private readonly TweetIndexCreator _tweetIndexCreator;

        public TweetComponent(IExamineManager examineManager, TweetIndexCreator tweetIndexCreator)
        {
            _examineManager = examineManager;
            _tweetIndexCreator = tweetIndexCreator;
        }
        public void Initialize()
        {
            foreach (var index in _tweetIndexCreator.Create())
            {
                if (index is LuceneIndex)
                {
                    var luceneIndex = index as LuceneIndex;
                    var dir = luceneIndex.GetLuceneDirectory();
                }
                _examineManager.AddIndex(index);

            }
        }
        public void Terminate() { }
    }
}