using System.Net.Http.Json;
using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class EstudianteService : IEstudianteService
{
    private readonly HttpClient _httpClient;
    public EstudianteService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<EstudianteConAsistenciasDto>> GetEstudiantesAsync()
    {
        try
        {
            var estudiantes = await _httpClient.GetFromJsonAsync<List<EstudianteConAsistenciasDto>>("Estudiantes");
            return estudiantes ?? new List<EstudianteConAsistenciasDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EstudianteService ERROR]: {ex.Message}");
            return new List<EstudianteConAsistenciasDto>();
        }
    }

    public async Task<EstudianteConAsistenciasDto?> GetByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EstudianteConAsistenciasDto>($"Estudiantes/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<MiMateriaDto?> GetMiMateriaAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MiMateriaDto>("Estudiantes/mi-materia");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Exito, string? Mensaje)> InscribirMateriaAsync(int grupoId)
    {
        try
        {
            var res = await _httpClient.PostAsync($"Estudiantes/inscribir/{grupoId}", null);
            var content = await res.Content.ReadAsStringAsync();
            if (res.IsSuccessStatusCode)
            {
                return (true, "Materia inscrita correctamente.");
            }
            return (false, string.IsNullOrWhiteSpace(content) ? "No se pudo inscribir la materia." : content);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Exito, string? Mensaje)> RetirarMateriaAsync(int grupoId)
    {
        try
        {
            var res = await _httpClient.PostAsync($"Estudiantes/retirar/{grupoId}", null);
            var content = await res.Content.ReadAsStringAsync();
            if (res.IsSuccessStatusCode)
            {
                return (true, "Materia retirada correctamente.");
            }
            return (false, string.IsNullOrWhiteSpace(content) ? "No se pudo retirar la materia." : content);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Exito, string? Error)> ActualizarMacAsync(int estudianteId, ActualizarMacEstudianteDto dto)
    {
        var respuesta = await _httpClient.PutAsJsonAsync($"Estudiantes/{estudianteId}/mac", new
        {
            dto.MacAddress,
            dto.Carrera,
            dto.Semestre
        });
        if (respuesta.IsSuccessStatusCode) return (true, null);
        var error = await respuesta.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(error) ? "Error al actualizar datos del estudiante." : error);
    }

    public async Task<bool> AutoVincularMacAsync(string mac)
    {
        try
        {
            var respuesta = await _httpClient.PostAsJsonAsync("Estudiantes/auto-vincular-mac", new { MacAddress = mac });
            return respuesta.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

