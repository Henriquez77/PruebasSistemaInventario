using System;
using System.Collections.Generic;

namespace PruebasSistemaInventario.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string? Nombre { get; set; }

    public string? Contrasenia { get; set; }

    public int? RolId { get; set; }

    public virtual Role? Rol { get; set; }
}
