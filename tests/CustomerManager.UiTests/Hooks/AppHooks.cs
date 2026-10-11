using CustomerManager.UiTests.Support;
using FlaUI.Core.Capturing;
using NUnit.Framework;
using Reqnroll;

namespace CustomerManager.UiTests.Hooks;

[Binding]
public class AppHooks
{
    private readonly AppSession _session;
    private readonly ScenarioContext _scenarioContext;
    private readonly IReqnrollOutputHelper _output;

    public AppHooks(AppSession session, ScenarioContext scenarioContext, IReqnrollOutputHelper output)
    {
        _session = session;
        _scenarioContext = scenarioContext;
        _output = output;
    }

    [BeforeScenario]
    public void LaunchApp() => _session.Launch();

    /// <summary>
    /// Takes a screenshot the moment a step fails, while the app still shows the problem, and attaches
    /// it to both the Reqnroll HTML report and the NUnit result. The app itself is closed when
    /// Reqnroll disposes the AppSession at the end of the scenario.
    /// </summary>
    [AfterStep]
    public void ScreenshotOnFailure()
    {
        if (_scenarioContext.TestError is null)
            return;

        var folder = Path.Combine(TestSettings.ResultsFolder, "screenshots");
        Directory.CreateDirectory(folder);

        var safeTitle = string.Concat(_scenarioContext.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
        var path = Path.Combine(folder, $"{DateTime.Now:yyyyMMdd-HHmmss}-{safeTitle}.png");

        Capture.Screen().ToFile(path);
        _output.AddAttachment(path);
        TestContext.AddTestAttachment(path);
    }
}
