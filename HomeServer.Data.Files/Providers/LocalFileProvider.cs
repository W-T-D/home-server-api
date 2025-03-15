namespace HomeServer.Data.Files.Providers;

public class LocalFileProvider : IFileProvider
{
    public async Task<MemoryStream> LoadFileAsync(string path, CancellationToken ctx = new())
    {
        await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        var ms = new MemoryStream();
        await fs.CopyToAsync(ms, ctx);

        return ms;
    }

    public async Task SaveFileAsync(string path, MemoryStream stream, CancellationToken ctx = new())
    {
        await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        await fs.WriteAsync(stream.ToArray(), ctx);
    }

    public void DeleteFile(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }
        
        File.Delete(path);
    }
}