using TodoApi.Services;
using TodoApi.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Personaliza a resposta de erro para requisições inválidas
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

// Adiciona os serviços necessários ao contêiner de injeção de dependência
// Registro do repositório como Singleton (uma única instância para toda a aplicação)
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();

// Registro do serviço TodoService como Scoped (uma instância por requisição HTTP)
builder.Services.AddScoped<ITodoService, TodoService>(); // Registro do serviço
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
