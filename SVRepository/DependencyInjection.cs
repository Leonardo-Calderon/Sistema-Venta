// En: SVRepository/DependencyInjection.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SVRepository.DB;
using SVRepository.Implementation;
using SVRepository.Interfaces;

namespace SVRepository
{
    public static class DependencyInjection
    {
        public static void RegisterRepositoryDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // --- ESTA ES LA LÍNEA QUE CAMBIA ---
            // En lugar de leer la cadena aquí y pasarla, pasamos el objeto de configuración directamente.
            services.AddSingleton(new Conexion(configuration));

            // Registra todos tus repositorios (esto se queda igual)
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<IMedidaRepository, MedidaRepository>();
            services.AddTransient<IMenuRolRepository, MenuRolRepository>();
            // ... y los demás repositorios
            services.AddTransient<INegocioRepository, NegocioRepository>();
            services.AddTransient<IProductoRepository, ProductoRepository>();
            services.AddTransient<IRolRepository, RolRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IVentaRepository, VentaRepository>();
        }
    }
}