using TaskApi.Services;
using TaskApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "sua_chave_secreta_aqui";

// Configuração de controllers + validação customizada de erros
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

//Serviços para usuário e autenticação
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<AuthService>(sp =>
    new AuthService(sp.GetRequiredService<UserService>(), jwtSecret));

// Configuração do JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(jwtSecret);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Autorização
builder.Services.AddAuthorization();


// Adiciona os serviços necessários ao contêiner de injeção de dependência
// Registro do repositório como Singleton (uma única instância para toda a aplicação)
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();

// Registro do serviço TodoService como Scoped (uma instância por requisição HTTP)
builder.Services.AddScoped<ITaskService, TaskService>(); // Registro do serviço
builder.Services.AddControllers();

// Swagger (documentação)
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware Swagger só em dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Autenticação vem antes da autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
