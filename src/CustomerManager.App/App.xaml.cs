using System.Windows;
using CustomerManager.App.Services;
using CustomerManager.App.ViewModels;
using CustomerManager.App.Views;

namespace CustomerManager.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // UI tests launch the app with --reset-data so every scenario starts from the same seed customers.
        var resetData = e.Args.Contains("--reset-data", StringComparer.OrdinalIgnoreCase);
        var repository = new JsonCustomerRepository(JsonCustomerRepository.DefaultFilePath(), resetData);
        var windowService = new WindowService(repository);

        var loginWindow = new LoginWindow(new LoginViewModel(new AuthService()), windowService);
        loginWindow.Show();
    }
}
