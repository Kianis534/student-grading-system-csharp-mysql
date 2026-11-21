## What does this change?

<!-- A short summary and the reason for the change. Link the issue it closes, e.g. "Closes #12". -->

## How was it tested?

<!-- Unit/integration tests added or updated, and any manual checks in the app. -->

## Checklist

- [ ] `dotnet build -c Release` has no warnings
- [ ] `dotnet test -c Release` passes (with `SGS_TEST_CONNECTION_STRING` set for integration tests)
- [ ] Schema changes are in `database/01-schema.sql` and noted in `CHANGELOG.md`
- [ ] No credentials, `Database.config` or `.env` files are included
