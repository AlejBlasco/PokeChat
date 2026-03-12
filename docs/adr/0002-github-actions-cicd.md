# 0002. GitHub Actions CI/CD Pipeline

## Status
Accepted

## Date
2026-03-12

## Context
PokeChat requires an automated CI/CD pipeline to ensure code quality, security,
and build integrity on every pull request and push. The pipeline must enforce
security levels 1-4 defined in the project global rules.

The following checks are required:
- NuGet package vulnerability scanning
- Dependabot for automated dependency updates
- Code linting and style enforcement
- Code smell detection
- Build validation

## Decision
We will use GitHub Actions as the CI/CD platform with the following workflows:

- **ci.yml** — Restores, lints, builds and validates the solution on every push and pull request.
- **codeql.yml** — SAST analysis using CodeQL for C# on every push, pull request and weekly schedule.
- **dependency-review.yml** — Scans for vulnerable dependencies on every pull request.
- **dependabot.yml** — Automated weekly dependency updates for NuGet and GitHub Actions.

All workflows use pinned SHA action versions and minimum required permissions.

## Consequences
### Positive
- Every pull request is validated before merge.
- Vulnerable packages are detected automatically.
- Code quality and security are enforced consistently.
- Dependabot keeps dependencies up to date with minimal manual effort.

### Negative
- Initial setup cost for configuring all workflows.
- Build times increase slightly due to security scanning steps.

### Risks
- Pinned SHA versions must be updated periodically — mitigated by Dependabot watching GitHub Actions.
