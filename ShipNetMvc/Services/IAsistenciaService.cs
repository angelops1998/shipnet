using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IAsistenciaService
{
    // Docente
    Task<List<SesionAsistenciaDto>> GetSesionesDocenteAsync();
    Task<List<GrupoDto>> GetGruposDocenteAsync();
    Task<bool> AbrirSesionAsync(int grupoId);
    Task<bool> CerrarSesionAsync(int id);
    Task<SesionAsistenciaDto?> GetDetalleAsync(int id);

    // Admin
    Task<List<SesionAsistenciaDto>> GetTodasSesionesAsync();

    // Estudiante
    Task<List<SesionAsistenciaDto>> GetSesionesAbiertasAsync();
    Task<(bool Exito, string? Error)> MarcarAsistenciaAsync(int sesionId, string? macAddress);
    Task<List<RegistroAsistenciaDto>> GetHistorialEstudianteAsync();
}
