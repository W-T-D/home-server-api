using HomeServer.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.SetupHost();
builder.AddServices();
builder.AddDataLayer();
builder.AddBackgroundJobs();
builder.AddApiDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c =>
{
    var allowedOrigins = builder.Configuration.GetSection("AllowedClients").Value?.Split(',') ?? [];
    c.WithOrigins(allowedOrigins);
    c.WithMethods("GET", "POST", "PUT", "DELETE");
});

app.UseAuthorization();

app.MapControllers();

app.Run();