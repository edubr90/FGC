# Contributing to FCG

Thank you for contributing to the FCG project. This document defines the mandatory standards and workflow for contributions.

## Guidelines

- Follow the `.editorconfig` rules for formatting and naming.
- Use 4-space indentation, file-scoped namespaces, and PascalCase for types and methods.
- Private fields must use `_camelCase` with `readonly` where applicable.

## Branches and Commits

- Branches: `feature/<short-desc>`, `fix/<short-desc>`, `hotfix/<short-desc>`.
- Commit messages: Use `type(scope): short description` (e.g., `feat(auth): add JWT token service`).

## Pull Requests

- Open a PR against `main` or an appropriate feature branch.
- Include a clear description of changes and link to any related issue.
- PRs must be reviewed by at least one teammate.

## Testing

- Add unit tests for critical business logic.
- Aim for meaningful tests, including one module developed with TDD or BDD.

## Database and Migrations

- Use Entity Framework Core migrations for schema changes.
- Add migration files and ensure `dotnet ef database update` runs locally.

## CI / Quality

- All PRs must pass build and test pipelines.
- Use static analysis and code style enforcement where possible.

## Local Development

- Use SQLite for local development and testing when possible.
- Store local secrets in user secrets or environment variables. Do not commit secrets.

## Questions

Contact the repository maintainers for guidance.
