using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace CustomerManager.UiTests.Pages;

public record CustomerRow(string FullName, string Email, string Phone, string Type, string CustomerSince, bool Newsletter);

public class MainPage : PageBase
{
    public const string WindowId = "MainWindow";

    public MainPage(Window window) : base(window)
    {
    }

    public void ClickAddCustomer() => Click(ById("AddCustomerButton"));

    public string StatusMessage => ById("StatusText").Name;

    /// <summary>Reads every row of the customer grid. Column order matches MainWindow.xaml.</summary>
    public IReadOnlyList<CustomerRow> Customers
    {
        get
        {
            var grid = ById("CustomersGrid").AsGrid();
            return grid.Rows.Select(row =>
            {
                var cells = row.Cells;
                var newsletter = cells[5].FindFirstDescendant(cf => cf.ByControlType(ControlType.CheckBox))
                    ?.AsCheckBox().IsChecked ?? false;
                return new CustomerRow(
                    CellText(cells[0]), CellText(cells[1]), CellText(cells[2]),
                    CellText(cells[3]), CellText(cells[4]), newsletter);
            }).ToList();
        }
    }

    private static string CellText(AutomationElement cell) =>
        cell.Patterns.Value.PatternOrDefault?.Value.ValueOrDefault ?? cell.Name ?? string.Empty;
}
