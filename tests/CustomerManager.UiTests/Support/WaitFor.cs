using System.Diagnostics;

namespace CustomerManager.UiTests.Support;

/// <summary>
/// Polls a condition instead of sleeping for a fixed time. UI Automation can briefly throw while
/// windows open and close, so an exception while polling counts as "not yet".
/// </summary>
public static class WaitFor
{
    private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(200);

    public static T? Value<T>(Func<T?> getValue, TimeSpan? timeout = null) where T : class
    {
        var limit = timeout ?? TestSettings.DefaultTimeout;
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            try
            {
                var value = getValue();
                if (value is not null)
                    return value;
            }
            catch (Exception) when (stopwatch.Elapsed < limit)
            {
                // Not ready yet; try again.
            }

            if (stopwatch.Elapsed >= limit)
                return null;
            Thread.Sleep(Interval);
        }
    }

    public static bool Condition(Func<bool> condition, TimeSpan? timeout = null) =>
        Value(() => condition() ? "done" : null, timeout) is not null;
}
