using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace CustomerManager.UiTests.Support;

/// <summary>
/// One running copy of the demo app for one scenario. Reqnroll creates it per scenario and shares the
/// same instance with hooks, steps and page objects through constructor injection, then disposes it.
/// </summary>
public sealed class AppSession : IDisposable
{
    private Application? _app;

    public UIA3Automation Automation { get; } = new();

    public Application App => _app ?? throw new InvalidOperationException("The app has not been launched.");

    public void Launch()
    {
        Directory.CreateDirectory(TestSettings.ResultsFolder);

        var startInfo = new ProcessStartInfo(TestSettings.AppPath, "--reset-data")
        {
            UseShellExecute = false
        };
        // Tests use their own data file, so they never touch the data you use when demoing the app.
        startInfo.Environment["CUSTOMERMANAGER_DATA_FILE"] = Path.Combine(TestSettings.ResultsFolder, "customers.json");

        _app = Application.Launch(startInfo);
    }

    /// <summary>
    /// Finds an open window of the app by AutomationId. A modal dialog is a child of its owner window
    /// in the UI Automation tree, so the children of each top-level window are searched too.
    /// </summary>
    public Window? FindWindow(string automationId)
    {
        foreach (var window in App.GetAllTopLevelWindows(Automation))
        {
            if (window.AutomationId == automationId)
                return window;

            var child = window.FindFirstChild(cf => cf.ByAutomationId(automationId));
            if (child is not null)
                return child.AsWindow();
        }

        return null;
    }

    public Window WaitForWindow(string automationId) =>
        WaitFor.Value(() => FindWindow(automationId))
        ?? throw new TimeoutException($"Window '{automationId}' did not appear within {TestSettings.DefaultTimeout.TotalSeconds}s.");

    public bool WaitForWindowToClose(string automationId) =>
        WaitFor.Condition(() => FindWindow(automationId) is null);

    public void Dispose()
    {
        if (_app is not null)
        {
            try
            {
                if (!_app.HasExited)
                    _app.Kill();
            }
            catch (InvalidOperationException)
            {
                // The process has already exited.
            }

            _app.Dispose();
            _app = null;
        }

        Automation.Dispose();
    }
}
