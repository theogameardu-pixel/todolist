using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskFlow.Database;
using TaskFlow.Services;
using TaskFlow.ViewModels;
using TaskFlow.Views;

namespace TaskFlow;

public partial class App : Application
{
    private readonly IHost _host;
 codex/generate-windows-to-do-list-application-iic5ro
    private IServiceScope? _appScope;

 main

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddDbContext<TaskFlowDbContext>();
 codex/generate-windows-to-do-list-application-iic5ro
                services.AddScoped<DatabaseService>();

                services.AddSingleton<DatabaseService>();
 main
                services.AddScoped<TaskService>();
                services.AddScoped<ProjectService>();
                services.AddSingleton<ThemeService>();
                services.AddSingleton<NotificationService>();

                services.AddSingleton<MainWindow>();
 codex/generate-windows-to-do-list-application-iic5ro
                services.AddScoped<MainViewModel>();

                services.AddSingleton<MainViewModel>();
 main
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

 codex/generate-windows-to-do-list-application-iic5ro
        _appScope = _host.Services.CreateScope();

        var databaseService = _appScope.ServiceProvider.GetRequiredService<DatabaseService>();
        await databaseService.InitializeAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _appScope.ServiceProvider.GetRequiredService<MainViewModel>();

        var databaseService = _host.Services.GetRequiredService<DatabaseService>();
        await databaseService.InitializeAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
 main
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
 codex/generate-windows-to-do-list-application-iic5ro
        _appScope?.Dispose();

 main
        _host.Dispose();
        base.OnExit(e);
    }
}
