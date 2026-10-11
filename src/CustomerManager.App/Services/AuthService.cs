namespace CustomerManager.App.Services;

/// <summary>
/// Demo-only authentication with one hard-coded account. Never do this in a real app.
/// </summary>
public class AuthService : IAuthService
{
    public const string DemoUsername = "admin";
    public const string DemoPassword = "Password1!";

    public bool SignIn(string username, string password) =>
        string.Equals(username?.Trim(), DemoUsername, StringComparison.OrdinalIgnoreCase)
        && password == DemoPassword;
}
