namespace HomeServer.Data.Models;

public class User
{
    public string Id { get; set; }
    
    public string Email { get; set; }

    public string DisplayName { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Role> Roles { get; set; }

    public ICollection<FileInfo> Files { get; set; }
}