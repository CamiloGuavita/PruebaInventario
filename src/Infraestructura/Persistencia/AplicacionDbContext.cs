using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;

namespace Infraestructura.Persistencia
{
    public class AplicacionDbContext : DbContext
    {
        public AplicacionDbContext() { }

        public AplicacionDbContext(DbContextOptions<AplicacionDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Bodega> Bodegas { get; set; }
        public virtual DbSet<Movimientos> Movimientos { get; set; }
        public virtual DbSet<Inventario> Inventarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-DRFQQF8\\SQLEXPRESS_2019;Database=pruebaInventarioCG;Uid=prueba;Pwd=prueba;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(t => t.Nombre).HasMaxLength(250).IsRequired();
                entity.Property(t => t.Descripcion).HasMaxLength(500);
                entity.Property(t => t.sku).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Bodega>(entity => {
                entity.Property(t => t.Nombre).HasMaxLength(250).IsRequired();
                entity.Property(t => t.Descripcion).HasMaxLength(500);
                entity.Property(t => t.Ubicacion).HasMaxLength(100).IsRequired();
                entity.Property(t => t.CapacidadMaxima).IsRequired();
            });

            modelBuilder.Entity<Movimientos>(entity => {
                entity.Property(t => t.Cantidad).IsRequired();
                entity.Property(t => t.Tipo).IsRequired();
                entity.Property(t => t.FechaMovimiento).IsRequired();
                entity.Property(t => t.Valor).HasPrecision(18, 2).IsRequired();
                entity.HasOne(d => d.Bodega)
                       .WithMany(p => p.Movimientos)
                       .HasForeignKey(d => d.BodegaId)
                       .HasConstraintName("FK_Movimiento_Bodega");
                entity.HasOne(d => d.Producto)
                        .WithMany(p => p.Movimientos)
                        .HasForeignKey(d => d.ProductoId)
                        .HasConstraintName("FK_Movimientos_Producto");
            });

            modelBuilder.Entity<Inventario>(entity => {
                entity.Property(t => t.Cantidad).IsRequired();
                entity.Property(t => t.ValorAcumulado).HasPrecision(18, 2).IsRequired();
                entity.Property(t => t.Creacion).IsRequired();
                entity.HasOne(d => d.Bodega)
                        .WithMany(p => p.Inventario)
                        .HasForeignKey(d => d.BodegaId)
                        .HasConstraintName("FK_Inventario_Bodega");
                entity.HasOne(d => d.Producto)
                        .WithMany(p => p.Inventario)
                        .HasForeignKey(d => d.ProductoId)
                        .HasConstraintName("FK_Inventario_Producto");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
