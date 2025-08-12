using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SVPresentation.Forms;
using SVRepository;
using SVServices;

namespace SVPresentation
{
    /// <summary>
    /// Clase principal que contiene el punto de entrada de la aplicación Windows Forms.
    /// </summary>
    /// <remarks>
    /// Esta clase configura el host de la aplicación, registra las dependencias necesarias
    /// y inicia la aplicación Windows Forms con el formulario de login como punto de entrada.
    /// Utiliza Microsoft.Extensions.Hosting para la configuración y gestión del ciclo de vida
    /// de la aplicación.
    /// </remarks>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// </summary>
        /// <remarks>
        /// Este método configura la aplicación Windows Forms, construye el host de la aplicación
        /// con todas las dependencias registradas, obtiene el formulario de login desde el
        /// contenedor de servicios y ejecuta la aplicación.
        /// </remarks>
        [STAThread]
        static void Main()
        {
            // Configurar la aplicación Windows Forms
            ApplicationConfiguration.Initialize();
            
            // Construir el host de la aplicación con todas las dependencias
            var host = CreateHostBuilder().Build();
            
            // Obtener el formulario de login desde el contenedor de servicios
            var formLogin = host.Services.GetRequiredService<FrmLogin>();
            
            // Ejecutar la aplicación con el formulario de login
            Application.Run(formLogin);
        }

        /// <summary>
        /// Crea y configura el host de la aplicación con todas las dependencias necesarias.
        /// </summary>
        /// <returns>Un IHostBuilder configurado con todas las dependencias de la aplicación.</returns>
        /// <remarks>
        /// Este método configura:
        /// - La configuración de la aplicación desde appsettings.json
        /// - Las dependencias de repositorios y servicios
        /// - Todos los formularios de la aplicación como servicios transitorios
        /// </remarks>
        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) => 
                { 
                    // Agregar archivo de configuración JSON
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) => 
                {
                    // Registrar dependencias de repositorios y servicios
                    services.RegisterRepositoryDependencies(context.Configuration);
                    services.RegisterServiceDependencies(context.Configuration);

                    // Registrar todos los formularios de la aplicación
                    services.AddTransient<FrmCategoria>();
                    services.AddTransient<FrmProducto>();
                    services.AddTransient<FrmNegocio>();
                    services.AddTransient<FrmUsuario>();
                    services.AddTransient<FrmVenta>();
                    services.AddTransient<FrmBuscarProducto>();
                    services.AddTransient<FrmHistorial>();
                    services.AddTransient<FrmDetalleVenta>();
                    services.AddTransient<FrmReporteVenta>();
                    services.AddTransient<FrmLogin>();
                    services.AddTransient<FrmActualizarClave>();
                    services.AddTransient<Layout>();
                });
    }
}