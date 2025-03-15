using HomeServer.Data;
using HomeServer.Data.Files;
using HomeServer.Data.Files.Providers;
using Microsoft.EntityFrameworkCore;

namespace HomeServer.Api.Extensions;

public static class DataLayerSettings
{
    public static IServiceCollection AddDataLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileProvider, LocalFileProvider>();
        builder.Services.AddDbContext<EFDbContext>(b => b.UseNpgsql(""));

        
        return builder.Services;
    }
}