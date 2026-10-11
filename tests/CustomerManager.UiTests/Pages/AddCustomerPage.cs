using System.Globalization;
using CustomerManager.UiTests.Support;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace CustomerManager.UiTests.Pages;

public class AddCustomerPage : PageBase
{
    public const string WindowId = "AddCustomerWindow";

    private static readonly string[] ErrorIds = { "FullNameError", "EmailError", "PhoneError", "CustomerSinceError" };

    public AddCustomerPage(Window window) : base(window)
    {
    }

    public void EnterFullName(string value) => TypeInto(ById("FullNameTextBox"), value);
    public void EnterEmail(string value) => TypeInto(ById("EmailTextBox"), value);
    public void EnterPhone(string value) => TypeInto(ById("PhoneTextBox"), value);

    public string FullName => ById("FullNameTextBox").AsTextBox().Text;
    public string Email => ById("EmailTextBox").AsTextBox().Text;
    public string Phone => ById("PhoneTextBox").AsTextBox().Text;

    public void SelectCustomerType(string type)
    {
        ById("CustomerTypeComboBox").AsComboBox().Select(type);
        Wait.UntilInputIsProcessed();
    }

    public string? CustomerType => ById("CustomerTypeComboBox").AsComboBox().SelectedItem?.Text;

    public IReadOnlyList<string> CustomerTypeOptions =>
        ById("CustomerTypeComboBox").AsComboBox().Items.Select(item => item.Text).ToList();

    /// <summary>
    /// Types the date into the DatePicker's text box in this machine's short date format, then tabs
    /// away so the DatePicker parses it. Null clears the date.
    /// </summary>
    public void SetCustomerSince(DateTime? date)
    {
        var text = date?.ToString("d", CultureInfo.CurrentCulture) ?? string.Empty;
        TypeInto(DatePickerTextBox(), text);
        Keyboard.Type(VirtualKeyShort.TAB);
        Wait.UntilInputIsProcessed();
    }

    public DateTime? CustomerSince =>
        DateTime.TryParse(DatePickerTextBox().AsTextBox().Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var date)
            ? date.Date
            : null;

    // Intentionally found by Name: the newsletter check box has no AutomationId (Episode 6).
    private CheckBox NewsletterCheckBox => ByName("Subscribe to newsletter").AsCheckBox();

    public bool SubscribeToNewsletter
    {
        get => NewsletterCheckBox.IsChecked == true;
        set => NewsletterCheckBox.IsChecked = value;
    }

    public void Save() => Click(ById("SaveButton"));

    // Intentionally found by Name: the Cancel button has no AutomationId (Episode 6).
    public void Cancel() => Click(ByName("Cancel"));

    public void PressEscape()
    {
        Window.Focus();
        Keyboard.Type(VirtualKeyShort.ESCAPE);
        Wait.UntilInputIsProcessed();
    }

    /// <summary>The validation messages currently shown. Hidden messages are not in the tree at all.</summary>
    public IReadOnlyList<string> VisibleErrors =>
        ErrorIds.Select(TryById).Where(e => e is not null).Select(e => e!.Name).ToList();

    public bool WaitForError(string message) =>
        WaitFor.Condition(() => VisibleErrors.Contains(message));

    private AutomationElement DatePickerTextBox() =>
        WaitFor.Value(() => ById("CustomerSinceDatePicker").FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit)))
        ?? throw new InvalidOperationException("The Customer since date text box was not found.");
}
