namespace HomeServer.Models.Files;

public class FileInfoDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Location { get; set; }

    public string ContentType { get; set; }
    
    public DateTime LastModifyDate { get; set; }

    public ulong Size { get; set; }
}