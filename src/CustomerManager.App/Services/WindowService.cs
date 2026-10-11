using System.Windows;
using CustomerManager.App.Models;
using CustomerManager.App.ViewModels;
using CustomerManager.App.Views;

namespace CustomerManager.App.Services;

public class WindowService : IWindowService
{
    private readonly ICustomerRepository _repository;

    public WindowService(ICustomerRepository repository) => _repository = repository;

    public void ShowMainWindow()
    {
        var window = new MainWindow { DataContext = new MainViewModel(_repository, this) };
        Application.Current.MainWindow = window;
        window.Show();
    }

    public Customer? ShowAddCustomerDialog()
    {
        var viewModel = new AddCustomerViewModel(_repository);
        var window = new AddCustomerWindow
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };

        Customer? saved = null;
        viewModel.CloseRequested += (_, customer) =>
        {
            saved = customer;
            window.DialogResult = customer is not null;
        };

        window.ShowDialog();
        return saved;
    }
}
