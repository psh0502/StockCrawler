using Quartz;
using System;
using System.Collections.Generic;
using System.Threading;

namespace StockCrawler.Services
{
    public class ArgumentJobExecutionContext : IJobExecutionContext
    {
        public ArgumentJobExecutionContext(IJob job)
        {
            Job = job;
        }
        public IJob Job { get; private set; }
        public IScheduler Scheduler => throw new NotImplementedException();

        public ITrigger Trigger => throw new NotImplementedException();

        public ICalendar Calendar => throw new NotImplementedException();

        public bool Recovering => throw new NotImplementedException();

        public TriggerKey RecoveringTriggerKey => throw new NotImplementedException();

        public int RefireCount => throw new NotImplementedException();

        public JobDataMap MergedJobDataMap => throw new NotImplementedException();

        public IJobDetail JobDetail => throw new NotImplementedException();

        public IJob JobInstance => throw new NotImplementedException();

        public DateTimeOffset FireTimeUtc => throw new NotImplementedException();

        public DateTimeOffset? ScheduledFireTimeUtc => throw new NotImplementedException();

        public DateTimeOffset? PreviousFireTimeUtc => throw new NotImplementedException();

        public DateTimeOffset? NextFireTimeUtc => throw new NotImplementedException();

        public string FireInstanceId => throw new NotImplementedException();

        public object Result { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TimeSpan JobRunTime => throw new NotImplementedException();

        public CancellationToken CancellationToken => throw new NotImplementedException();
        private Dictionary<object, object> DataDictionary { get; set; } = new Dictionary<object, object>();
        public object Get(object key)
        {
            if (DataDictionary.TryGetValue(key, out object result))
                return result;
            else
                return null;
        }

        public void Put(object key, object objectValue)
        {
            DataDictionary[key] = objectValue;
        }
    }
}
