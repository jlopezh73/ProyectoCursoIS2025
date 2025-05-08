using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class UsuarioAccionDTO
{
    public int ID { get; set; }

    public int? IDUsuario { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? Accion { get; set; }

    public int? IDUsuarioSesion { get; set; }
    
}
