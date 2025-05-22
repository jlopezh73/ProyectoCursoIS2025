using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServiciosIdentidad.Entities;

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

    
    public virtual DbSet<Usuario> Usuarios { get; set; }    

    public virtual DbSet<Usuario_Sesion> Usuario_Sesions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL(_configuration.GetConnectionString("CursosDB")??"");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity.ToTable("Usuario", "Cursos2025");

            entity.Property(e => e.Activo).HasColumnType("bit(1)");
            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.Materno).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Paterno).HasMaxLength(50);
            entity.Property(e => e.Puesto).HasMaxLength(50);
        });

        
        modelBuilder.Entity<Usuario_Sesion>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity.ToTable("Usuario_Sesion", "Cursos2025");

            entity.HasIndex(e => e.IDUsuario, "fk_UsuarioSesion_Usuario");

            entity.Property(e => e.DireccionIP).HasMaxLength(20);
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaUltimoAcceso).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(600);

            entity.HasOne(d => d.IDUsuarioNavigation).WithMany(p => p.Usuario_Sesions)
                .HasForeignKey(d => d.IDUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UsuarioSesion_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
