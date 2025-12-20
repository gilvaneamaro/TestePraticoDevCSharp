using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestePraticoDevCSharp.App.Interfaces;
using TestePraticoDevCSharp.App.Services;
using TestePraticoDevCSharp.Domain.Repositories;
using TestePraticoDevCSharp.Infrastructure.Data;
using TestePraticoDevCSharp.UI;
using TestePraticoDevCSharp.UI.Navigation;

namespace TestePraticoDevCSharp
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            // ---------- Infra ----------
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IClienteRepository, ClienteRepository>();

            // ---------- Application ----------
            services.AddScoped<ClienteService>();
            services.AddScoped<ProdutoService>();
            services.AddSingleton<FormNavigator>();

            // ---------- UI ----------
            services.AddTransient<FormVenda>();
            services.AddTransient<FormCliente>();
            services.AddTransient<FormDefineCliente>();
            services.AddTransient<FormEstoque>();
            services.AddTransient<FormNovoCliente>();
            services.AddTransient<FormRelatorio>();
            services.AddTransient<Main>();

            services.AddSingleton<IDbConnectionFactory>(
                _ => new DbConnectionFactory());

            var serviceProvider = services.BuildServiceProvider();
            Application.Run(serviceProvider.GetRequiredService<Main>());
            
        }
    }
}
