using System;
using System.Collections.Generic;

namespace PruebasSistemaInventario.Models;

public partial class Venta
{
    public int VentaId { get; set; }

    public string? Titulo { get; set; }

    public string? Cliente { get; set; }

    public DateTime? Fecha { get; set; }

    public int? ProductoId { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Total { get; set; }

    public virtual Producto? Producto { get; set; }
}
