using Microsoft.Extensions.DependencyInjection;
using SVRepository.DB;
using SVRepository.Implementation;
using SVRepository.Interfaces;

namespace SVRepository
{
    public static class DependencyInjection
    {
        public static void RegisterRepositoryDependencies(this IServiceCollection services)
        {
            services.AddSingleton<Conexion>();
            services.AddTransient<IMedidaRepository, MedidaRepository>();
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<IProductoRepository, ProductoRepository>();
            services.AddTransient<INegocioRepository, NegocioRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IRolRepository, RolRepository>();
            services.AddTransient<IVentaRepository, VentaRepository>();
            services.AddTransient<IMenuRolRepository, MenuRolRepository>();
        }
    }
}
