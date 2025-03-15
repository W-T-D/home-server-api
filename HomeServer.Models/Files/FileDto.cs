namespace HomeServer.Models.Files;

public class FileDto
{
    public MemoryStream Data { get; set; }
    
    public FileInfoDto Info { get; set; } 
}