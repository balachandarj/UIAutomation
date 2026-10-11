using System.Globalization;
using CustomerManager.UiTests.Pages;
using CustomerManager.UiTests.Support;
using NUnit.Framework;
using Reqnroll;

namespace CustomerManager.UiTests.StepDefinitions;

/// <summary>
/// Steps read like the feature file and talk only to page objects; no locators or FlaUI calls here.
/// </summary>
[Binding]
public class AddCustomerSteps
{
    private readonly AppSession _session;

    public AddCustomerSteps(AppSession session) => _session = session;

    private MainPage MainPage => new(_session.WaitForWindow(MainPage.WindowId));
    private AddCustomerPage AddCustomerPage => new(_session.WaitForWindow(AddCustomerPage.WindowId));

    [Given("I am signed in")]
    public void GivenIAmSignedIn()
    {
        var loginPage = new LoginPage(_session.WaitForWindow(LoginPage.WindowId));
        loginPage.SignIn(TestSettings.Username, TestSettings.Password);
        _session.WaitForWindow(MainPage.WindowId);
    }

    [Given("I am on the Add Customer screen")]
    public void GivenIAmOnTheAddCustomerScreen()
    {
        MainPage.ClickAddCustomer();
        _session.WaitForWindow(AddCustomerPage.WindowId);
    }

    [When("I enter the full name {string}")]
    public void WhenIEnterTheFullName(string name) => AddCustomerPage.EnterFullName(name);

    [When("I enter a full name of {int} characters")]
    public void WhenIEnterAFullNameOfCharacters(int length) => AddCustomerPage.EnterFullName(new string('A', length));

    [When("I enter the email {string}")]
    public void WhenIEnterTheEmail(string email) => AddCustomerPage.EnterEmail(email);

    [When("I enter the phone {string}")]
    public void WhenIEnterThePhone(string phone) => AddCustomerPage.EnterPhone(phone);

    [When("I choose the customer type {string}")]
    public void WhenIChooseTheCustomerType(string type) => AddCustomerPage.SelectCustomerType(type);

    [When("I set the customer since date to {string}")]
    public void WhenISetTheCustomerSinceDateTo(string date) => AddCustomerPage.SetCustomerSince(ParseDate(date));

    [When("I clear the customer since date")]
    public void WhenIClearTheCustomerSinceDate() => AddCustomerPage.SetCustomerSince(null);

    [When("I tick subscribe to newsletter")]
    public void WhenITickSubscribeToNewsletter() => AddCustomerPage.SubscribeToNewsletter = true;

    [When("I save the customer")]
    public void WhenISaveTheCustomer() => AddCustomerPage.Save();

    [When("I cancel")]
    public void WhenICancel() => AddCustomerPage.Cancel();

    [When("I press Escape")]
    public void WhenIPressEscape() => AddCustomerPage.PressEscape();

    [Then("the Add Customer screen is closed")]
    public void ThenTheAddCustomerScreenIsClosed() =>
        Assert.That(_session.WaitForWindowToClose(AddCustomerPage.WindowId), Is.True,
            "The Add Customer screen should have closed.");

    [Then("the Add Customer screen is still open")]
    public void ThenTheAddCustomerScreenIsStillOpen() =>
        Assert.That(_session.FindWindow(AddCustomerPage.WindowId), Is.Not.Null,
            "The Add Customer screen should still be open.");

    [Then("I see the error {string}")]
    public void ThenISeeTheError(string message)
    {
        var page = AddCustomerPage;
        Assert.That(page.WaitForError(message), Is.True,
            $"Expected the error \"{message}\". Shown: {Describe(page.VisibleErrors)}");
    }

    [Then("no errors are shown")]
    public void ThenNoErrorsAreShown() => Assert.That(AddCustomerPage.VisibleErrors, Is.Empty);

    [Then("the customer list contains")]
    public void ThenTheCustomerListContains(DataTable table)
    {
        var customers = MainPage.Customers;
        foreach (var row in table.Rows)
        {
            var expected = new CustomerRow(
                row["Full name"],
                row["Email"],
                row["Phone"],
                row["Type"],
                ParseDate(row["Customer since"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                row["Newsletter"] == "Yes");

            Assert.That(customers, Does.Contain(expected),
                $"Customer not found in the list. Rows: {Describe(customers.Select(c => c.ToString()))}");
        }
    }

    [Then("the customer list has {int} customers")]
    public void ThenTheCustomerListHasCustomers(int count) => Assert.That(MainPage.Customers, Has.Count.EqualTo(count));

    [Then("no customer was added")]
    public void ThenNoCustomerWasAdded() =>
        Assert.That(MainPage.Customers, Has.Count.EqualTo(TestSettings.SeedCustomerCount));

    [Then("the status bar shows {string}")]
    public void ThenTheStatusBarShows(string message) => Assert.That(MainPage.StatusMessage, Is.EqualTo(message));

    [Then("the customer type is {string}")]
    public void ThenTheCustomerTypeIs(string type) => Assert.That(AddCustomerPage.CustomerType, Is.EqualTo(type));

    [Then("the customer type options are {string}")]
    public void ThenTheCustomerTypeOptionsAre(string options) =>
        Assert.That(AddCustomerPage.CustomerTypeOptions,
            Is.EqualTo(options.Split(',', StringSplitOptions.TrimEntries)));

    [Then("the customer since date is today")]
    public void ThenTheCustomerSinceDateIsToday() => Assert.That(AddCustomerPage.CustomerSince, Is.EqualTo(DateTime.Today));

    [Then("subscribe to newsletter is not ticked")]
    public void ThenSubscribeToNewsletterIsNotTicked() => Assert.That(AddCustomerPage.SubscribeToNewsletter, Is.False);

    [Then("the email field still contains {string}")]
    public void ThenTheEmailFieldStillContains(string value) => Assert.That(AddCustomerPage.Email, Is.EqualTo(value));

    [Then("the phone field still contains {string}")]
    public void ThenThePhoneFieldStillContains(string value) => Assert.That(AddCustomerPage.Phone, Is.EqualTo(value));

    /// <summary>Accepts "today", "tomorrow", "yesterday" or a yyyy-MM-dd date, so scenarios never go stale.</summary>
    private static DateTime ParseDate(string text) => text.Trim().ToLowerInvariant() switch
    {
        "today" => DateTime.Today,
        "tomorrow" => DateTime.Today.AddDays(1),
        "yesterday" => DateTime.Today.AddDays(-1),
        _ => DateTime.ParseExact(text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
    };

    private static string Describe(IEnumerable<string> items)
    {
        var list = items.ToList();
        return list.Count == 0 ? "(none)" : string.Join("; ", list);
    }
}
