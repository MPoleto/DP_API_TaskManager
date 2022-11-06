using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using dp_api_taskManager.Persistence;
using dp_api_taskManager.Persistence.Repository;
using dp_api_taskManager.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrganizerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ITaskToDoRepository, TaskToDoRepository>();
builder.Services.AddScoped<ITaskToDoService, TaskToDoService>(); 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "Tasks To-Do API",
    Description = "Projeto ASP.NET Core Web API para cadastrar tarefas"
  });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
