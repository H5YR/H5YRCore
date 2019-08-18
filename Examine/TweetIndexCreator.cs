using Examine;
using H5YR.Core.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using System.Collections.Generic;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Examine;
using Umbraco.Web.Search;
using Examine.LuceneEngine.Providers;

namespace H5YR.Core.Examine
{
    public class TweetIndexCreator : LuceneIndexCreator
    {
        public override IEnumerable<IIndex> Create()
        {
            var index = new LuceneIndex("TweetIndex",
                CreateFileSystemLuceneDirectory("TweetIndex"),
                new FieldDefinitionCollection(
                    new FieldDefinition("Content", FieldDefinitionTypes.FullTextSortable),
                    new FieldDefinition("Username", FieldDefinitionTypes.FullText),
                    new FieldDefinition("Screenname", FieldDefinitionTypes.FullText),
                    new FieldDefinition("Avatar", FieldDefinitionTypes.FullText),
                    new FieldDefinition("TweetedOn", FieldDefinitionTypes.FullText),
                    new FieldDefinition("NumberOfTweets", FieldDefinitionTypes.FullText),
                    new FieldDefinition("Url", FieldDefinitionTypes.FullText)
                ),
                new StandardAnalyzer(Version.LUCENE_30));

            return new[] { index };
        }
    }
}