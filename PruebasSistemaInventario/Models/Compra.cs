using System;
using System.Collections.Generic;

namespace PruebasSistemaInventario.Models;

public partial class Compra
{
    public int CompraId { get; set; }

    public string? Titulo { get; set; }

    public DateTime? Fecha { get; set; }

    public int? ProductoId { get; set; }

    public int? Cantidad { get; set; }

    public int? ProveedorId { get; set; }

    public decimal? Total { get; set; }

    public virtual Producto? Producto { get; set; }

    public virtual Proveedore? Proveedor { get; set; }
}
