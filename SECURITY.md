# Security Policy

## Reporting a vulnerability

Please **don't** open a public issue for security problems.
Report them privately through GitHub's
[security advisory form](https://github.com/Kianis534/student-grading-system-csharp-mysql/security/advisories/new)
and include steps to reproduce. You should get a reply within a few days.

## Supported versions

Only the latest release on `main` receives fixes.

## How the app protects data

| Area | Approach |
| --- | --- |
| Passwords | Salted PBKDF2-HMAC-SHA256 (210,000 iterations), compared in constant time. Older, weaker hashes are upgraded on the next sign-in. |
| Sign-in | Unknown usernames take as long to reject as wrong passwords, so accounts can't be discovered by timing. The error message never says which part was wrong. |
| SQL | Every query is parameterized. Multi-row writes run in transactions. |
| Data access | Students can only read courses they are enrolled in; the check is enforced in SQL, not just the UI. |
| Database integrity | Foreign keys, unique keys and `CHECK` constraints enforce the rules even if the app is bypassed. |
| Secrets | No credentials in source control. The connection string comes from the `SGS_CONNECTION_STRING` environment variable or a git-ignored `Database.config`. |
| CSV export | Values that start with `=`, `+`, `-` or `@` are escaped to prevent spreadsheet formula injection. |

## Recommendations for deployments

- Create a dedicated MySQL account with only `SELECT, INSERT, UPDATE, DELETE` on the
  `StudentGradingSystem` schema (see [`database/create-app-user.sql`](database/create-app-user.sql)) instead of using `root`.
- Change or delete the demo accounts from `database/02-seed.sql` before storing real student data.
- Keep `Database.config` readable only by the people who run the app.
