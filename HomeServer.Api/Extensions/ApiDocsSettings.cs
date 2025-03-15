namespace HomeServer.Api.Extensions;

public static class ApiDocsSettings
{
    public static IServiceCollection AddApiDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder.Services;
    }
}