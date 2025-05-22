using System.Collections.Generic;
using System.Threading.Tasks;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;

namespace CursosUI.Services.Implementations
{
    public class ProfesoresService : IProfesoresService
    {
        HttpClient _httpClient;        
        private string _token;
        

        public ProfesoresService(IHttpClientFactory httpClientFactory,
                IHttpContextAccessor httpContextAccessor) 
        {
            this._httpClient = httpClientFactory.CreateClient("ProfesoresAPI");
            _token = httpContextAccessor.HttpContext?.Session.GetString("token_usuario")?.ToString() ?? string.Empty;
        }

        public async Task<List<ProfesorDTO>> ObtenerTodosLosProfesoresAsync()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var response = await _httpClient.GetFromJsonAsync<List<ProfesorDTO>>("");        
            return response;
        }

        public async Task<ProfesorDTO> ObtenerProfesorPorIdAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var response = await _httpClient.GetFromJsonAsync<ProfesorDTO>($"{id}");        
            return response;
        }

        public async Task CrearProfesorAsync(ProfesorDTO profesorDTO)
        {
            var response = await _httpClient.PostAsJsonAsync<ProfesorDTO>($"", profesorDTO);            
            return;

        }

        public async Task ActualizarProfesorAsync(ProfesorDTO profesorDTO)
        {
            var response = await _httpClient.PutAsJsonAsync<ProfesorDTO>($"{profesorDTO.id}", profesorDTO);            
            return;
        }

        public async Task EliminarProfesorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{id}");            
            return;
        }
    }
}