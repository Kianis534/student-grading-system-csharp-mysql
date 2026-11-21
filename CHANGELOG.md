# Changelog

All notable changes to this project are documented here.
The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project uses [Semantic Versioning](https://semver.org/).

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
