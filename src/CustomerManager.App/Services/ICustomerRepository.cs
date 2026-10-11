using CustomerManager.App.Models;

namespace CustomerManager.App.Services;

public interface ICustomerRepository
{
    IReadOnlyList<Customer> GetAll();
    bool EmailExists(string email);
    void Add(Customer customer);
}
