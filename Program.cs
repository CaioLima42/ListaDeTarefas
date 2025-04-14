using listaTarefas;
using listaTarefas.Repositirios.Interface;
using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // <-- necessário pro Swagger funcionar
builder.Services.AddSwaggerGen();           // <-- adiciona a UI do Swagger

builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<SistemaTarefasDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<ITarefaRepositorio, TarefaRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                        // <-- gera o Swagger JSON
    app.UseSwaggerUI();                      // <-- exibe a interface do Swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();