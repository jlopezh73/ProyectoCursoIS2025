using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;

namespace CursosUI.Services.Implementations
{
    public class CursosService : ICursosService
    {
        HttpClient _httpClient;        
        private string _token;
        

        public CursosService(IHttpClientFactory httpClientFactory,
                IHttpContextAccessor httpContextAccessor)
        {
            this._httpClient = httpClientFactory.CreateClient("CursosAPI");
            _token = httpContextAccessor.HttpContext?.Session.GetString("token_usuario")?.ToString() ?? string.Empty;
        }

        public async Task<List<CursoDTO>> ObtenerTodosLosCursosAsync()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var response = await _httpClient.GetFromJsonAsync<List<CursoDTO>>("");        
            return response;
        }

        public async Task<CursoDTO> ObtenerCursoPorIdAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var response = await _httpClient.GetFromJsonAsync<CursoDTO>($"{id}");        
            return response;
        }

        public async Task CrearCursoAsync(CursoDTO cursoDTO)
        {
            var response = await _httpClient.PostAsJsonAsync<CursoDTO>($"", cursoDTO);            
            return;
        }

        public async Task ActualizarCursoAsync(CursoDTO cursoDTO)
        {
            var response = await _httpClient.PutAsJsonAsync<CursoDTO>($"{cursoDTO.id}", cursoDTO);            
            return;
        }

        public async Task EliminarCursoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{id}");            
            return;
        }
    }
}