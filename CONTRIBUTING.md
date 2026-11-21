# Contributing

Thanks for taking the time to improve Student Grading System. Bug reports, fixes and small features are all welcome.

## Before you start

- Check the [open issues](https://github.com/Kianis534/student-grading-system-csharp-mysql/issues) so work isn't duplicated.
- For anything bigger than a bug fix, open an issue first and describe what you'd like to change.

## Local setup

1. Follow [Getting started](README.md#getting-started) in the README to build the app and create the database.
2. Make sure everything passes before you change anything:

   ```bash
   dotnet build StudentGradingSystem.sln -c Release
   dotnet test StudentGradingSystem.sln -c Release
   ```

   To work on the live demo, run `dotnet run --project src/StudentGradingSystem.Web` and open the URL it prints.

3. To also run the MySQL integration tests, point them at a server you don't mind them using
   (each run creates and drops its own `sgs_test_*` database):

   ```powershell
   $env:SGS_TEST_CONNECTION_STRING = "Server=localhost;Port=3306;User ID=root;Password=<your password>"
   dotnet test StudentGradingSystem.sln -c Release
   ```

## Guidelines

- **Keep the layers separate.** Business rules live in `StudentGradingSystem.Core` (no UI, no database). SQL lives in `StudentGradingSystem.Data.MySql`. Forms and Razor pages only call services.
- **Keep both stores in sync.** The live demo runs on `StudentGradingSystem.Data.InMemory`. If you change repository behaviour, update both implementations and their tests.
- **Parameterize every query.** Never build SQL by concatenating user input.
- **Add tests.** New rules go with unit tests; new queries go with an integration test.
- **No secrets in commits.** `Database.config` and `.env` are git-ignored - keep it that way.
- **Match the existing style.** `.editorconfig` covers formatting; CI builds with warnings treated as errors.
- **Schema changes** go in `database/01-schema.sql` (and `02-seed.sql` if the demo data needs updating). Mention them in `CHANGELOG.md`.

## Pull requests

1. Create a branch from `main` (`fix/marks-validation`, `feat/csv-import`, ...).
2. Write clear commit messages in the imperative mood ("Add CSV export", not "Added CSV export").
3. Make sure CI is green, then open the PR and fill in the template.
