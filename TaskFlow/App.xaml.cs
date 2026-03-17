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
        private IServiceScope? _appScope;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
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

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            _appScope = _host.Services.CreateScope();

            var databaseService = _appScope.ServiceProvider.GetRequiredService<DatabaseService>();
            await databaseService.InitializeAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _appScope.ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _appScope?.Dispose();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
