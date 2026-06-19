using System.Reflection;
using Contracts;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Data;
using Users.Data.Domain;
using Users.Data.Repositories;

namespace Users;

/// <summary>
/// Métodos de extensión para registrar los servicios del módulo de usuarios.
/// </summary>
public static class UsersModuleExtensions
{
    /// <summary>
    /// Registra todos los servicios necesarios para el módulo de usuarios en el contenedor de dependencias.
    /// Configura el contexto de base de datos con PostgreSQL, la identidad de usuarios, el repositorio de usuarios,
    /// los validadores del módulo y el mediador.
    /// </summary>
    /// <param name="services">Colección de servicios donde se registran las dependencias.</param>
    /// <param name="configuration">Configuración de la aplicación, utilizada para obtener la cadena de conexión.</param>
    /// <param name="assemblies">Lista de ensamblados a los que se agrega el ensamblado actual para el descubrimiento de componentes.</param>
    /// <returns>La misma instancia de <see cref="IServiceCollection"/> para permitir encadenamiento.</returns>
    public static IServiceCollection AddUserModuleServices(this IServiceCollection services, ConfigurationManager configuration, List<Assembly> assemblies)
    {
        services.AddModuleValidators(typeof(UsersModuleExtensions).Assembly);

        string? connectionString = configuration.GetConnectionString("UsersConnectionString");

        services.AddDbContext<UserDbContext>(opt =>
        {
            opt
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .UseAsyncSeeding(async (database, isFirstRun, cancellationToken) =>
            {
                var context = (UserDbContext)database;

                if (!await context.AppUsers.AnyAsync(cancellationToken))
                {
                    var user1 = new AppUser
                    {
                        UserName = "jppachar",
                        Email = "jppachar@yopmail.com",
                        FullName = "Juan Pablo Pachar",
                        NormalizedEmail = "JPPACHAR@YOPMAIL.COM",
                        NormalizedUserName = "JPPACHAR",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    };

                    var password = "Jppachar1@";
                    var hasher = new PasswordHasher<AppUser>();

                    user1.PasswordHash = hasher.HashPassword(user1, password);

                    var user2 = new AppUser
                    {
                        UserName = "bdpachar",
                        Email = "bdpachar@yopmail.com",
                        FullName = "Blass Daniel Pachar",
                        NormalizedEmail = "BDPACHAR@YOPMAIL.COM",
                        NormalizedUserName = "BDPACHAR",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    };

                    var password2 = "Bdpachar1@";

                    user2.PasswordHash = hasher.HashPassword(user2, password2);

                    await context.AppUsers.AddRangeAsync([user1, user2], cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }
            });
        });

        services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<UserDbContext>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        assemblies.Add(typeof(UsersModuleExtensions).Assembly);
        services.AddMediator(typeof(UsersModuleExtensions).Assembly);

        return services;
    }
}
