using HomeServer.Jobs;
using Quartz;

namespace HomeServer.Api.Extensions;

public static class JobsSettings
{
    public static IServiceCollection AddBackgroundJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
        
        builder.Services.AddQuartz(opt =>
        {
            var interval = builder.Configuration.GetSection("ServerOptions:StoreDeletedFileDays").Get<int>();
            var jobKey = new JobKey(nameof(DeleteOldFilesJob));
            opt.AddJob<DeleteOldFilesJob>(jobKey);
            opt.AddTrigger(t => t
                .ForJob(jobKey)
                .WithIdentity("DeleteOldFilesJobTrigger")
                .WithSimpleSchedule(s 
                    => s.WithInterval(TimeSpan.FromDays(interval)).RepeatForever()));
        });
        
        return builder.Services;
    }
}