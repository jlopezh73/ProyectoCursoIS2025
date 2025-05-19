using CursosUI.DTOs;

namespace CursosUI.Services.Interfaces
{
    public interface IParticipantesService
    {
        // Define los métodos que implementará la clase ProfesoresService
        Task<List<ParticipanteDTO>> ObtenerTodosLosParticipantesAsync();
        Task<ParticipanteDTO> ObtenerParticipantePorIdAsync(int id);
        Task CrearParticipanteAsync(ParticipanteDTO p);
        Task ActualizarParticipanteAsync(ParticipanteDTO participanteActualizado);
        Task EliminarParticipanteAsync(int id);
    }
}