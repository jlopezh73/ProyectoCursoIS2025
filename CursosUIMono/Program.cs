using System.Globalization;
using AppServiciosIdentidad.Gateways.Providers;
using CursosUIMono.DAOs;
using CursosUIMono.DTOs;
using CursosUIMono.Entities;
using CursosUIMono.Services.Implementations;
using CursosUIMono.Services.Interfaces;
using CursosUIMono.Services.Providers;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

CultureInfo culturaMexicana = new CultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentCulture = culturaMexicana;
CultureInfo.DefaultThreadCurrentUICulture = culturaMexicana;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddDbContext<CursosContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("CursosDB")??"";
    options.UseMySQL(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".CursosUIMono.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<CursoDTO>();
builder.Services.AddScoped<ProfesorDTO>();
builder.Services.AddScoped<ParticipanteDTO>();
builder.Services.AddScoped<RespuestaPeticionDTO>();
builder.Services.AddScoped<RespuestaValidacionUsuarioDTO>();
builder.Services.AddScoped<PeticionInicioSesionDTO>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    return new MongoClient(connectionString);
});



builder.Services.AddScoped<CursosDAO>();
builder.Services.AddScoped<ProfesoresDAO>();
builder.Services.AddScoped<ParticipantesDAO>();
builder.Services.AddScoped<CursosImagenesDAO>();
builder.Services.AddScoped<UsuariosDAO>();
builder.Services.AddScoped<UsuarioSesionesDAO>();
builder.Services.AddScoped<UsuarioAccionesDAO>();


builder.Services.AddScoped<ICursosService, CursosService>();
builder.Services.AddScoped<IProfesoresService, ProfesoresService>();
builder.Services.AddScoped<IParticipantesService, ParticipantesService>();
builder.Services.AddScoped<ICursosImagenesService, CursosImagenesFileSystemService>();
builder.Services.AddScoped<IIdentidadService, IdentidadService>();
builder.Services.AddScoped<ISesionesService, SesionesService>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>();
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.UseSession();
app.MapControllers();

app.Run();
