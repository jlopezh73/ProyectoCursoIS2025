using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServiciosProfesores.Entities;

public partial class CursosContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public CursosContext(IConfiguration configuration)
    {
        _configuration = configuration;    
    }

    public CursosContext(DbContextOptions<CursosContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;    
    }

    
    public virtual DbSet<Profesor> Profesors { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL(_configuration.GetConnectionString("CursosDB")??"");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("Profesor", "Cursos2025");

            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.especializacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(100);
            entity.Property(e => e.telefono).HasMaxLength(50);
        });

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
