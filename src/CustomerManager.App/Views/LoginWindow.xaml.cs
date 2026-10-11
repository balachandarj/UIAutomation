using System.Windows;
using CustomerManager.App.Services;
using CustomerManager.App.ViewModels;

namespace CustomerManager.App.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginWindow(LoginViewModel viewModel, IWindowService windowService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;

        viewModel.LoginSucceeded += (_, _) =>
        {
            // Open the main window before closing this one so the app doesn't shut down in between.
            windowService.ShowMainWindow();
            Close();
        };
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e) =>
        _viewModel.Password = PasswordInput.Password;
}
