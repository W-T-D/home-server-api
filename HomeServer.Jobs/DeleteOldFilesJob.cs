using HomeServer.Common.Extensions;
using HomeServer.Data;
using HomeServer.Data.Files;
using HomeServer.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace HomeServer.Jobs;

[DisallowConcurrentExecution]
public class DeleteOldFilesJob(
    EFDbContext dbContext,
    ILogger<DeleteOldFilesJob> logger,
    IFileProvider fileProvider,
    IOptions<ServerOptions> serverOptions) : IJob
{
    private readonly ServerOptions _serverOptions = serverOptions.Value;
    
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation($"{nameof(DeleteOldFilesJob)} started.");
        var waitPeriodEnd = DateTime.UtcNow.AddDays(_serverOptions.StoreDeletedFileDays * -1);
        var filesToDelete = await dbContext.FileInfos
            .Where(fi => fi.IsDeleted && fi.ModifyDate <= waitPeriodEnd)
            .ToListAsync();

        if (filesToDelete.Count == 0)
        {
            logger.LogInformation($"{nameof(DeleteOldFilesJob)} finished. No files to delete.");
            return;
        }
        
        const int batchSize = 32;
        var batches = filesToDelete.Batch(batchSize);
        
        foreach (var batch in batches)
        {
            foreach (var fileInfo in batch)
            {
                var path = GetFileLocation(fileInfo.Id);
                fileProvider.DeleteFile(path);
                
                dbContext.Remove(fileInfo);
            }
            await dbContext.SaveChangesAsync();
        }
        
        logger.LogInformation($"{nameof(DeleteOldFilesJob)} finished. Total files deleted: {filesToDelete.Count}.");
    }
    
    private string GetFileLocation(Guid id) => Path.Combine(_serverOptions.FilesLocation, id.ToString());
}