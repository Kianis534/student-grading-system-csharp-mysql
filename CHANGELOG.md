# Changelog

All notable changes to this project are documented here.
The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project uses [Semantic Versioning](https://semver.org/).

## [Unreleased]

### Fixed
- Released grades went stale: after grades were calculated, changing marks or adding/deleting an assessment left the old grades in place, so students could see a grade that didn't match their marks. Released grades are now recalculated on every such change (courses without released grades stay ungraded).
- Values with more than two decimal places (for example 9.999 out of 10) passed validation and were then rounded by MySQL, or turned into 0.00 and hit a CHECK constraint. They're now rejected with a clear message.
- Desktop marks entry on Windows set to a comma-decimal language read "7.5" as 75. Thousands separators are no longer accepted in marks.
- Desktop **Save marks** stayed disabled while the first change was still being typed.
- A hand-edited `Database.config` that wasn't valid XML (for example an unescaped `&` in the password) crashed sign-in with a generic error. It now explains the problem and offers to open database settings.
- Live demo: message boxes now take the keyboard focus and lock the window behind them, as modal dialogs do. Before, Enter or Delete could still reach the grid behind an open confirmation.
- Live demo: **Ctrl+S** / **Cmd+S** on marks entry no longer opens the browser's "Save page" dialog.
- Live demo: leaving marks entry with unsaved marks now asks first however you leave, including the browser's back button, reloading and closing the tab.

### Changed
- Live demo restyled to mirror the desktop app: window frames with title bars, the same header bands, grid columns, button bars, Windows-style message boxes and keyboard shortcuts. The desktop app is unchanged.

## [1.1.0] - 2026-11-22

### Added
- **Live demo** at https://kianis534.github.io/student-grading-system-csharp-mysql/: a Blazor WebAssembly version that runs in the browser with sample data (preview only, resets on reload), deployed to GitHub Pages by a new workflow.
- `StudentGradingSystem.Data.InMemory`: in-memory repositories and sample data matching `02-seed.sql`, with tests that mirror the MySQL integration tests.

### Changed
- MySQL code moved out of `StudentGradingSystem.Core` into `StudentGradingSystem.Data.MySql`. Core no longer depends on any database and targets both .NET Framework 4.8 and .NET 10.
- `AuthService` accepts the PBKDF2 work factor to upgrade hashes to (the desktop app keeps 210,000 iterations).
- On .NET 10, password hashing uses `Rfc2898DeriveBytes.Pbkdf2` (the constructors are obsolete there).

## [1.0.0] - 2026-11-22

First public release.

### Added
- Instructor workspace: course overview, assessments with weightage tracking, spreadsheet-style marks entry, one-click grade calculation and CSV export.
- Student portal: enrolled courses, released grades and a per-assessment breakdown of how each mark counts toward the final grade.
- In-app **Database settings** dialog with a connection test, so the app can be configured without editing files.
- `scripts/setup-database.ps1` to create the database and write `Database.config` in one step.
- `docker-compose.yml` for a local MySQL 8.4 with the schema and demo data preloaded.
- Layered architecture: `StudentGradingSystem.Core` (domain, services, MySQL repositories) separated from the WinForms UI.
- 96 automated tests (unit + MySQL integration) and GitHub Actions CI.
- Release packaging script producing a ready-to-run zip.

### Security
- Passwords stored as salted PBKDF2-SHA256 hashes instead of plain text.
- Database credentials removed from source code; configuration now comes from an environment variable or a git-ignored file.
- Students can only read data for courses they are enrolled in.

### Fixed
- Database script failed on a fresh server because `Users` referenced `Students` before it existed.
- Opening a screen no longer kills other running copies of the app.
- Navigating back and forth no longer leaves hidden windows running in the background; closing the dashboard now exits the app.
- The grades view no longer shows the marks-entry controls.

[1.1.0]: https://github.com/Kianis534/student-grading-system-csharp-mysql/releases/tag/v1.1.0
[1.0.0]: https://github.com/Kianis534/student-grading-system-csharp-mysql/releases/tag/v1.0.0
