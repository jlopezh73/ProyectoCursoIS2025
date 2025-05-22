using System;
using System.Collections.Generic;

namespace ServiciosIdentidad.Entities;

public partial class Usuario_Sesion
{
    public int ID { get; set; }

    public int IDUsuario { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaUltimoAcceso { get; set; }

    public string? DireccionIP { get; set; }

    public string? Token { get; set; }

    public virtual Usuario IDUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Usuario_Accion> Usuario_Accions { get; set; } = new List<Usuario_Accion>();
}
