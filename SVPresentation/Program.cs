using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SVPresentation.Forms;
using SVRepository;
using SVServices;


namespace SVPresentation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var host = CreateHostBuilder().Build();
            var formservices = host.Services.GetRequiredService<FrmLogin>();
            Application.Run(formservices);
        }
        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder().ConfigureAppConfiguration((context, config) => { 
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            }).ConfigureServices((context, services) => 
            { 
                services.RegisterRepositoryDependencies();
                services.RegisterServiceDependencies();

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