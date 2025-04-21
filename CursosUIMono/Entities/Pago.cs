using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class Pago
{
    public int id { get; set; }

    public DateTime? fechaPago { get; set; }

    public decimal? monto { get; set; }

    public int? idParticipante { get; set; }

    public int? idCurso { get; set; }

    public virtual Curso? idCursoNavigation { get; set; }

    public virtual Participante? idParticipanteNavigation { get; set; }
}
