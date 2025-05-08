using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class Usuario
{
    public int ID { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Paterno { get; set; }

    public string? Materno { get; set; }

    public string? Nombre { get; set; }

    public string? Puesto { get; set; }

    public string? Password { get; set; }

    public ulong? Activo { get; set; }

    public virtual ICollection<Usuario_Accion> Usuario_Accions { get; set; } = new List<Usuario_Accion>();

    public virtual ICollection<Usuario_Sesion> Usuario_Sesions { get; set; } = new List<Usuario_Sesion>();
}
