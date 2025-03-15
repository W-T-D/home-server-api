namespace HomeServer.Models.Configurations;

public class ServerOptions
{
    public string FilesLocation { get; set; }
    
    public int FileSizeLimit { get; set; }
    
    public int StoreDeletedFileDays { get; set; }
}