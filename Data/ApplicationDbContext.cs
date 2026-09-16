using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using shipnet.Models;

namespace shipnet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Docente> Docentes => Set<Docente>();
        public DbSet<Materia> Materias => Set<Materia>();
        public DbSet<Grupo> Grupos => Set<Grupo>();
        public DbSet<Equipo> Equipos => Set<Equipo>();
        public DbSet<SesionAsistencia> SesionesAsistencia => Set<SesionAsistencia>();
        public DbSet<RegistroAsistencia> RegistrosAsistencia => Set<RegistroAsistencia>();
        public DbSet<Evaluacion> Evaluaciones => Set<Evaluacion>();
        public DbSet<IntentoEvaluacion> IntentosEvaluacion => Set<IntentoEvaluacion>();
    }
}