using FlaUI.Core.AutomationElements;

namespace CustomerManager.UiTests.Pages;

public class LoginPage : PageBase
{
    public const string WindowId = "LoginWindow";

    public LoginPage(Window window) : base(window)
    {
    }

    public void SignIn(string username, string password)
    {
        TypeInto(ById("UsernameTextBox"), username);
        TypeInto(ById("PasswordBox"), password);
        Click(ById("SignInButton"));
    }

    public string? ErrorMessage => TryById("LoginErrorText")?.Name;
}
