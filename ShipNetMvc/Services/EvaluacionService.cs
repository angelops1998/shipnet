using System.Net.Http.Json;
using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class EvaluacionService : IEvaluacionService
{
    private readonly HttpClient _httpClient;

    public EvaluacionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Docente
    public async Task<List<EvaluacionDto>> GetEvaluacionesDocenteAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<EvaluacionDto>>("Evaluaciones/docente");
        return result ?? new List<EvaluacionDto>();
    }

    public async Task<bool> CrearEvaluacionAsync(CrearEvaluacionViewModel vm)
    {
        var response = await _httpClient.PostAsJsonAsync("Evaluaciones", vm);
        return response.IsSuccessStatusCode;
    }

    public async Task<EvaluacionDto?> GetDetalleEvaluacionAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EvaluacionDto>($"Evaluaciones/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> EliminarEvaluacionAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"Evaluaciones/{id}");
        return response.IsSuccessStatusCode;
    }

    // Admin
    public async Task<List<EvaluacionDto>> GetTodasEvaluacionesAsync()
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<List<EvaluacionDto>>("Evaluaciones/todas");
            return res ?? new List<EvaluacionDto>();
        }
        catch
        {
            return new List<EvaluacionDto>();
        }
    }

    // Estudiante
    public async Task<List<EvaluacionDisponibleDto>> GetEvaluacionesDisponiblesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<EvaluacionDisponibleDto>>("Evaluaciones/disponibles");
        return result ?? new List<EvaluacionDisponibleDto>();
    }

    public async Task<(bool Exito, IntentoEvaluacionDto? Intento, string? Error)> IniciarIntentoAsync(int evaluacionId, string? macAddress)
    {
        var response = await _httpClient.PostAsJsonAsync("Evaluaciones/iniciar", new { evaluacionId, macAddress });
        if (response.IsSuccessStatusCode)
        {
            var intento = await response.Content.ReadFromJsonAsync<IntentoEvaluacionDto>();
            return (true, intento, null);
        }

        var error = await response.Content.ReadAsStringAsync();
        return (false, null, string.IsNullOrWhiteSpace(error) ? "No se pudo iniciar la evaluación." : error);
    }

    public async Task<bool> FinalizarIntentoAsync(int intentoId, decimal puntaje)
    {
        var response = await _httpClient.PostAsJsonAsync("Evaluaciones/finalizar", new { intentoId, puntaje });
        return response.IsSuccessStatusCode;
    }
}
