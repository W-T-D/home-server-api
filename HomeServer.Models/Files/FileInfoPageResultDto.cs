namespace HomeServer.Models.Files;

public class FileInfoPageResultDto
{
    public IReadOnlyCollection<FileInfoDto> Files { get; set; }

    public int TotalCount { get; set; }
}