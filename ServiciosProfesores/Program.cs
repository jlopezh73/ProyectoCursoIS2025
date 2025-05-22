using System.Globalization;
using ServiciosProfesores.DAOs;
using ServiciosProfesores.DTOs;
using ServiciosProfesores.Entities;
using ServiciosProfesores.Services.Implementations;
using ServiciosProfesores.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

CultureInfo culturaMexicana = new CultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentCulture = culturaMexicana;
CultureInfo.DefaultThreadCurrentUICulture = culturaMexicana;


builder.Services.AddDbContext<CursosContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("CursosDB")??"";
    options.UseMySQL(connectionString);
}, ServiceLifetime.Singleton);

builder.Services.AddSingleton<ProfesorDTO>();
builder.Services.AddSingleton<RespuestaPeticionDTO>();

builder.Services.AddSingleton<ProfesoresDAO>();

builder.Services.AddHostedService<ProfesoresBackgroundService>();



builder.Services.AddSingleton<IProfesoresService, ProfesoresService>();
builder.Services.AddSingleton<IBitacoraService, BitacoraService>();

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

app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/Profesores", async (IProfesoresService service) =>
 {
     return await service.ObtenerTodosLosProfesoresAsync();
 })
.WithName("ObtenerTodosLosProfesores")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Profesores");
    });

app.MapGet("/api/Profesores/{id}", async (IProfesoresService service, int id) =>
 {
     return await service.ObtenerProfesorPorIdAsync(id);
 })
.WithName("ObtenerPorIdProfesor")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Profesores");
    });


app.MapPost("/api/Profesores", async (IProfesoresService service,
                                                    ProfesorDTO profesorDTO) =>
 {
     await service.CrearProfesorAsync(profesorDTO);
 })
.WithName("AgregaProfesor")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Profesores");
    });

app.MapPut("/api/Profesores/{id}", async (IProfesoresService service,
                                                    ProfesorDTO profesorDTO) =>
 {    
    await service.ActualizarProfesorAsync(profesorDTO);
})
.WithName("ActualizarProfesor")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Profesores");
    });


app.MapDelete("/api/Profesores/{id}", async (IProfesoresService service,
                                                    int profesorDTO) =>
 {    
    await service.EliminarProfesorAsync(profesorDTO);
})
.WithName("EliminarProfesor")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General", "Administrador de Profesores");
    });

app.Run();

