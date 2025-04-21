using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class Participante
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public string? email { get; set; }

    public string? telefono { get; set; }

    public virtual ICollection<CursoParticipante> CursoParticipantes { get; set; } = new List<CursoParticipante>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
