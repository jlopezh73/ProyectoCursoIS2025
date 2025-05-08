using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class Usuario_Accion
{
    public int ID { get; set; }

    public int? IDUsuario { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? Accion { get; set; }

    public int? IDUsuarioSesion { get; set; }

    public virtual Usuario? IDUsuarioNavigation { get; set; }

    public virtual Usuario_Sesion? IDUsuarioSesionNavigation { get; set; }
}
