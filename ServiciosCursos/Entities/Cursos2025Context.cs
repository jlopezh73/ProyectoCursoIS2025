using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServiciosCursos.Entities;

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
    
    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<CursoImagen> CursoImagens { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL(_configuration.GetConnectionString("CursosDB")??"");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("Curso", "Cursos2025");

            entity.HasIndex(e => e.idProfesor, "idProfesor");

            entity.Property(e => e.descripcion).HasMaxLength(255);
            entity.Property(e => e.fechaInicio).HasColumnType("datetime");
            entity.Property(e => e.fechaTermino).HasColumnType("datetime");
            entity.Property(e => e.nombre).HasMaxLength(100);
            entity.Property(e => e.precio).HasPrecision(10);
            
        });

        modelBuilder.Entity<CursoImagen>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("CursoImagen", "Cursos2025");

            entity.HasIndex(e => e.idCurso, "idCurso");

            entity.Property(e => e.archivo).HasMaxLength(100);

            entity.HasOne(d => d.idCursoNavigation).WithMany(p => p.CursoImagens)
                .HasForeignKey(d => d.idCurso)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("cursoimagen_ibfk_1");
        });
        

        
        
        
        
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
