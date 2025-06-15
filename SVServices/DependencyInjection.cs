// En: SVServices/DependencyInjection.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SVServices.Implementation;
using SVServices.Interfaces;

namespace SVServices
{
    public static class DependencyInjection
    {
        public static void RegisterServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra todos tus servicios
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
        }
    }
}