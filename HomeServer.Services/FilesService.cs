using System.Security.Cryptography;
using HomeServer.Common;
using HomeServer.Data;
using HomeServer.Data.Files;
using HomeServer.Models.Configurations;
using HomeServer.Models.Files;
using HomeServer.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FileInfo = HomeServer.Data.Models.FileInfo;

namespace HomeServer.Services;

public interface IFilesService
{
    Task<Result<FileInfoDto>> UploadAsync(FileDto fileDto, CancellationToken ctx = new());
    Task<Result<FileInfoPageResultDto>> GetFileListAsync(SearchFileRequestDto requestDto, CancellationToken ctx = new());
    Task<Result<FileDto>> GetByIdAsync(Guid id, CancellationToken ctx = new());
    Task<Result<FileInfoDto>> DeleteAsync(Guid id, CancellationToken ctx = new());
}

public class FilesService(IOptions<ServerOptions> serverOptions, EFDbContext dbContext, IFileProvider fileProvider)
    : IFilesService
{
    private readonly ServerOptions _serverOptions = serverOptions.Value;

    public async Task<Result<FileInfoDto>> UploadAsync(FileDto fileDto, CancellationToken ctx = new())
    {
        fileDto.Info.Id = await GenerateIdAsync(fileDto.Data, ctx);
        
        var existingFile = dbContext.FileInfos.SingleOrDefault(f => f.Id == fileDto.Info.Id);
        
        if (existingFile is not null)
        {
            if (existingFile.IsDeleted)
            {
                await RestoreFile(existingFile, ctx);
            }
            
            fileDto.Info = existingFile.ToDto(GetFileLocation(fileDto.Info.Id));
            return Result<FileInfoDto>.Success(fileDto.Info);
        }

        var fileInfoEntity = InitializeFileEntityWithMetadata(fileDto);
        
        dbContext.Set<FileInfo>().Add(fileInfoEntity);

        fileDto.Info = fileInfoEntity.ToDto(GetFileLocation(fileDto.Info.Id));
        await fileProvider.SaveFileAsync(GetFileLocation(fileDto.Info.Id), fileDto.Data, ctx);
        
        await dbContext.SaveChangesAsync(ctx);
        
        return Result<FileInfoDto>.Success(fileDto.Info);
    }
    
    public async Task<Result<FileInfoPageResultDto>> GetFileListAsync(
        SearchFileRequestDto requestDto,
        CancellationToken ctx = new())
    {
        var filesQuery = dbContext.FileInfos
            .Where(f => !f.IsDeleted);

        var count = await filesQuery.CountAsync(ctx);
        
        if (!string.IsNullOrEmpty(requestDto.Filter))
        {
            filesQuery = filesQuery.Where(f => f.Name.Contains(requestDto.Filter));
        }
        
        var files = await filesQuery
            .OrderByDescending(f =>f.CreateDate)
            .Skip(requestDto.Skip)
            .Take(requestDto.Take)
            .ToListAsync(ctx);
        
        var filesDtos = files.Select(f => f.ToDto(GetFileLocation(f.Id))).ToList();

        var pageResult = new FileInfoPageResultDto
        {
            Files = filesDtos,
            TotalCount = count
        };
        
        return Result<FileInfoPageResultDto>.Success(pageResult);
    }
    
    public async Task<Result<FileDto>> GetByIdAsync(Guid id, CancellationToken ctx = new())
    {
        var file = await dbContext.FileInfos.SingleOrDefaultAsync(f => f.Id == id, ctx);
        
        if (file is null)
        {
            return Result<FileDto>.Failure(Error.RecordNotFound("File not found"));
        }

        var fileLocation = GetFileLocation(file.Id);
        
        var fileStream = await fileProvider.LoadFileAsync(fileLocation, ctx);
        
        var fileDto = new FileDto
        {
            Data = fileStream,
            Info = file.ToDto(fileLocation)
        };
        
        return Result<FileDto>.Success(fileDto);
    }
    
    public async Task<Result<FileInfoDto>> DeleteAsync(Guid id, CancellationToken ctx = new())
    {
        var file = await dbContext.FileInfos.SingleOrDefaultAsync(f => f.Id == id, ctx);
        
        if (file is null)
        {
            return Result<FileInfoDto>.Failure(Error.RecordNotFound("File not found"));
        }

        file.IsDeleted = true;
        file.ModifyDate = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync(ctx);
        
        return Result<FileInfoDto>.Success(file.ToDto());
    }

    private async Task<Guid> GenerateIdAsync(MemoryStream stream, CancellationToken ctx = new())
    {
        stream.Seek(0, SeekOrigin.Begin);
        var hashBytes = await MD5.HashDataAsync(stream, ctx);

        return new Guid(hashBytes);
    }

    private async Task RestoreFile(FileInfo existingFile, CancellationToken ctx = new())
    {
        existingFile.IsDeleted = false;
        existingFile.ModifyDate = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(ctx);
    }

    private FileInfo InitializeFileEntityWithMetadata(FileDto fileDto)
    {
        var fileInfoEntity = fileDto.Info.ToEntity();
        fileInfoEntity.CreateDate = DateTime.UtcNow;
        fileInfoEntity.Size = (ulong)fileDto.Data.ToArray().Length;
        
        return fileInfoEntity;
    }

    private string GetFileLocation(Guid id) => Path.Combine(_serverOptions.FilesLocation, id.ToString());
}