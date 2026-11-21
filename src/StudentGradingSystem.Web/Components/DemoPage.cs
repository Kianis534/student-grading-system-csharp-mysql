using Microsoft.AspNetCore.Components;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Web.Services;

namespace StudentGradingSystem.Web.Components;

/// <summary>Base class for demo pages: access to the session and a role check.</summary>
public abstract class DemoPage : ComponentBase
{
    [Inject]
    protected DemoSession Session { get; set; } = default!;

    [Inject]
    protected NavigationManager Nav { get; set; } = default!;

    /// <summary>Sends the visitor to the sign-in page unless they're signed in with <paramref name="role"/>.</summary>
    protected bool EnsureRole(UserRole role)
    {
        if (Session.User?.Role == role)
        {
            return true;
        }

        Nav.NavigateTo(string.Empty, replace: true);
        return false;
    }

    protected static string Format(decimal value) => value.ToString("0.##");

    protected static string Percent(decimal value) => value.ToString("0.00") + "%";
}
