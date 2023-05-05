using System;
using System.Collections.Generic;

namespace PruebasSistemaInventario.Models;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public string? Pais { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
