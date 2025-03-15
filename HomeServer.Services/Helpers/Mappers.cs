using HomeServer.Models.Files;
using FileInfo = HomeServer.Data.Models.FileInfo;

namespace HomeServer.Services.Helpers;

public static class FileInfoMapper
{
    public static FileInfo ToEntity(this FileInfoDto fileInfoDto)
    {
        return new FileInfo
        {
            Id = fileInfoDto.Id,
            Name = fileInfoDto.Name,
            ContentType = fileInfoDto.ContentType,
            Size = fileInfoDto.Size,
        };
    }
    
    public static FileInfoDto ToDto(this FileInfo fileInfo, string? location = null)
    {
        return new FileInfoDto
        {
            Id = fileInfo.Id,
            Name = fileInfo.Name,
            Location = location,
            ContentType = fileInfo.ContentType,
            LastModifyDate = fileInfo.ModifyDate ?? fileInfo.CreateDate,
            Size = fileInfo.Size
        };
    }
}