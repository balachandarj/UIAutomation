using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CustomerManager.App.Models;
using CustomerManager.App.Services;

namespace CustomerManager.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IWindowService _windowService;
    private string _statusMessage = "Ready";

    public MainViewModel(ICustomerRepository repository, IWindowService windowService)
    {
        _windowService = windowService;
        Customers = new ObservableCollection<Customer>(repository.GetAll());
        AddCustomerCommand = new RelayCommand(AddCustomer);
        ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
    }

    public ObservableCollection<Customer> Customers { get; }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public ICommand AddCustomerCommand { get; }
    public ICommand ExitCommand { get; }

    private void AddCustomer()
    {
        var customer = _windowService.ShowAddCustomerDialog();
        if (customer is null)
            return;

        Customers.Add(customer);
        StatusMessage = $"Customer '{customer.FullName}' was added";
    }
}
