using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Web.Services;

namespace StudentGradingSystem.Web.Components;

/// <summary>Base class for demo pages: session, message boxes, navigation and grid helpers.</summary>
public abstract class DemoPage : ComponentBase
{
    [Inject]
    protected DemoSession Session { get; set; } = default!;

    [Inject]
    protected DialogService Dialogs { get; set; } = default!;

    [Inject]
    protected NavigationManager Nav { get; set; } = default!;

    /// <summary>Sends the visitor to the sign-in window unless they're signed in with <paramref name="role"/>.</summary>
    protected bool EnsureRole(UserRole role)
    {
        if (Session.User?.Role == role)
        {
            return true;
        }

        Nav.NavigateTo(string.Empty, replace: true);
        return false;
    }

    /// <summary>Desktop: "Sign out" returns to the login window.</summary>
    protected void SignOut()
    {
        Session.SignOut();
        Nav.NavigateTo(string.Empty);
    }

    /// <summary>Arrow keys move the selected grid row, like a DataGridView. Returns the new index.</summary>
    protected static int MoveSelection(KeyboardEventArgs e, int current, int count)
    {
        if (count == 0)
        {
            return -1;
        }

        return e.Key switch
        {
            "ArrowDown" => Math.Min(count - 1, current + 1),
            "ArrowUp" => Math.Max(0, current - 1),
            "Home" => 0,
            "End" => count - 1,
            _ => current,
        };
    }

    protected static string Format(decimal value) => value.ToString("0.##");

    protected static string Percent(decimal value) => value.ToString("0.00") + "%";

    /// <summary>Same separator the desktop app uses between subtitle parts.</summary>
    protected const string Dot = "  ·  ";
}
