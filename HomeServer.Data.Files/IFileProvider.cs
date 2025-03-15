namespace HomeServer.Data.Files;

public interface IFileProvider
{
    Task<MemoryStream> LoadFileAsync(string path, CancellationToken ctx = new());
    
    Task SaveFileAsync(string path, MemoryStream stream, CancellationToken ctx = new());
    
    void DeleteFile(string path);
}