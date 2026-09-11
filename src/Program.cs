using FCG.CatalogAPI.Application.Consumers;
using FCG.CatalogAPI.Domain.Interfaces;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Application.Service;
using FCG.CatalogAPI.Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Prometheus;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();


// Configuração do mongo Db
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDbConnection") ?? "mongodb://localhost:27017";
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    // Nome do banco que definimos anteriormente para o catálogo
    return client.GetDatabase("FCGCatalogDB");
});

string key = builder.Configuration["Jwt:Key"] ?? "";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => { policy.RequireRole("Administrador"); });
    options.AddPolicy("Usuario", policy => { policy.RequireRole("Usuario"); });
    options.AddPolicy("AdministradorOuUsuario", policy => { policy.RequireRole("Administrador", "Usuario"); });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Api de Catálogos", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Digite 'Bearer {seu_token}' para autenticação"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddControllers();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMassTransit(busRegistration =>
{
    busRegistration.AddConsumer<PaymentProcessedConsumer>();

    busRegistration.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = builder.Configuration["RabbitMq:Host"] ?? "localhost";

        cfg.Host(rabbitHost, "/", hostConfigurator =>
        {
            hostConfigurator.Username("guest");
            hostConfigurator.Password("guest");
        });

        cfg.UseRawJsonSerializer();

        cfg.ReceiveEndpoint("catalog-payment-processed", e =>
        {
            e.ConfigureConsumer<PaymentProcessedConsumer>(context);
        });
    });
});

var app = builder.Build();

// 1. Roteamento base
app.UseRouting();

// 2. Métricas HTTP do Prometheus
app.UseHttpMetrics();

// Nota: Bloco de migrations do Entity Framework removido, 
// pois o MongoDB gerencia coleções dinamicamente.

// 3. Swagger e Segurança
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 4. Endpoints e Métricas finais
app.MapControllers();
app.MapMetrics(); // Expõe o /metrics

app.Run();