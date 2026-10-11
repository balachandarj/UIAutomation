using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CustomerManager.App.Models;

namespace CustomerManager.App.Services;

/// <summary>
/// Stores customers in a local JSON file so tests need no database.
/// The file path can be overridden with the CUSTOMERMANAGER_DATA_FILE environment variable,
/// and the app's --reset-data argument restores the seed data (used by test hooks).
/// </summary>
public class JsonCustomerRepository : ICustomerRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _filePath;
    private readonly List<Customer> _customers;

    public JsonCustomerRepository(string filePath, bool resetData = false)
    {
        _filePath = filePath;
        if (resetData || !File.Exists(_filePath))
        {
            _customers = SeedData();
            Save();
        }
        else
        {
            _customers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(_filePath), JsonOptions)
                         ?? new List<Customer>();
        }
    }

    public static string DefaultFilePath()
    {
        var overridePath = Environment.GetEnvironmentVariable("CUSTOMERMANAGER_DATA_FILE");
        if (!string.IsNullOrWhiteSpace(overridePath))
            return overridePath;

        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CustomerManager");
        return Path.Combine(folder, "customers.json");
    }

    public IReadOnlyList<Customer> GetAll() => _customers.AsReadOnly();

    public bool EmailExists(string email) =>
        _customers.Any(c => string.Equals(c.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));

    public void Add(Customer customer)
    {
        _customers.Add(customer);
        Save();
    }

    private void Save()
    {
        var folder = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(folder))
            Directory.CreateDirectory(folder);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_customers, JsonOptions));
    }

    private static List<Customer> SeedData() => new()
    {
        new Customer { FullName = "Ada Lovelace", Email = "ada@example.com", Phone = "555-0101",
                       Type = CustomerType.Business, CustomerSince = new DateTime(2021, 3, 14), SubscribeToNewsletter = true },
        new Customer { FullName = "Alan Turing", Email = "alan@example.com", Phone = "555-0102",
                       Type = CustomerType.Retail, CustomerSince = new DateTime(2022, 6, 23) },
        new Customer { FullName = "Grace Hopper", Email = "grace@example.com",
                       Type = CustomerType.Wholesale, CustomerSince = new DateTime(2023, 12, 9), SubscribeToNewsletter = true }
    };
}
