using System.Windows.Input;
using CustomerManager.App.Services;

namespace CustomerManager.App.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        SignInCommand = new RelayCommand(SignIn);
    }

    /// <summary>Raised when the credentials are accepted; the view opens the main window.</summary>
    public event EventHandler? LoginSucceeded;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    /// <summary>Set from the view's PasswordChanged handler, because PasswordBox.Password can't be bound.</summary>
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            if (SetProperty(ref _errorMessage, value))
                OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public ICommand SignInCommand { get; }

    private void SignIn()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Username is required";
            return;
        }

        if (string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Password is required";
            return;
        }

        if (!_authService.SignIn(Username, Password))
        {
            ErrorMessage = "Invalid username or password";
            return;
        }

        ErrorMessage = string.Empty;
        LoginSucceeded?.Invoke(this, EventArgs.Empty);
    }
}
