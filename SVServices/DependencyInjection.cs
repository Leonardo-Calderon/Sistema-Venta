using Microsoft.Extensions.DependencyInjection;
using SVServices.Implementation;
using SVServices.Interfaces;

namespace SVServices
{
    public static class DependencyInjection
    {
        public static void RegisterServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IMedidaService, MedidaService>();
            services.AddTransient<ICategoriaService , CategoriaService>();
            services.AddTransient<IProductoService, ProductoService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<INegocioService, NegocioService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IRolService, RolService>();
            services.AddTransient<ICorreoService, CorreoService>();
            services.AddTransient<IVentaService, VentaService>();
            services.AddTransient<IMenuRolService, MenuRolService>();
        }
    }
}
