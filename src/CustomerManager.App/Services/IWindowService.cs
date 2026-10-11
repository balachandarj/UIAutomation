using CustomerManager.App.Models;

namespace CustomerManager.App.Services;

/// <summary>Keeps window creation out of the view models so they stay testable.</summary>
public interface IWindowService
{
    void ShowMainWindow();
    Customer? ShowAddCustomerDialog();
}
