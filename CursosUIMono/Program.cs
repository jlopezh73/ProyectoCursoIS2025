using System.Globalization;
using CursosUIMono.DAOs;
using CursosUIMono.DTOs;
using CursosUIMono.Entities;
using CursosUIMono.Services.Implementations;
using CursosUIMono.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

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

builder.Services.AddScoped<CursoDTO>();
builder.Services.AddScoped<ProfesorDTO>();
builder.Services.AddScoped<RespuestaPeticionDTO>();
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<CursosDAO>();
builder.Services.AddScoped<ProfesoresDAO>();
builder.Services.AddScoped<CursosImagenesDAO>();

builder.Services.AddScoped<ICursosService, CursosService>();
builder.Services.AddScoped<IProfesoresService, ProfesoresService>();
builder.Services.AddScoped<ICursosImagenesService, CursosImagenesMongoDBService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

app.Run();
