using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Services;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Db;
using TaskManager.Infrastructure.Repositories;

// Punto de entrada de la API.
// Este archivo configura el pipeline de ASP.NET Core, la inyección de dependencias
// y los servicios de infraestructura necesarios (EF Core, repositorios, CORS, Swagger).
var builder = WebApplication.CreateBuilder(args);

// Configuración de la persistencia: aquí se registra el DbContext de EF Core.
// La cadena de conexión está embebida para simplicidad (SQLite local).
// En producción deberías moverla a `appsettings.json` o variables de entorno.
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

// Registro de dependencias (DI):
// - `ITaskRepository` se resuelve con `TaskRepository` (implementación EF Core).
// - `TaskService` contiene la lógica de aplicación y orquesta llamadas al repositorio.
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<TaskService>();

// MVC / Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: permite llamadas desde el frontend Angular que corre en localhost:4200.
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev",
        policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "http://127.0.0.1:4200"));
});

var app = builder.Build();

// Middlewares mínimos: CORS, Swagger en desarrollo, autorización y mapeo de controllers.
app.UseCors("LocalDev");

if (app.Environment.IsDevelopment())
{
    // Documentación interactiva de la API para desarrollo.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// En este proyecto no hay autenticación configurada, pero se deja el middleware.
app.UseAuthorization();

// Convenciones: los controllers definen las rutas y endpoints REST.
app.MapControllers();

app.Run();



