// En: SVRepository/DependencyInjection.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SVRepository.DB;
using SVRepository.Implementation;
using SVRepository.Interfaces;

namespace SVRepository
{
    /// <summary>
    /// Clase estática que proporciona métodos de extensión para registrar las dependencias del repositorio
    /// en el contenedor de inyección de dependencias.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa el patrón de configuración de dependencias para el módulo de repositorio,
    /// permitiendo la inyección de dependencias de manera centralizada y configurable.
    /// </remarks>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra todas las dependencias del repositorio en el contenedor de servicios.
        /// </summary>
        /// <param name="services">Colección de servicios donde se registrarán las dependencias.</param>
        /// <param name="configuration">Configuración de la aplicación que contiene la cadena de conexión.</param>
        /// <remarks>
        /// Este método registra:
        /// - La clase de conexión a la base de datos como singleton
        /// - Todos los repositorios como servicios transitorios
        /// - Las interfaces y sus implementaciones correspondientes
        /// </remarks>
        /// <example>
        /// <code>
        /// var services = new ServiceCollection();
        /// var configuration = new ConfigurationBuilder()
        ///     .AddJsonFile("appsettings.json")
        ///     .Build();
        /// services.RegisterRepositoryDependencies(configuration);
        /// </code>
        /// </example>
        public static void RegisterRepositoryDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra la conexión a la base de datos como singleton para reutilizar la misma instancia
            services.AddSingleton(new Conexion(configuration));

            // Registra todos los repositorios como servicios transitorios
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<IMedidaRepository, MedidaRepository>();
            services.AddTransient<IMenuRolRepository, MenuRolRepository>();
            services.AddTransient<INegocioRepository, NegocioRepository>();
            services.AddTransient<IProductoRepository, ProductoRepository>();
            services.AddTransient<IRolRepository, RolRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IVentaRepository, VentaRepository>();
        }
    }
}