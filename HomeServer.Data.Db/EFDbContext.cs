using HomeServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using FileInfo = HomeServer.Data.Models.FileInfo;

namespace HomeServer.Data;

public class EFDbContext(DbContextOptions<EFDbContext> options) : DbContext(options)
{
    public DbSet<FileInfo> FileInfos { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileInfo>(e =>
        {
            e.Property(fi => fi.Name).IsRequired();
            e.Property(fi => fi.ContentType).IsRequired();
            e.Property(fi => fi.CreateDate).IsRequired();
            e.Property(fi => fi.ModifyDate).IsRequired(false);
            e.Property(fi => fi.Size).IsRequired();
            e.Property(fi => fi.IsDeleted).HasDefaultValue(false);

            e.HasMany(fi => fi.Owners).WithMany(u => u.Files);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.Property(u => u.Email).IsRequired();
            e.Property(u => u.DisplayName).IsRequired();
            e.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            e.Property(u => u.CreateDate).IsRequired();

            e.HasMany(u => u.Roles)
                .WithMany(r => r.Users);
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.Property(r => r.Name).IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}