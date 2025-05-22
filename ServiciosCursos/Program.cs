using System.Globalization;
using ServiciosCursos.Entities;
using ServiciosCursos.DAOs;
using ServiciosCursos.DTOs;
using ServiciosCursos.Services.Interfaces;
using ServiciosCursos.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

CultureInfo culturaMexicana = new CultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentCulture = culturaMexicana;
CultureInfo.DefaultThreadCurrentUICulture = culturaMexicana;

builder.Services.AddDbContext<CursosContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CursosDB") ?? "";
    options.UseMySQL(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<CursoDTO>();
builder.Services.AddScoped<ProfesorDTO>();
builder.Services.AddScoped<RespuestaPeticionDTO>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    return new MongoClient(connectionString);
});



builder.Services.AddScoped<CursosDAO>();
builder.Services.AddScoped<CursosImagenesDAO>();


builder.Services.AddScoped<ICursosService, CursosService>();
builder.Services.AddScoped<IProfesoresService, ProfesoresService>();
builder.Services.AddScoped<ICursosImagenesService, CursosImagenesFileSystemService>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>();


builder.Services.AddAuthentication(options => {    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    
}).AddJwtBearer(options => {
    var config = builder.Configuration;
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    var llave = config["JWTSettings:Key"];
    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidIssuer = config["JWTSettings:Issuer"],
        ValidAudience = config["JWTSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave)),        
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapGet("/api/Cursos", async (ICursosService service) =>
 {
     return await service.ObtenerTodosLosCursosAsync();
 })
.WithName("ObtenerTodosLosCursos")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Cursos");
    });

app.MapGet("/api/Cursos/{id}", async (ICursosService service, int id) =>
 {
     return await service.ObtenerCursoPorIdAsync(id);
 })
.WithName("ObtenerPorIdCurso")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Cursos");
    });


app.MapPost("/api/Cursos", async (ICursosService service,
                                                    CursoDTO cursoDTO) =>
 {
     await service.CrearCursoAsync(cursoDTO);
 })
.WithName("AgregarCurso")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Cursos");
    });

app.MapPut("/api/Cursos/{id}", async (ICursosService service,
                                                    CursoDTO cursoDTO) =>
 {    
    await service.ActualizarCursoAsync(cursoDTO);
})
.WithName("ActualizarCurso")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Cursos");
    });


app.MapDelete("/api/Cursos/{id}", async (ICursosService service,
                                                    int cursoDTO) =>
 {    
    await service.EliminarCursoAsync(cursoDTO);
})
.WithName("EliminarCurso")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Cursos");
    });

app.Run();

