# Code and Automate

**BDD UI automation for Windows desktop apps.** Automate a real WPF app end to end with **FlaUI** (drives the UI), **Reqnroll** (Gherkin scenarios) and **NUnit** (runs and asserts), one episode at a time.

Most UI automation tutorials cover the web. This series is for manual testers and junior SDETs who have a WPF desktop app at work that nobody has automated yet.

| | |
|---|---|
| **What** | Automate a real WPF app end to end with FlaUI, Reqnroll and NUnit. |
| **How** | Start from a requirement, write acceptance criteria, turn them into test cases and Gherkin, then build the automation layer by layer, one episode per step. |
| **Final outcome** | A framework you can clone: readable feature files, page objects, stable locators, screenshots on failure, an HTML report, and tests running in GitHub Actions. |
| **Prerequisites** | Windows, Visual Studio 2022 or Rider, .NET 8, basic C#. No prior automation experience needed. |
| **Not covered** | Web or mobile automation, WinForms deep dives, paid tools. |

## Episodes

Every episode has a git tag where it starts and one where it ends, so you can check out exactly the code you see on screen:

```
git checkout ep03-start
```

Video links and tags are added as episodes go live. The finished "Add a customer" suite shown in the Episode 0 intro is at tag [`ep00-hook`](https://github.com/balachandarj/UIAutomation/tree/ep00-hook).

### Module 1: Foundations

| # | Episode | You leave with | Video | Code |
|---|---------|----------------|-------|------|
| 0 | Series intro: what, how, final outcome | A clear picture of the finished suite and why to follow along | Coming soon | ep00-start · ep00-end |
| 1 | From requirement to automated test: the whole flow | The mental model the rest of the series follows | Coming soon | ep01-start · ep01-end |
| 2 | Meet the demo app | The app running locally from the repo | Coming soon | [ep02-start](https://github.com/balachandarj/UIAutomation/tree/ep02-start) · ep02-end |
| 3 | Writing acceptance criteria and test cases for one user story | A test case table for "Add a customer" | Coming soon | ep03-start · ep03-end |
| 4 | Turning test cases into Gherkin feature files | First .feature file with scenarios and a scenario outline | Coming soon | ep04-start · ep04-end |

### Module 2: First passing test

| # | Episode | You leave with | Video | Code |
|---|---------|----------------|-------|------|
| 5 | Tooling setup: .NET 8, NUnit, Reqnroll, FlaUI packages | A solution that builds and runs an empty scenario | Coming soon | ep05-start · ep05-end |
| 6 | Finding elements: Inspect, FlaUInspect and AutomationId | Knowing how to pick a stable locator | Coming soon | ep06-start · ep06-end |
| 7 | Launching and closing the app with FlaUI | Hooks that start the app before each scenario | Coming soon | ep07-start · ep07-end |
| 8 | Step definitions: wiring Gherkin to FlaUI | First green scenario on screen | Coming soon | ep08-start · ep08-end |

### Module 3: A real framework

| # | Episode | You leave with | Video | Code |
|---|---------|----------------|-------|------|
| 9 | Page Object Model for WPF windows | Steps that read like English, no locators in step code | Coming soon | ep09-start · ep09-end |
| 10 | Waits and flakiness: retries, conditions, timing | Tests that pass ten runs in a row | Coming soon | ep10-start · ep10-end |
| 11 | Test data and scenario context | Data-driven scenarios without copy-paste | Coming soon | ep11-start · ep11-end |
| 12 | Grids, combo boxes, dialogs and menus | Patterns for the tricky WPF controls | Coming soon | ep12-start · ep12-end |
| 13 | Assertions, screenshots and logging on failure | Useful evidence when a test fails | Coming soon | ep13-start · ep13-end |

### Module 4: Scale and CI

| # | Episode | You leave with | Video | Code |
|---|---------|----------------|-------|------|
| 14 | Tags, filtering and organising a growing suite | Smoke vs regression runs | Coming soon | ep14-start · ep14-end |
| 15 | Living documentation and HTML reports | A report to show a manager | Coming soon | ep15-start · ep15-end |
| 16 | Running in GitHub Actions on a Windows runner | Tests running on every push | Coming soon | ep16-start · ep16-end |
| 17 | Capstone: a new feature from requirement to CI in one video | Proof you can repeat the whole flow alone | Coming soon | ep17-start · ep17-end |

## Run the demo app

Requires Windows and the .NET 8 SDK.

```
dotnet run --project src/CustomerManager.App
```

Sign in with `admin` / `Password1!`. Customers are stored in `%LOCALAPPDATA%\CustomerManager\customers.json` (override with the `CUSTOMERMANAGER_DATA_FILE` environment variable). Start with `--reset-data` to restore the three seed customers:

```
dotnet run --project src/CustomerManager.App -- --reset-data
```

## Run the UI tests

The suite drives the real app with the mouse and keyboard, so run it on a Windows desktop session and don't touch the mouse while it runs.

```
dotnet build
dotnet test tests/CustomerManager.UiTests
```

Each scenario launches the app with its own data file, signs in and works through the "Add a customer" story (`tests/CustomerManager.UiTests/Features/AddCustomer.feature`). Afterwards, open the HTML report at `tests/CustomerManager.UiTests/bin/Debug/net8.0-windows/TestResults/report.html`. Failed steps get a screenshot attached in the report and in the NUnit results.

Run only the smoke scenarios:

```
dotnet test tests/CustomerManager.UiTests --filter Category=smoke
```

## Repository layout

```
docs/requirements/   user stories and acceptance criteria
docs/test-cases/     test case tables per story
docs/automation-ids.md  locator map for the app
docs/thumbnail/      thumbnail template (open in a browser, save the 1280x720 image)
src/CustomerManager.App/  the demo WPF app (.NET 8, MVVM)
tests/CustomerManager.UiTests/  FlaUI + Reqnroll + NUnit suite (from Episode 5)
.github/workflows/   CI on a Windows runner (Episode 16)
```
