using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskFlow.Database;
using TaskFlow.Services;
using TaskFlow.ViewModels;
using TaskFlow.Views;

namespace TaskFlow
{
    public partial class App : Application
    {
        private readonly IHost _host;
        private IServiceScope _appScope;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(delegate(HostBuilderContext context, IServiceCollection services)
                {
                    services.AddDbContext<TaskFlowDbContext>();
                    services.AddScoped<DatabaseService>();
                    services.AddScoped<TaskService>();
                    services.AddScoped<ProjectService>();
                    services.AddSingleton<ThemeService>();
                    services.AddSingleton<NotificationService>();

                    services.AddSingleton<MainWindow>();
                    services.AddScoped<MainViewModel>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            _appScope = _host.Services.CreateScope();

            DatabaseService databaseService = _appScope.ServiceProvider.GetRequiredService<DatabaseService>();
            databaseService.InitializeAsync().GetAwaiter().GetResult();

            MainWindow mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _appScope.ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync().GetAwaiter().GetResult();

            if (_appScope != null)
            {
                _appScope.Dispose();
            }

            _host.Dispose();
            base.OnExit(e);
        }
    }
}
