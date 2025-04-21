using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CursosUIMono.Entities;

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

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<CursoImagen> CursoImagens { get; set; }

    public virtual DbSet<CursoParticipante> CursoParticipantes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Participante> Participantes { get; set; }

    public virtual DbSet<Profesor> Profesors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL(_configuration.GetConnectionString("CursosDB")??"");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("Administrador", "Cursos2025");

            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.nombre).HasMaxLength(100);
            entity.Property(e => e.rol).HasMaxLength(20);
            entity.Property(e => e.telefono).HasMaxLength(50);
        });

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

            entity.HasOne(d => d.idProfesorNavigation).WithMany(p => p.Cursos)
                .HasForeignKey(d => d.idProfesor)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("curso_ibfk_1");
        });

        modelBuilder.Entity<CursoImagen>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("CursoImagen", "Cursos2025");

            entity.HasIndex(e => e.idCurso, "idCurso");

            entity.Property(e => e.archivo).HasMaxLength(100);

            entity.Property(e => e.contenido)
                .HasColumnType("longblob")
                .HasColumnName("contenido");

            entity.HasOne(d => d.idCursoNavigation).WithMany(p => p.CursoImagens)
                .HasForeignKey(d => d.idCurso)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("cursoimagen_ibfk_1");
        });

        modelBuilder.Entity<CursoParticipante>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("CursoParticipante", "Cursos2025");

            entity.HasIndex(e => e.idCurso, "idCurso");

            entity.HasIndex(e => e.idParticipante, "idParticipante");

            entity.Property(e => e.saldo).HasPrecision(10);

            entity.HasOne(d => d.idCursoNavigation).WithMany(p => p.CursoParticipantes)
                .HasForeignKey(d => d.idCurso)
                .HasConstraintName("cursoparticipante_ibfk_1");

            entity.HasOne(d => d.idParticipanteNavigation).WithMany(p => p.CursoParticipantes)
                .HasForeignKey(d => d.idParticipante)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("cursoparticipante_ibfk_2");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("Pago", "Cursos2025");

            entity.HasIndex(e => e.idCurso, "idCurso");

            entity.HasIndex(e => e.idParticipante, "idParticipante");

            entity.Property(e => e.fechaPago).HasColumnType("datetime");
            entity.Property(e => e.monto).HasPrecision(10);

            entity.HasOne(d => d.idCursoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.idCurso)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pago_ibfk_2");

            entity.HasOne(d => d.idParticipanteNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.idParticipante)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pago_ibfk_1");
        });

        modelBuilder.Entity<Participante>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("Participante", "Cursos2025");

            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.nombre).HasMaxLength(100);
            entity.Property(e => e.telefono).HasMaxLength(50);
        });

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
