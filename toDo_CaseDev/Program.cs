using TodoApi.Services;
using TodoApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITodoService, TodoService>(); // Registro do serviço
builder.Services.AddControllers();

// Swagger (documentação)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todo API",
        Version = "v1"
    });

    // Adiciona configuração de segurança para o Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, // O token será passado no cabeçalho
        Name = "Authorization", // O nome do cabeçalho
        Type = SecuritySchemeType.ApiKey, // Tipo de autenticação
        Scheme = "Bearer", // O prefixo que usamos no cabeçalho (Bearer)
        BearerFormat = "JWT", // Formato do token
        Description = "Informe o token JWT com o prefixo 'Bearer' no cabeçalho"
    });

    // Configuração de segurança para aplicar a autenticação nas rotas
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Middleware Swagger só em dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autenticação vem antes da autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
