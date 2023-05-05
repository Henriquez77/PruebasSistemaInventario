using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PruebasSistemaInventario.Models;

public partial class InventarioContext : DbContext
{
    public InventarioContext()
    {
    }

    public InventarioContext(DbContextOptions<InventarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }
    

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=DESKTOP-VI57TV8\\HWJHONATAN;Database=Inventario;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.CompraId).HasName("PK__Compras__067DA745BAAFD476");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(16, 2)");

            entity.HasOne(d => d.Producto).WithMany(p => p.Compras)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK__Compras__Product__30F848ED");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Compras)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK__Compras__Proveed__31EC6D26");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AEA3C0CAB8B0");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(16, 2)");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__61266A59F3B09DF5");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302F124AC9BF1");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B86B09BB39");

            entity.Property(e => e.Contrasenia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RolId).HasDefaultValueSql("((2))");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK__Usuarios__RolId__276EDEB3");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.VentaId).HasName("PK__Ventas__5B4150AC928BBE1D");

            entity.Property(e => e.Cliente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(16, 2)");

            entity.HasOne(d => d.Producto).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK__Ventas__Total__2C3393D0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
