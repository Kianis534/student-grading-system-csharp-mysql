namespace StudentGradingSystem.Web.Services;

public enum DialogIcon
{
    Information,
    Question,
    Warning,
    Error,
}

/// <summary>A message box waiting for an answer.</summary>
public sealed class DialogRequest
{
    public DialogRequest(string message, DialogIcon icon, IReadOnlyList<string> buttons, TaskCompletionSource<string?> completion)
    {
        Message = message;
        Icon = icon;
        Buttons = buttons;
        Completion = completion;
    }

    public string Message { get; }

    public DialogIcon Icon { get; }

    public IReadOnlyList<string> Buttons { get; }

    internal TaskCompletionSource<string?> Completion { get; }
}

/// <summary>
/// Browser stand-in for the desktop app's <c>MessageBox.Show</c>: same wording, same buttons.
/// Pages await the result just like the WinForms code does.
/// </summary>
public sealed class DialogService
{
    public event Action? Changed;

    public DialogRequest? Current { get; private set; }

    public Task<string?> ShowAsync(string message, DialogIcon icon, params string[] buttons)
    {
        Current?.Completion.TrySetResult(null);

        var completion = new TaskCompletionSource<string?>();
        Current = new DialogRequest(message, icon, buttons.Length == 0 ? new[] { "OK" } : buttons, completion);
        Changed?.Invoke();
        return completion.Task;
    }

    public Task InfoAsync(string message) => ShowAsync(message, DialogIcon.Information, "OK");

    public Task WarningAsync(string message) => ShowAsync(message, DialogIcon.Warning, "OK");

    /// <summary>Yes/No question; "No" is the safe default, as in the desktop app.</summary>
    public async Task<bool> ConfirmAsync(string message, bool destructive = false) =>
        await ShowAsync(message, destructive ? DialogIcon.Warning : DialogIcon.Question, "Yes", "No") == "Yes";

    public void Close(string? result)
    {
        DialogRequest? request = Current;
        Current = null;
        Changed?.Invoke();
        request?.Completion.TrySetResult(result);
    }
}
