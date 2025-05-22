using System.Globalization;
using ServiciosIdentidad.Entities;
using ServiciosIdentidad.DAOs;
using ServiciosIdentidad.DTOs;
using ServiciosIdentidad.Services.Interfaces;
using ServiciosIdentidad.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

CultureInfo culturaMexicana = new CultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentCulture = culturaMexicana;
CultureInfo.DefaultThreadCurrentUICulture = culturaMexicana;

builder.Services.AddDbContext<CursosContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CursosDB") ?? "";
    options.UseMySQL(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<RespuestaPeticionDTO>();
builder.Services.AddScoped<RespuestaValidacionUsuarioDTO>();
builder.Services.AddScoped<UsuarioDTO>();

builder.Services.AddScoped<UsuariosDAO>();
builder.Services.AddScoped<UsuarioSesionesDAO>();


builder.Services.AddScoped<IIdentidadService, IdentidadService>();
builder.Services.AddScoped<ISesionesService, SesionesService>();
builder.Services.AddScoped<IGeneradorTokensService, GeneradorTokensJWTService>();

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/api/Identidad/usuarios", async (IIdentidadService service) =>
 {
     return await service.ObtenerTodosLosUsuariosAsync();
 })
.WithName("ObtenerTodosLosUsuarios")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General");
    });

app.MapPost("/api/Identidad/validarUsuario", async (IIdentidadService service,
                                                    PeticionInicioSesionDTO peticion,
                                                    HttpContext context) =>
 {
    var ip = context.Connection.RemoteIpAddress;       
    return await service.ValidarUsuario(peticion, ip.ToString());
})
.WithName("ValidarUsuario");

app.MapPost("/api/Identidad/usuario", async (IIdentidadService service,
                                                    UsuarioDTO usuarioDTO) =>
 {
     await service.CrearUsuarioAsync(usuarioDTO);
 })
.WithName("AgregarUsuario")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General");
    });

app.MapPut("/api/Identidad/usuario", async (IIdentidadService service,
                                                    UsuarioDTO usuarioDTO) =>
 {    
    await service.ActualizarUsuarioAsync(usuarioDTO);
})
.WithName("ActualizarUsuario")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General");
    });


app.MapDelete("/api/Identidad/usuario", async (IIdentidadService service,
                                                    int usuarioDTO) =>
 {    
    await service.EliminarUsuarioAsync(usuarioDTO);
})
.WithName("EliminarUsuario")
.RequireAuthorization(auth =>
    {
        auth.RequireRole("Administrador General");
    });


app.Run();
