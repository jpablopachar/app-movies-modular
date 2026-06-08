# Copilot Instructions for app-movies-modular

Use this file as the quick-start guide. For full architecture details, see [CLAUDE.md](../CLAUDE.md).

## Build and run

- SDK is pinned in [global.json](../global.json) (10.0.300).
- Build solution: `dotnet build server/server.slnx`
- Restore packages: `dotnet restore server/server.slnx`
- Run API: `dotnet run --project server/Api`

## Architecture boundaries (must follow)

- This is a modular monolith (`Movies`, `Users`, `Contracts`, `Api`, plus shared `libs`).
- `Contracts` is the only shared surface between feature modules.
- Do not add direct references between `Movies` and `Users`.
- Keep module-specific implementation inside each module.

Project references to preserve:
- `Api` -> `Movies`, `Users`
- `Movies` -> `Contracts`
- `Users` -> `Contracts`
- `Contracts` -> `libs/Mediator`, `libs/Results`

## Core implementation patterns

- Mediator is custom (not MediatR). Handlers implement `IRequestHandler<TRequest, TResponse>` and are discovered via `AddMediator(...)`.
- Result pattern is custom (`Result<T>` + `ResultStatus`). Prefer `Result<T>.Success(...)`, `.Error(...)`, `.NotFound()`, etc.
- FastEndpoints endpoints that require validation should inherit `ValidatedEndpoint<TRequest>`.
- Validators implement `IValidator<TRequest>` and should return validation errors (no exceptions for validation flow).

See examples:
- Query/response contract: [server/Contracts/MovieDetails/MovieDetailsQuery.cs](../server/Contracts/MovieDetails/MovieDetailsQuery.cs)
- Validated endpoint base: [server/Contracts/Core/ValidatedEndpoint.cs](../server/Contracts/Core/ValidatedEndpoint.cs)
- Mediator registration: [libs/Mediator/ServiceCollectionExtensions.cs](../libs/Mediator/ServiceCollectionExtensions.cs)
- Validator registration: [server/Contracts/ServiceCollectionValidatorExtensions.cs](../server/Contracts/ServiceCollectionValidatorExtensions.cs)

## Feature workflow

When adding a new feature:
1. Add query/response records in `server/Contracts/<Feature>/`.
2. Implement handler in the owning module (`server/Movies` or `server/Users`).
3. Add FastEndpoints endpoint in the owning module.
4. Add validator in the owning module when needed.
5. Ensure DI wiring in API is updated where needed.

## Code documentation

All C# code must be documented in **Spanish** using XML comments (`///`). Full rules: [`.github/instructions/documentation.instructions.md`](instructions/documentation.instructions.md).

- Document: classes, interfaces, enums, records, delegates, public/internal methods, public/internal properties, non-trivial constructors.
- Always include `<summary>`. Add `<param>`, `<returns>`, and `<exception>` on methods following the rules in the instructions file.
- Use `<see cref="..."/>` to cross-reference related types. Use `<see langword="null"/>` etc. for C# keywords.
- Query records → describe what they request. Response records → describe what they return. Handler classes → reference the handled query via `<see cref="..."/>`. Domain `internal` methods → document preconditions and thrown exceptions.

## Agent guardrails

- Prefer minimal, focused edits; avoid broad refactors unless requested.
- Validate changes with `dotnet build server/server.slnx` after meaningful edits.
- If changing startup/wiring, verify [server/Api/Program.cs](../server/Api/Program.cs) still reflects intended architecture.
- Never introduce cross-module coupling that bypasses `Contracts`.
- All new or modified C# members must include Spanish XML documentation following the rules above.
