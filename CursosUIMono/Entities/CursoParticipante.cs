using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class CursoParticipante
{
    public int id { get; set; }

    public int? idCurso { get; set; }

    public int? idParticipante { get; set; }

    public decimal? saldo { get; set; }

    public virtual Curso? idCursoNavigation { get; set; }

    public virtual Participante? idParticipanteNavigation { get; set; }
}
