using System.Net.Http.Json;
using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class AsistenciaService : IAsistenciaService
{
    private readonly HttpClient _httpClient;

    public AsistenciaService(HttpClient httpClient) => _httpClient = httpClient;

    // Docente
    public async Task<List<SesionAsistenciaDto>> GetSesionesDocenteAsync()
    {
        var sesiones = await _httpClient.GetFromJsonAsync<List<SesionAsistenciaDto>>("Asistencia/docente/sesiones");
        return sesiones ?? new List<SesionAsistenciaDto>();
    }

    public async Task<List<GrupoDto>> GetGruposDocenteAsync()
    {
        var grupos = await _httpClient.GetFromJsonAsync<List<GrupoDto>>("Asistencia/docente/grupos");
        return grupos ?? new List<GrupoDto>();
    }

    public async Task<bool> AbrirSesionAsync(int grupoId)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Asistencia/abrir", new { grupoId });
        return respuesta.IsSuccessStatusCode;
    }

    public async Task<bool> CerrarSesionAsync(int id)
    {
        var respuesta = await _httpClient.PostAsync($"Asistencia/cerrar/{id}", null);
        return respuesta.IsSuccessStatusCode;
    }

    public async Task<SesionAsistenciaDto?> GetDetalleAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<SesionAsistenciaDto>($"Asistencia/{id}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    // Admin
    public async Task<List<SesionAsistenciaDto>> GetTodasSesionesAsync()
    {
        try
        {
            var sesiones = await _httpClient.GetFromJsonAsync<List<SesionAsistenciaDto>>("Asistencia/todas");
            return sesiones ?? new List<SesionAsistenciaDto>();
        }
        catch
        {
            return new List<SesionAsistenciaDto>();
        }
    }

    // Estudiante
    public async Task<List<SesionAsistenciaDto>> GetSesionesAbiertasAsync()
    {
        var sesiones = await _httpClient.GetFromJsonAsync<List<SesionAsistenciaDto>>("Asistencia/abiertas");
        return sesiones ?? new List<SesionAsistenciaDto>();
    }

    public async Task<(bool Exito, string? Error)> MarcarAsistenciaAsync(int sesionId, string? macAddress)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Asistencia/marcar", new { sesionId, macAddress });
        if (respuesta.IsSuccessStatusCode)
            return (true, null);

        var error = await respuesta.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(error) ? "No se pudo registrar la asistencia." : error);
    }

    public async Task<List<RegistroAsistenciaDto>> GetHistorialEstudianteAsync()
    {
        var registros = await _httpClient.GetFromJsonAsync<List<RegistroAsistenciaDto>>("Asistencia/estudiante/historial");
        return registros ?? new List<RegistroAsistenciaDto>();
    }
}

