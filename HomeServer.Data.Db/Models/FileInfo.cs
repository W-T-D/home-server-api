namespace HomeServer.Data.Models;

public class FileInfo
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string ContentType { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifyDate { get; set; }
    
    public ulong Size { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<User> Owners { get; set; }
}