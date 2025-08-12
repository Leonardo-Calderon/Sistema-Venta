// En: SVServices/DependencyInjection.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SVServices.Implementation;
using SVServices.Interfaces;

namespace SVServices
{
    /// <summary>
    /// Clase estática que proporciona métodos de extensión para registrar las dependencias de servicios
    /// en el contenedor de inyección de dependencias.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa el patrón de configuración de dependencias para el módulo de servicios,
    /// permitiendo la inyección de dependencias de manera centralizada y configurable.
    /// Los servicios incluyen funcionalidades de negocio, seguridad, validación y utilidades.
    /// </remarks>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra todas las dependencias de servicios en el contenedor de servicios.
        /// </summary>
        /// <param name="services">Colección de servicios donde se registrarán las dependencias.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        /// <remarks>
        /// Este método registra todos los servicios de la capa de servicios como servicios transitorios:
        /// - Servicios de negocio (categorías, productos, usuarios, ventas, etc.)
        /// - Servicios de seguridad (auditoría, validación, pruebas de seguridad)
        /// - Servicios de utilidades (correo, Cloudinary, etc.)
        /// </remarks>
        /// <example>
        /// <code>
        /// var services = new ServiceCollection();
        /// var configuration = new ConfigurationBuilder()
        ///     .AddJsonFile("appsettings.json")
        ///     .Build();
        /// services.RegisterServiceDependencies(configuration);
        /// </code>
        /// </example>
        public static void RegisterServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra todos los servicios de negocio
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<ICorreoService, CorreoService>();
            services.AddTransient<IMedidaService, MedidaService>();
            services.AddTransient<IMenuRolService, MenuRolService>();
            services.AddTransient<INegocioService, NegocioService>();
            services.AddTransient<IProductoService, ProductoService>();
            services.AddTransient<IRolService, RolService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IVentaService, VentaService>();
            
            // Registra todos los servicios de seguridad y auditoría
            services.AddTransient<IAuditoriaService, AuditoriaService>();
            services.AddTransient<IValidacionService, ValidacionService>();
            services.AddTransient<ISeguridadSQLService, SeguridadSQLService>();
            services.AddTransient<IPruebasSeguridadService, PruebasSeguridadService>();
        }
    }
}