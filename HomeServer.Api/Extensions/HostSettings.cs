using Microsoft.AspNetCore.Http.Features;

namespace HomeServer.Api.Extensions;

public static class HostSettings
{
    public static IServiceCollection SetupHost(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(c =>
        {
            c.Limits.MaxRequestBodySize = long.MaxValue;
        });

        builder.Services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = long.MaxValue; 
        });

        builder.Services.AddControllers();
        builder.Services.AddRouting(opt =>
        {
            opt.LowercaseUrls = true;
        });

        return builder.Services;
    }
}