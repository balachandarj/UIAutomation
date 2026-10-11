namespace CustomerManager.App.Models;

public enum CustomerType
{
    Retail,
    Business,
    Wholesale
}

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public CustomerType Type { get; set; } = CustomerType.Retail;
    public DateTime CustomerSince { get; set; } = DateTime.Today;
    public bool SubscribeToNewsletter { get; set; }
}
