using System.Text.RegularExpressions;
using System.Windows.Input;
using CustomerManager.App.Models;
using CustomerManager.App.Services;

namespace CustomerManager.App.ViewModels;

public class AddCustomerViewModel : ViewModelBase
{
    public const int MaxNameLength = 100;

    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private static readonly Regex PhonePattern = new(@"^[0-9+()\-\s]{7,20}$", RegexOptions.Compiled);

    private readonly ICustomerRepository _repository;

    private string _fullName = string.Empty;
    private string _email = string.Empty;
    private string _phone = string.Empty;
    private CustomerType _selectedType = CustomerType.Retail;
    private DateTime? _customerSince = DateTime.Today;
    private bool _subscribeToNewsletter;

    private string _fullNameError = string.Empty;
    private string _emailError = string.Empty;
    private string _phoneError = string.Empty;
    private string _customerSinceError = string.Empty;

    public AddCustomerViewModel(ICustomerRepository repository)
    {
        _repository = repository;
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(this, null));
    }

    /// <summary>Raised with the saved customer, or null when the user cancels.</summary>
    public event EventHandler<Customer?>? CloseRequested;

    public IReadOnlyList<CustomerType> CustomerTypes { get; } = Enum.GetValues<CustomerType>();

    public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
    public string Email { get => _email; set => SetProperty(ref _email, value); }
    public string Phone { get => _phone; set => SetProperty(ref _phone, value); }
    public CustomerType SelectedType { get => _selectedType; set => SetProperty(ref _selectedType, value); }
    public DateTime? CustomerSince { get => _customerSince; set => SetProperty(ref _customerSince, value); }
    public bool SubscribeToNewsletter { get => _subscribeToNewsletter; set => SetProperty(ref _subscribeToNewsletter, value); }

    public string FullNameError { get => _fullNameError; private set => SetProperty(ref _fullNameError, value); }
    public string EmailError { get => _emailError; private set => SetProperty(ref _emailError, value); }
    public string PhoneError { get => _phoneError; private set => SetProperty(ref _phoneError, value); }
    public string CustomerSinceError { get => _customerSinceError; private set => SetProperty(ref _customerSinceError, value); }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private void Save()
    {
        if (!Validate())
            return;

        var customer = new Customer
        {
            FullName = FullName.Trim(),
            Email = Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim(),
            Type = SelectedType,
            CustomerSince = CustomerSince!.Value.Date,
            SubscribeToNewsletter = SubscribeToNewsletter
        };

        _repository.Add(customer);
        CloseRequested?.Invoke(this, customer);
    }

    /// <summary>Validates every field on Save so all messages show at once.</summary>
    private bool Validate()
    {
        var name = FullName.Trim();
        FullNameError = name.Length == 0 ? "Full name is required"
            : name.Length > MaxNameLength ? $"Full name must be {MaxNameLength} characters or fewer"
            : string.Empty;

        var email = Email.Trim();
        EmailError = email.Length == 0 ? "Email is required"
            : !EmailPattern.IsMatch(email) ? "Enter a valid email address"
            : _repository.EmailExists(email) ? "A customer with this email already exists"
            : string.Empty;

        var phone = Phone.Trim();
        PhoneError = phone.Length > 0 && !PhonePattern.IsMatch(phone)
            ? "Phone can contain only digits, spaces, +, - and brackets"
            : string.Empty;

        CustomerSinceError = CustomerSince is null ? "Customer since date is required"
            : CustomerSince.Value.Date > DateTime.Today ? "Customer since date cannot be in the future"
            : string.Empty;

        return FullNameError.Length == 0 && EmailError.Length == 0
               && PhoneError.Length == 0 && CustomerSinceError.Length == 0;
    }
}
