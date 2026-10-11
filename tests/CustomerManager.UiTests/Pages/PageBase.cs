using CustomerManager.UiTests.Support;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;

namespace CustomerManager.UiTests.Pages;

/// <summary>
/// Shared helpers for page objects. Locators live only in page objects, never in step definitions.
/// </summary>
public abstract class PageBase
{
    protected PageBase(Window window) => Window = window;

    public Window Window { get; }

    /// <summary>Finds a control by AutomationId, waiting for it to appear.</summary>
    protected AutomationElement ById(string automationId) =>
        WaitFor.Value(() => Window.FindFirstDescendant(cf => cf.ByAutomationId(automationId)))
        ?? throw new InvalidOperationException($"Control '{automationId}' was not found in '{Window.Title}'.");

    /// <summary>Finds a control by its Name, for controls that have no AutomationId.</summary>
    protected AutomationElement ByName(string name) =>
        WaitFor.Value(() => Window.FindFirstDescendant(cf => cf.ByName(name)))
        ?? throw new InvalidOperationException($"Control named '{name}' was not found in '{Window.Title}'.");

    /// <summary>Returns the control right now, or null when it isn't in the tree (for example a collapsed error).</summary>
    protected AutomationElement? TryById(string automationId) =>
        Window.FindFirstDescendant(cf => cf.ByAutomationId(automationId));

    protected static void TypeInto(AutomationElement element, string text)
    {
        element.AsTextBox().Enter(text);
        Wait.UntilInputIsProcessed();
    }

    protected static void Click(AutomationElement element)
    {
        element.AsButton().Click();
        Wait.UntilInputIsProcessed();
    }
}
