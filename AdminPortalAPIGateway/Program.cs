using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddPolicy("ZensarCorsPolicy", p => {
        p.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});
builder.Services.AddOcelot(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ZensarCorsPolicy");

app.UseOcelot();

app.UseStaticFiles();

app.Run();
