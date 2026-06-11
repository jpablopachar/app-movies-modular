using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Users.Data.Domain;

namespace Users.Data;

/// <summary>
/// Contexto de datos del módulo de usuarios con configuración de identidad y mapeos del dominio
/// </summary>
/// <param name="options">Opciones de configuración del contexto de base de datos.</param>
public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext(options)
{
    /// <summary>
    /// Conjunto de usuarios de aplicación administrados por el contexto
    /// </summary>
    public DbSet<AppUser> AppUsers { get; set; }

    /// <summary>
    /// Configura el modelo de entidades aplicando esquema por defecto y configuraciones del ensamblado
    /// </summary>
    /// <param name="builder">Constructor del modelo de entidades.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("users");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    /// <summary>
    /// Establece convenciones globales de mapeo para propiedades del modelo
    /// </summary>
    /// <param name="configurationBuilder">Constructor de convenciones del modelo.</param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}
