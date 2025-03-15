using HomeServer.Models.Configurations;
using HomeServer.Services;

namespace HomeServer.Api.Extensions;

public static class ServicesSettings
{
    public static IServiceCollection AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection("ServerOptions"));
        builder.Services.AddScoped<IFilesService, FilesService>();
        
        return builder.Services;
    }
}