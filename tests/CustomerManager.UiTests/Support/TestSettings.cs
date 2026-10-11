namespace CustomerManager.UiTests.Support;

public static class TestSettings
{
#if DEBUG
    private const string BuildConfiguration = "Debug";
#else
    private const string BuildConfiguration = "Release";
#endif

    /// <summary>How long to wait for windows and controls before failing.</summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

    public const string Username = "admin";
    public const string Password = "Password1!";

    /// <summary>The app starts with these three customers when launched with --reset-data.</summary>
    public const int SeedCustomerCount = 3;

    public static string ResultsFolder => Path.Combine(AppContext.BaseDirectory, "TestResults");

    /// <summary>
    /// Path to CustomerManager.App.exe. Set CUSTOMERMANAGER_APP_PATH to override; otherwise the app's
    /// build output in this repo is used, so building the solution is enough.
    /// </summary>
    public static string AppPath
    {
        get
        {
            var overridePath = Environment.GetEnvironmentVariable("CUSTOMERMANAGER_APP_PATH");
            if (!string.IsNullOrWhiteSpace(overridePath))
                return overridePath;

            var path = Path.Combine(FindRepoRoot(), "src", "CustomerManager.App", "bin", BuildConfiguration,
                "net8.0-windows", "CustomerManager.App.exe");
            if (!File.Exists(path))
                throw new FileNotFoundException(
                    $"The demo app was not found at {path}. Build the solution first, or set CUSTOMERMANAGER_APP_PATH.");
            return path;
        }
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "CodeAndAutomate.sln")))
            dir = dir.Parent;
        return dir?.FullName
               ?? throw new DirectoryNotFoundException("Could not find CodeAndAutomate.sln above the test output folder.");
    }
}
