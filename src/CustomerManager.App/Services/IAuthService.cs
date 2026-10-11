namespace CustomerManager.App.Services;

public interface IAuthService
{
    bool SignIn(string username, string password);
}
