# Test cases: US-001 Add a customer

Requirement: [US-001](../requirements/US-001-add-a-customer.md)

**Common preconditions:** the app is started with `--reset-data` (seed customers Ada Lovelace, Alan Turing, Grace Hopper), the user is signed in as `admin` / `Password1!`, and the **Add customer** dialog is open.

The **Tag** column is the planned Reqnroll tag (Episode 14): `@smoke` runs on every push, `@regression` in the full run.

| ID | Title | AC | Steps | Test data | Expected result | Tag |
|----|-------|----|-------|-----------|-----------------|-----|
| TC-001 | Save a customer with required fields only | AC1, AC9 | 1. Enter full name. 2. Enter email. 3. Click Save. | Name: `Margaret Hamilton`; Email: `margaret@example.com` | Dialog closes. Grid shows Margaret Hamilton, type Retail, today's date, newsletter unchecked. Status: "Customer 'Margaret Hamilton' was added". | @smoke |
| TC-002 | Save a customer with every field filled | AC1 | 1. Fill all fields. 2. Click Save. | Name: `Katherine Johnson`; Email: `katherine@example.com`; Phone: `+1 (555) 010-3000`; Type: Business; Since: 2024-01-15; Newsletter: checked | Dialog closes. Grid row shows every value entered. | @regression |
| TC-003 | Full name is required | AC2 | 1. Leave full name blank. 2. Enter email. 3. Click Save. | Email: `noname@example.com` | "Full name is required" shown. Dialog stays open. Nothing added. | @smoke |
| TC-004 | Email is required | AC3 | 1. Enter full name. 2. Leave email blank. 3. Click Save. | Name: `Margaret Hamilton` | "Email is required" shown. Dialog stays open. Nothing added. | @smoke |
| TC-005 | Invalid email formats are rejected | AC4 | 1. Enter full name. 2. Enter an invalid email. 3. Click Save. Repeat per data row. | `margaret`, `margaret@`, `@example.com`, `margaret@example`, `mar garet@example.com` | "Enter a valid email address" shown for each. Nothing added. | @regression |
| TC-006 | Duplicate email is rejected, ignoring case | AC5 | 1. Enter full name. 2. Enter a seed customer's email in different case. 3. Click Save. | Name: `Another Ada`; Email: `ADA@example.com` | "A customer with this email already exists" shown. Nothing added. | @regression |
| TC-007 | Full name at the 100-character limit is accepted | AC6 | 1. Enter a 100-character name. 2. Enter email. 3. Click Save. | Name: 100 × `A`; Email: `long@example.com` | Customer saved. | @regression |
| TC-008 | Full name over 100 characters is rejected | AC6 | 1. Enter a 101-character name. 2. Enter email. 3. Click Save. | Name: 101 × `A`; Email: `toolong@example.com` | "Full name must be 100 characters or fewer" shown. Nothing added. | @regression |
| TC-009 | Invalid phone is rejected | AC7 | 1. Enter name and email. 2. Enter an invalid phone. 3. Click Save. Repeat per data row. | `555-01` (too short), `call me`, `555.0101` | Phone message shown for each. Nothing added. | @regression |
| TC-010 | Future customer since date is rejected | AC8 | 1. Enter name and email. 2. Set Customer since to tomorrow. 3. Click Save. | Since: today + 1 day | "Customer since date cannot be in the future" shown. Nothing added. | @regression |
| TC-011 | Customer since is required | AC8 | 1. Enter name and email. 2. Clear the Customer since date. 3. Click Save. | Since: empty | "Customer since date is required" shown. Nothing added. | @regression |
| TC-012 | Defaults when the dialog opens | AC8, AC9 | 1. Open the dialog. 2. Inspect each field. | | Type is Retail, Customer since is today, Newsletter is unchecked, all text boxes empty, no messages shown. Type list holds Retail, Business, Wholesale. | @regression |
| TC-013 | Cancel discards the customer | AC10 | 1. Enter name and email. 2. Click Cancel. | Name: `Margaret Hamilton`; Email: `margaret@example.com` | Dialog closes. Grid still has 3 customers. Status unchanged. | @smoke |
| TC-014 | Escape discards the customer | AC10 | 1. Enter name and email. 2. Press Escape. | as TC-013 | as TC-013 | @regression |
| TC-015 | All errors shown together and input kept | AC11 | 1. Leave name blank. 2. Enter invalid email and phone. 3. Click Save. | Email: `bad`; Phone: `abc` | Name, email and phone messages all shown. Dialog open. Email and phone text still present. | @regression |
| TC-016 | Spaces around values are trimmed | AC1 | 1. Enter name and email with leading and trailing spaces. 2. Click Save. | Name: `  Margaret Hamilton  `; Email: `  margaret@example.com  ` | Grid shows `Margaret Hamilton` and `margaret@example.com` without spaces. | @regression |

## Gherkin preview (Episode 4)

TC-003 and TC-004 become plain scenarios; TC-005 and TC-009 become scenario outlines with an `Examples:` table built from their data rows.
