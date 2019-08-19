using H5YR.Core.Examine;
using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web.Scheduling;

namespace H5YR.Core
{
    // We start by setting up a composer and component so our task runner gets registered on application startup
    public class TweetIndexPopulatorTaskComposer : ComponentComposer<TweetIndexPopulatorTaskComponent>
    {
    }

    public class TweetIndexPopulatorTaskComponent : IComponent
    {
        private IProfilingLogger _logger;
        private IRuntimeState _runtime;
        private BackgroundTaskRunner<IBackgroundTask> _tweetIndexPopulatorTaskRunner;

        public TweetIndexPopulatorTaskComponent(IProfilingLogger logger, IRuntimeState runtime)
        {
            _logger = logger;
            _runtime = runtime;
            _tweetIndexPopulatorTaskRunner = new BackgroundTaskRunner<IBackgroundTask>("TweetIndexPopulatorTask", _logger);
        }

        public void Compose(Composition composition)
        {
            composition.RegisterUnique<ITweetService>();
        }
        public void Initialize()
        {
            int delayBeforeWeStart = 60000; // 60000ms = 1min
            int howOftenWeRepeat = 7200000; //300000ms = 5mins

            var task = new TweetPopulateIndex(_tweetIndexPopulatorTaskRunner, delayBeforeWeStart, howOftenWeRepeat, _runtime, _logger);

            //As soon as we add our task to the runner it will start to run (after its delay period)
            _tweetIndexPopulatorTaskRunner.TryAdd(task);
        }

        public void Terminate()
        {
        }
    }

    // Now we get to define the recurring task
    public class TweetPopulateIndex : RecurringTaskBase
    {
        private IRuntimeState _runtime;
        private IProfilingLogger _logger;

        public TweetPopulateIndex(IBackgroundTaskRunner<RecurringTaskBase> runner, int delayBeforeWeStart, int howOftenWeRepeat, IRuntimeState runtime, IProfilingLogger logger)
            : base(runner, delayBeforeWeStart, howOftenWeRepeat)
        {
            _runtime = runtime;
            _logger = logger;
        }

        public override bool PerformRun()
        {
            try
            {
                var service = new ITweetService();
                service.RebuildIndex("TweetIndex");
            }
            catch (Exception ex)
            {
                _logger.Info<TweetPopulateIndex>("The following error occured with the Scheduled task used to rebuild the Tweet Index :  {ErrorMessage}", ex);
            }

            // If we want to keep repeating - we need to return true
            // But if we run into a problem/error & want to stop repeating - return false
            return true;
        }

        public override bool IsAsync => false;
    }
}