using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(builder);
builder.Configuration.AddJsonFile(Path.Combine("configuration","configuration.json"));
builder.Services.AddOcelot();

var app = builder.Build();
app.UseOcelot();

app.Run();
