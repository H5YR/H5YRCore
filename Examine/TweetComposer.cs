using Umbraco.Core;
using Umbraco.Core.Composing;
using H5YR.Core;

namespace H5YR.Core.Examine
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class TweetComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<TweetComponent>();

            composition.RegisterUnique<TweetIndexValueSetBuilder>();
            composition.Register<TweetIndexPopulator>(Lifetime.Singleton);
            composition.RegisterUnique<TweetIndexCreator>();
            composition.RegisterUnique<ITweetService>();
        }
    }
}