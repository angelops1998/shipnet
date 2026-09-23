using Microsoft.AspNetCore.Identity;
using ShipNetApi.Models;

namespace ShipNetApi.Data;

public static class SeedData
{
    public static async Task InicializarAsync(IServiceProvider services)
    {
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        var usuarios = services.GetRequiredService<UserManager<ApplicationUser>>();
        var db = services.GetRequiredService<ApplicationDbContext>();

        foreach (var rol in new[] { "Admin", "Docente", "Estudiante" })
            if (!await roles.RoleExistsAsync(rol))
                await roles.CreateAsync(new IdentityRole(rol));

        if (await usuarios.FindByEmailAsync("admin@shipnet.com") != null) return; // ya se sembró antes

        await CrearUsuario(usuarios, "admin@shipnet.com", "admin123", "Admin", "Administrador");
        var docente = await CrearUsuario(usuarios, "docente@shipnet.com", "docente123", "Docente", "Juan Perez");
        var estudiante = await CrearUsuario(usuarios, "estudiante@shipnet.com", "est123", "Estudiante", "Ana Lopez");

        db.Docentes.Add(new Docente { Nombre = "Juan", Apellido = "Perez", ApplicationUserId = docente.Id });
        db.Estudiantes.Add(new Estudiante { Nombre = "Ana", Apellido = "Lopez", CI = "1234567", ApplicationUserId = estudiante.Id });
        await db.SaveChangesAsync();
    }

    static async Task<ApplicationUser> CrearUsuario(UserManager<ApplicationUser> usuarios, string email, string password, string rol, string nombre)
    {
        var user = new ApplicationUser { UserName = email, Email = email, NombreCompleto = nombre };
        await usuarios.CreateAsync(user, password);
        await usuarios.AddToRoleAsync(user, rol);
        return user;
    }
}
