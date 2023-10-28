using PlantUml.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IPlantUmlRenderer>(sp =>
{
    var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    if (string.IsNullOrWhiteSpace(assemblyPath))
    {
        throw new ApplicationException("Assembly path not found");
    }

    var path = Path.Combine(assemblyPath, "PlantUML", "plantuml.jar");

    return new RendererFactory()
               .CreateRenderer(new PlantUmlSettings
               {
                   JavaPath = path
               });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
