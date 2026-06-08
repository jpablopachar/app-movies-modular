# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the entire solution
dotnet build server/server.slnx

# Run the API (http://localhost:5178, https://localhost:7143)
dotnet run --project server/Api

# Build a single project
dotnet build server/Movies

# Restore packages
dotnet restore server/server.slnx
```

The solution file is at `server/server.slnx`. The SDK version is pinned to 10.0.300 via `global.json`.

## Architecture

This is a **modular monolith** built on .NET 10 with **FastEndpoints** for HTTP handling and **PostgreSQL** via EF Core.

### Project layout

```
libs/
  Mediator/     # Custom mediator implementation (IRequest, IRequestHandler, IPipelineBehavior)
  Results/      # Custom Result pattern (Result<T>, ResultStatus, PagedResult, ResultValidationError)
server/
  Contracts/    # Cross-module contracts: queries, responses, IValidator, ValidatedEndpoint
  Movies/       # Movies feature module (EF Core + PostgreSQL)
  Users/        # Users feature module (ASP.NET Identity + cart, EF Core + PostgreSQL, Serilog)
  Api/          # Entry point — references Movies and Users; wires up DI and HTTP pipeline
  server.slnx   # Solution file
```

### Key patterns

**Mediator** (`libs/Mediator`): Custom implementation — not MediatR. Handlers are registered automatically via `AddMediator(assembly)`, which scans for `IRequestHandler<TRequest, TResponse>` implementations. Pipeline behaviors (`IPipelineBehavior<TRequest, TResponse>`) are chained in reverse registration order.

**Result pattern** (`libs/Results`): Custom implementation — not Ardalis. Handlers return `Result<T>` with a `ResultStatus` (Ok, Error, Invalid, NotFound, Forbidden, Unauthorized, Conflict, CriticalError, Unavailable). Use the static factory methods: `Result<T>.Success(value)`, `Result<T>.Error(...)`, `Result<T>.NotFound()`, etc.

**Contracts** (`server/Contracts`): Shared layer consumed by both feature modules and the API. Queries are records that implement `IRequest<Result<TResponse>>`. Example: `MovieDetailsQuery : IRequest<Result<MovieDetailsResponse>>`.

**Endpoint validation**: Endpoints inherit `ValidatedEndpoint<TRequest>` (defined in `Contracts/Core`), which resolves all registered `IValidator<TRequest>` services, runs them, and returns a 400 with a structured error dict before calling `OnValidatedAsync`. Register validators module-by-module using `services.AddModuleValidators(assembly)`.

**Domain encapsulation**: Domain entities (`AppUser`, `CartMovie`) expose mutation only via `internal` methods (e.g. `AddMovieToCart`, `RemoveItemFromCart`, `UpdateQuantity`). Public setters are avoided; use the named methods instead.

### Dependency flow

```
Api → Movies → Contracts → Mediator, Results
Api → Users  → Contracts
```

`Contracts` is the only cross-module shared surface — feature modules must not reference each other.

### Adding a new feature

1. Add the query/response records to `server/Contracts/<FeatureName>/`.
2. Implement `IRequestHandler<Query, Result<Response>>` in the feature module (`Movies` or `Users`).
3. Create the FastEndpoints endpoint in the feature module, extending `ValidatedEndpoint<TRequest>` if validation is needed.
4. If validation is required, implement `IValidator<TRequest>` in the feature module and ensure `AddModuleValidators` is called for that assembly in `Api/Program.cs`.

## Code documentation

All C# code must be documented in **Spanish** using standard .NET XML comments (`///`). Full rules are in [`.github/instructions/documentation.instructions.md`](.github/instructions/documentation.instructions.md).

**What to document**: classes, interfaces, enums, records, delegates, public and internal methods, public and internal properties, non-trivial constructors.

**Required tags by member type**:

- Types (`class`, `interface`, `enum`, `record`): `<summary>` always.
- Methods and constructors: `<summary>` + `<param>` for each non-obvious parameter + `<returns>` when not `void`/valueless `Task` + `<exception>` only for intentionally thrown exceptions.
- Properties and enum values: `<summary>` always.

**Project-specific conventions**:
- Query records: describe what data they request and for which entity.
- Response records: describe what information they return.
- Handler classes: reference the query type with `<see cref="..."/>`.
- Domain mutation methods (`internal`): document preconditions and thrown exceptions.
- Use `<see langword="null"/>` / `<see langword="true"/>` / `<see langword="false"/>` instead of plain text for C# keywords.
