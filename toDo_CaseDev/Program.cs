using TodoApi.Services;
using TodoApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();  // Registro do repositório
builder.Services.AddSingleton<TodoService>();  // Registro do serviço
builder.Services.AddSingleton<TodoService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
