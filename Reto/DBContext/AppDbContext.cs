using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Reto.Models;

namespace Reto.DBContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Maestro> Maestros { get; set; }

    public virtual DbSet<Materium> Materia { get; set; }

    public virtual DbSet<MatriculaCurso> MatriculaCursos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:retosophos.database.windows.net,1433;Initial Catalog=InstitucionEducativa;Persist Security Info=False;User ID=ramontero;Password=Taqwerty092;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC270710AC46");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.Nrc).HasName("PK__Cursos__C7DEEA5BDF49D1D9");

            entity.Property(e => e.Nrc).ValueGeneratedNever();


        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.CodigoEstudiantil).HasName("PK__Estudian__3798AAC9E043F318");

            entity.Property(e => e.CodigoEstudiantil).ValueGeneratedNever();
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Login__A9D105352993B61D");
        });

        modelBuilder.Entity<Maestro>(entity =>
        {
            entity.HasKey(e => e.CodigoMaestro).HasName("PK__Maestro__E32F236C448E86FA");

            entity.Property(e => e.CodigoMaestro).ValueGeneratedNever();
        });

        modelBuilder.Entity<Materium>(entity =>
        {
            entity.HasKey(e => e.CodigoMateria).HasName("PK__Materia__AD5AB9777F071B6F");

            entity.Property(e => e.CodigoMateria).ValueGeneratedNever();

        });

        modelBuilder.Entity<MatriculaCurso>(entity =>
        {

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
