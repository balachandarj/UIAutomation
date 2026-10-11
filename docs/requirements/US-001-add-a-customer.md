# US-001: Add a customer

**As a** customer service agent
**I want to** add a new customer with their contact details
**So that** I can find and serve them later

## Context

- Screen: **Add customer** dialog, opened from the main window with the **Add customer** button or **Customers > Add customer...**
- Precondition for every criterion: the agent is signed in (demo account `admin` / `Password1!`).
- Fields: Full name\*, Email\*, Phone, Customer type, Customer since\*, Subscribe to newsletter. (\* = required)

## Acceptance criteria

| ID | Rule |
|----|------|
| AC1 | A customer with a valid full name and email is saved, the dialog closes, the customer appears in the customer list, and the status bar shows "Customer '&lt;name&gt;' was added". |
| AC2 | A customer cannot be saved without a full name. The message "Full name is required" is shown. |
| AC3 | A customer cannot be saved without an email address. The message "Email is required" is shown. |
| AC4 | The email must look like an email address (`name@domain.tld`). Otherwise "Enter a valid email address" is shown. |
| AC5 | Email addresses are unique, ignoring case. Saving a duplicate shows "A customer with this email already exists". |
| AC6 | The full name can be at most 100 characters. Longer names show "Full name must be 100 characters or fewer". |
| AC7 | Phone is optional. When entered, it must be 7 to 20 characters of digits, spaces, `+`, `-` and brackets, otherwise "Phone can contain only digits, spaces, +, - and brackets" is shown. |
| AC8 | Customer since defaults to today, is required, and cannot be in the future ("Customer since date is required" / "Customer since date cannot be in the future"). |
| AC9 | Customer type offers Retail, Business and Wholesale, and defaults to Retail. Newsletter defaults to unchecked. |
| AC10 | Cancel (or Escape) closes the dialog without saving anything. |
| AC11 | When validation fails, all field messages are shown at once, the dialog stays open, and the values already entered are kept. |

Leading and trailing spaces are trimmed from text fields before validation and saving.

## Out of scope for this story

Editing and deleting customers, searching the list, and a confirmation prompt when cancelling with unsaved changes. These arrive with later stories as the series needs them.
