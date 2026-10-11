# AutomationIds in the demo app

The locators the page objects (Episode 9) will use. Two controls have **no AutomationId on purpose**, so Episode 6 can show what happens when developers forget them.

## Login window (`LoginWindow`)

| Control | AutomationId |
|---------|--------------|
| Username text box | `UsernameTextBox` |
| Password box | `PasswordBox` |
| Error message | `LoginErrorText` (only in the tree when shown) |
| Sign in button | `SignInButton` |

## Main window (`MainWindow`)

| Control | AutomationId |
|---------|--------------|
| Menu bar | `MainMenu` |
| File menu / Exit | `FileMenu` / `ExitMenuItem` |
| Customers menu / Add customer | `CustomersMenu` / `AddCustomerMenuItem` |
| Add customer button | `AddCustomerButton` |
| Customer grid | `CustomersGrid` |
| Status bar text | `StatusText` |

## Add customer dialog (`AddCustomerWindow`)

| Control | AutomationId |
|---------|--------------|
| Full name | `FullNameTextBox`, error `FullNameError` |
| Email | `EmailTextBox`, error `EmailError` |
| Phone | `PhoneTextBox`, error `PhoneError` |
| Customer type | `CustomerTypeComboBox` |
| Customer since | `CustomerSinceDatePicker`, error `CustomerSinceError` |
| Subscribe to newsletter check box | **none** (find by Name "Subscribe to newsletter") |
| Save button | `SaveButton` |
| Cancel button | **none** (find by Name "Cancel") |

Error messages are collapsed when empty, so they are absent from the UI Automation tree until validation fails. Tests should wait for them to appear rather than read their text straight away.
