using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentGradingSystem.Infrastructure;

/// <summary>
/// One place for the app's colours and control styling, so every screen looks consistent.
/// Designers mark controls with a <see cref="Control.Tag"/> ("primary", "danger", "header", ...)
/// and <see cref="Apply"/> styles them at runtime.
/// </summary>
internal static class Theme
{
    public static readonly Color Primary = Color.FromArgb(37, 99, 235);
    public static readonly Color PrimaryHover = Color.FromArgb(29, 78, 216);
    public static readonly Color Success = Color.FromArgb(22, 163, 74);
    public static readonly Color SuccessHover = Color.FromArgb(21, 128, 61);
    public static readonly Color Danger = Color.FromArgb(220, 38, 38);
    public static readonly Color DangerHover = Color.FromArgb(185, 28, 28);
    public static readonly Color Surface = Color.White;
    public static readonly Color Background = Color.FromArgb(243, 244, 246);
    public static readonly Color Border = Color.FromArgb(209, 213, 219);
    public static readonly Color SubtleRow = Color.FromArgb(249, 250, 251);
    public static readonly Color Selection = Color.FromArgb(219, 234, 254);
    public static readonly Color TextPrimary = Color.FromArgb(17, 24, 39);
    public static readonly Color TextMuted = Color.FromArgb(107, 114, 128);
    public static readonly Color HeaderSubtitle = Color.FromArgb(219, 234, 254);
    public static readonly Color ErrorText = Color.FromArgb(185, 28, 28);

    private const string FontFamily = "Segoe UI";

    private static Icon? _appIcon;

    /// <summary>Styles a form and every tagged control inside it.</summary>
    public static void Apply(Form form)
    {
        if (form is null)
        {
            throw new ArgumentNullException(nameof(form));
        }

        Icon? icon = GetAppIcon();
        if (icon is not null)
        {
            form.Icon = icon;
        }

        StyleChildren(form);
    }

    private static void StyleChildren(Control parent)
    {
        foreach (Control control in parent.Controls)
        {
            switch (control)
            {
                case Button button:
                    StyleButton(button, button.Tag as string);
                    break;
                case DataGridView grid:
                    StyleGrid(grid);
                    break;
                case Panel panel when Equals(panel.Tag, "header"):
                    StyleHeader(panel);
                    break;
                case Panel panel when Equals(panel.Tag, "footer"):
                    panel.BackColor = Background;
                    break;
            }

            if (control.HasChildren)
            {
                StyleChildren(control);
            }
        }
    }

    public static void StyleButton(Button button, string? kind)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.Cursor = Cursors.Hand;
        button.Font = new Font(FontFamily, 9.75F, FontStyle.Bold);
        button.UseVisualStyleBackColor = false;

        (Color back, Color hover, Color fore, int border) = kind switch
        {
            "primary" => (Primary, PrimaryHover, Color.White, 0),
            "success" => (Success, SuccessHover, Color.White, 0),
            "danger" => (Danger, DangerHover, Color.White, 0),
            _ => (Surface, SubtleRow, TextPrimary, 1),
        };

        button.BackColor = back;
        button.ForeColor = fore;
        button.FlatAppearance.BorderSize = border;
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.MouseOverBackColor = hover;
        button.FlatAppearance.MouseDownBackColor = hover;
    }

    private static void StyleHeader(Panel header)
    {
        header.BackColor = Primary;
        foreach (Control child in header.Controls)
        {
            if (child is Label label && !Equals(label.Tag, "logo"))
            {
                label.ForeColor = Equals(label.Tag, "subtitle") ? HeaderSubtitle : Color.White;
                label.BackColor = Color.Transparent;
            }
        }
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BorderStyle = BorderStyle.None;
        grid.BackgroundColor = Surface;
        grid.GridColor = Color.FromArgb(229, 231, 235);
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.MultiSelect = false;
        grid.AutoGenerateColumns = false;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = 40;
        grid.RowTemplate.Height = 34;

        grid.ColumnHeadersDefaultCellStyle.BackColor = SubtleRow;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TextMuted;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SubtleRow;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextMuted;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font(FontFamily, 9F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

        grid.DefaultCellStyle.Font = new Font(FontFamily, 9.75F);
        grid.DefaultCellStyle.ForeColor = TextPrimary;
        grid.DefaultCellStyle.BackColor = Surface;
        grid.DefaultCellStyle.SelectionBackColor = Selection;
        grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
        grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
        grid.AlternatingRowsDefaultCellStyle.BackColor = SubtleRow;

        // Line numeric column headers up with their right-aligned values.
        foreach (DataGridViewColumn column in grid.Columns)
        {
            if (column.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight ||
                column.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
            {
                column.HeaderCell.Style.Alignment = column.DefaultCellStyle.Alignment;
            }
        }
    }

    /// <summary>For read-only report grids: don't highlight an arbitrary first row after binding.</summary>
    public static void UseAsReport(DataGridView grid)
    {
        grid.TabStop = false;
        grid.DataBindingComplete += (_, _) =>
        {
            grid.ClearSelection();
            grid.CurrentCell = null;
        };
    }

    private static Icon? GetAppIcon()
    {
        if (_appIcon is null)
        {
            try
            {
                _appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (ArgumentException)
            {
                _appIcon = null;
            }
        }

        return _appIcon;
    }
}
