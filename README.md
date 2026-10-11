# Code and Automate

BDD UI automation for a Windows desktop (WPF) app with **FlaUI**, **Reqnroll** and **NUnit**, built one episode at a time.

## Run the demo app

Requires Windows and the .NET 8 SDK.

```
dotnet run --project src/CustomerManager.App
```

Sign in with `admin` / `Password1!`. Customers are stored in `%LOCALAPPDATA%\CustomerManager\customers.json` (override with the `CUSTOMERMANAGER_DATA_FILE` environment variable). Start with `--reset-data` to restore the three seed customers:

```
dotnet run --project src/CustomerManager.App -- --reset-data
```

## Repository layout

```
docs/requirements/   user stories and acceptance criteria
docs/test-cases/     test case tables per story
docs/automation-ids.md  locator map for the app
src/CustomerManager.App/  the demo WPF app (.NET 8, MVVM)
tests/CustomerManager.UiTests/  FlaUI + Reqnroll + NUnit suite (from Episode 5)
.github/workflows/   CI on a Windows runner (Episode 16)
```

## Episodes

Each episode has a start and end tag (`ep03-start`, `ep03-end`). Links are added as videos go live.

| # | Episode | Tags |
|---|---------|------|
| 0 | Series intro | |
| 1 | From requirement to automated test | |
| 2 | Meet the demo app | |
| 3 | Acceptance criteria and test cases | |
