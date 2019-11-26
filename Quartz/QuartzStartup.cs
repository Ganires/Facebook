using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookService.Quartz
{
    public class QuartzStartup
    {
        private static IScheduler _scheduler; // after Start, and until shutdown completes, references the scheduler object        

        // starts the scheduler, defines the jobs and the triggers
        public static void Start(IServiceProvider container)
        {
            var jovFactory = container.GetService(typeof(IJobFactory)) as IJobFactory;
            if (_scheduler != null)
            {
                throw new InvalidOperationException("Already started.");
            }

            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.JobFactory = new IoCJobFactory(container);
            _scheduler.Start().Wait();

            var voteJob = JobBuilder.Create<Job>()
                .Build();

            var voteJobTrigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(s => s
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();

            _scheduler.ScheduleJob(voteJob, voteJobTrigger).Wait();
        }

        // initiates shutdown of the scheduler, and waits until jobs exit gracefully (within allotted timeout)
        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }

            // give running jobs 30 sec (for example) to stop gracefully
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
            {
                _scheduler = null;
            }
            else
            {
                // jobs didn't exit in timely fashion - log a warning...
            }
        }
    }
}
