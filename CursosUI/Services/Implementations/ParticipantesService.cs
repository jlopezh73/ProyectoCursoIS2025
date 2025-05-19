using System.Collections.Generic;
using System.Threading.Tasks;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;

namespace CursosUI.Services.Implementations
{
    public class ParticipantesService : IParticipantesService
    {
        //private readonly ParticipantesDAO _participantesDAO;

        public ParticipantesService() //ParticipantesDAO participantesDAO)
        {
            //_participantesDAO = participantesDAO;
        }

        public async Task<List<ParticipanteDTO>> ObtenerTodosLosParticipantesAsync()
        {
            /*var participantes = await _participantesDAO.ObtenerTodosAsync();            
            var participantesDTO = participantes.Select(participante => new ParticipanteDTO
            {
                id = participante.id,
                nombre = participante.nombre,
                email = participante.email,
                telefono = participante.telefono                
            }).ToList();            
            return participantesDTO;*/
            return null;
        }

        public async Task<ParticipanteDTO> ObtenerParticipantePorIdAsync(int id)
        {
            //var participante = await _participantesDAO.ObtenerPorIdAsync(id);
            //if (participante == null) return null;

            // Mapear ParticipanteDTO a Participante
            return new ParticipanteDTO() {
                /*id = participante.id,
                nombre = participante.nombre,
                email = participante.email,
                telefono = participante.telefono                */
            };
        }

        public async Task CrearParticipanteAsync(ParticipanteDTO participanteDTO)
        {
            // Mapear ParticipanteDTO a Participante
            var participante = new ParticipanteDTO() {
                id = participanteDTO.id,
                nombre = participanteDTO.nombre,
                email = participanteDTO.email,
                telefono = participanteDTO.telefono                
            };

            //await _participantesDAO.AgregarAsync(participante);

        }

        public async Task ActualizarParticipanteAsync(ParticipanteDTO participanteDTO)
        {
            // Mapear ParticipanteDTO a Participante
            /*var participante = new Participante() {
                id = participanteDTO.id,
                nombre = participanteDTO.nombre,
                email = participanteDTO.email,
                telefono = participanteDTO.telefono                
            };

            await _participantesDAO.ActualizarAsync(participante);*/
        }

        public async Task EliminarParticipanteAsync(int id)
        {
            //await _participantesDAO.EliminarAsync(id);
        }
    }
}