@US-001
Feature: Add a customer
  As a customer service agent
  I want to add a new customer with their contact details
  So that I can find and serve them later

  Requirement: docs/requirements/US-001-add-a-customer.md
  Test cases:  docs/test-cases/US-001-add-a-customer.md

  Background:
    Given I am signed in
    And I am on the Add Customer screen

  @smoke @TC-001 @AC1 @AC9
  Scenario: Save a customer with required fields only
    When I enter the full name "Margaret Hamilton"
    And I enter the email "margaret@example.com"
    And I save the customer
    Then the Add Customer screen is closed
    And the customer list contains
      | Full name         | Email                | Phone | Type   | Customer since | Newsletter |
      | Margaret Hamilton | margaret@example.com |       | Retail | today          | No         |
    And the status bar shows "Customer 'Margaret Hamilton' was added"

  @regression @TC-002 @AC1
  Scenario: Save a customer with every field filled
    When I enter the full name "Katherine Johnson"
    And I enter the email "katherine@example.com"
    And I enter the phone "+1 (555) 010-3000"
    And I choose the customer type "Business"
    And I set the customer since date to "2024-01-15"
    And I tick subscribe to newsletter
    And I save the customer
    Then the Add Customer screen is closed
    And the customer list contains
      | Full name         | Email                 | Phone             | Type     | Customer since | Newsletter |
      | Katherine Johnson | katherine@example.com | +1 (555) 010-3000 | Business | 2024-01-15     | Yes        |

  @smoke @TC-003 @AC2
  Scenario: Full name is required
    When I enter the email "noname@example.com"
    And I save the customer
    Then I see the error "Full name is required"
    And the Add Customer screen is still open
    And no customer was added

  @smoke @TC-004 @AC3
  Scenario: Email is required
    When I enter the full name "Margaret Hamilton"
    And I save the customer
    Then I see the error "Email is required"
    And the Add Customer screen is still open
    And no customer was added

  @regression @TC-005 @AC4
  Scenario Outline: Invalid email formats are rejected
    When I enter the full name "Margaret Hamilton"
    And I enter the email "<email>"
    And I save the customer
    Then I see the error "Enter a valid email address"
    And no customer was added

    Examples:
      | email                 |
      | margaret              |
      | margaret@             |
      | @example.com          |
      | margaret@example      |
      | mar garet@example.com |

  @regression @TC-006 @AC5
  Scenario: Duplicate email is rejected, ignoring case
    When I enter the full name "Another Ada"
    And I enter the email "ADA@example.com"
    And I save the customer
    Then I see the error "A customer with this email already exists"
    And no customer was added

  @regression @TC-007 @AC6
  Scenario: Full name at the 100-character limit is accepted
    When I enter a full name of 100 characters
    And I enter the email "long@example.com"
    And I save the customer
    Then the Add Customer screen is closed
    And the customer list has 4 customers

  @regression @TC-008 @AC6
  Scenario: Full name over 100 characters is rejected
    When I enter a full name of 101 characters
    And I enter the email "toolong@example.com"
    And I save the customer
    Then I see the error "Full name must be 100 characters or fewer"
    And no customer was added

  @regression @TC-009 @AC7
  Scenario Outline: Invalid phone numbers are rejected
    When I enter the full name "Margaret Hamilton"
    And I enter the email "margaret@example.com"
    And I enter the phone "<phone>"
    And I save the customer
    Then I see the error "Phone can contain only digits, spaces, +, - and brackets"
    And no customer was added

    Examples:
      | phone    | reason        |
      | 555-01   | too short     |
      | call me  | letters       |
      | 555.0101 | dot separator |

  @regression @TC-010 @AC8
  Scenario: A future customer since date is rejected
    When I enter the full name "Margaret Hamilton"
    And I enter the email "margaret@example.com"
    And I set the customer since date to "tomorrow"
    And I save the customer
    Then I see the error "Customer since date cannot be in the future"
    And no customer was added

  @regression @TC-011 @AC8
  Scenario: Customer since date is required
    When I enter the full name "Margaret Hamilton"
    And I enter the email "margaret@example.com"
    And I clear the customer since date
    And I save the customer
    Then I see the error "Customer since date is required"
    And no customer was added

  @regression @TC-012 @AC8 @AC9
  Scenario: The form opens with default values
    Then the customer type is "Retail"
    And the customer type options are "Retail, Business, Wholesale"
    And the customer since date is today
    And subscribe to newsletter is not ticked
    And no errors are shown

  @smoke @TC-013 @AC10
  Scenario: Cancel discards the customer
    When I enter the full name "Margaret Hamilton"
    And I enter the email "margaret@example.com"
    And I cancel
    Then the Add Customer screen is closed
    And no customer was added
    And the status bar shows "Ready"

  @regression @TC-014 @AC10
  Scenario: Escape discards the customer
    When I enter the full name "Margaret Hamilton"
    And I enter the email "margaret@example.com"
    And I press Escape
    Then the Add Customer screen is closed
    And no customer was added

  @regression @TC-015 @AC11
  Scenario: All errors are shown together and input is kept
    When I enter the email "bad"
    And I enter the phone "abc"
    And I save the customer
    Then I see the error "Full name is required"
    And I see the error "Enter a valid email address"
    And I see the error "Phone can contain only digits, spaces, +, - and brackets"
    And the Add Customer screen is still open
    And the email field still contains "bad"
    And the phone field still contains "abc"

  @regression @TC-016 @AC1
  Scenario: Spaces around values are trimmed
    When I enter the full name "  Margaret Hamilton  "
    And I enter the email "  margaret@example.com  "
    And I save the customer
    Then the Add Customer screen is closed
    And the customer list contains
      | Full name         | Email                | Phone | Type   | Customer since | Newsletter |
      | Margaret Hamilton | margaret@example.com |       | Retail | today          | No         |
