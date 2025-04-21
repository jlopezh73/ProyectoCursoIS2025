using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class Administrador
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public string? email { get; set; }

    public string? telefono { get; set; }

    public string? rol { get; set; }
}
