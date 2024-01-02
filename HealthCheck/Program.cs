using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("TestConnection"));
builder.Services.AddHealthChecks()
 .AddSqlServer(
 connectionString: builder.Configuration.GetConnectionString("TestConnection"),
 healthQuery: "SELECT 1;",
 name: "sql",
 tags: new string[] { "db", "sql", "sqlserver" })
 .AddSqlServer(
 connectionString: builder.Configuration.GetConnectionString("TestConnection1"),
 healthQuery: "SELECT 1;",
 name: "sql2",
 tags: new string[] { "db", "sql", "sqlserver" }
 )
 .AddSqlServer(
 connectionString: builder.Configuration.GetConnectionString("TestConnection2"),
 healthQuery: "SELECT 1;",
 name: "sql3",
 tags: new string[] { "db", "sql", "sqlserver" }
 );
//builder.Services.AddHealthChecks();
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/healthcheck", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}