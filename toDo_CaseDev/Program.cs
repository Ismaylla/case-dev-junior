using TodoApi.Services;
using TodoApi.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Custom error response for invalid entries
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage);

            var customResponse = new
            {
                Status = "Erro",
                Mensagens = errors
            };

            return new BadRequestObjectResult(customResponse);
        };
    });

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
