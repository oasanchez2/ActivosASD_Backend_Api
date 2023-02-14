using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrupoASD.GestionActivos.Api.Models
{
    public partial class ActivosASDContext : DbContext
    {
        public ActivosASDContext()
        {
        }

        public ActivosASDContext(DbContextOptions<ActivosASDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activos> Activos { get; set; }
        public virtual DbSet<EstadosActivos> EstadosActivos { get; set; }
        public virtual DbSet<LogsError> LogsError { get; set; }
        public virtual DbSet<TipoActivo> TipoActivo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activos>(entity =>
            {
                entity.HasKey(e => e.IdActivo);

                entity.HasIndex(e => e.IdEstadoActual);

                entity.HasIndex(e => e.IdTipoActivo);

                entity.Property(e => e.Alto).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Ancho).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FechaBaja).HasColumnType("datetime");

                entity.Property(e => e.FechaCompra).HasColumnType("datetime");

                entity.Property(e => e.Largo).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroInternoInventario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Peso).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Serial)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorCompra).HasColumnType("money");

                entity.HasOne(d => d.IdEstadoActualNavigation)
                    .WithMany(p => p.Activos)
                    .HasForeignKey(d => d.IdEstadoActual)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activos_EstadosActivos");

                entity.HasOne(d => d.IdTipoActivoNavigation)
                    .WithMany(p => p.Activos)
                    .HasForeignKey(d => d.IdTipoActivo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activos_TipoActivo");
            });

            modelBuilder.Entity<EstadosActivos>(entity =>
            {
                entity.HasKey(e => e.IdEstado);

                entity.Property(e => e.NombreEstado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogsError>(entity =>
            {
                entity.HasKey(e => e.IdLogError);

                entity.Property(e => e.DayError).HasColumnType("datetime");

                entity.Property(e => e.InnerException).IsUnicode(false);

                entity.Property(e => e.MessageError)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.MethodError)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OrignError)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.StackTrace).IsUnicode(false);
            });

            modelBuilder.Entity<TipoActivo>(entity =>
            {
                entity.HasKey(e => e.IdTipoActivo);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
