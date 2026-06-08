---
applyTo: "**/*.cs"
---

# XML Documentation Instructions

All C# code in this project must be documented in **Spanish** using standard .NET XML comments (`///`).

## General rules

- Always document: classes, interfaces, enums, records, delegates, public and internal methods, public and internal properties, and non-trivial constructors.
- Do not document: private fields, obvious access properties (`Id`, `Name` when the name is self-explanatory in context), and interface implementations that do not add their own semantics.
- Text should be clear and focused on the **why** or the **contract**, not on repeating the member name.
- Do not end `<summary>` descriptions with a period when it is a short nominal phrase; do use a period for complete sentences.

## Required tags by member type

### Classes, interfaces, enums, records, and delegates

```csharp
/// <summary>
/// Brief description of the type responsibility.
/// </summary>
```

Use `<remarks>` only when there is relevant additional information (invariants, usage constraints, applied patterns).

### Methods and constructors

```csharp
/// <summary>
/// Description of what the method does or what the constructor initializes.
/// </summary>
/// <param name="paramName">Descripción del parámetro.</param>
/// <returns>Descripción del valor de retorno (omitir si es void o Task sin valor).</returns>
/// <exception cref="ExceptionType">Condición bajo la cual se lanza.</exception>
```

- Include `<param>` for each non-obvious parameter.
- Include `<returns>` when the return type is not `void` or a valueless `Task`.
- Include `<exception>` only for exceptions that the method throws **intentionally** (do not list every possible runtime exception).

### Properties

```csharp
/// <summary>
/// Description of what the property represents.
/// </summary>
```

### Enum values

```csharp
/// <summary>
/// Description of the value meaning.
/// </summary>
```

## Project-specific patterns

### Queries and responses (Contracts)

Query records must describe **which data they request** and for which entity. Response records must describe **which information they return**.

```csharp
/// <summary>
/// Solicitud para obtener [descripción] por [criterio].
/// </summary>
/// <param name="ParamName">Descripción del parámetro del record.</param>
public record MiQuery(Guid Id) : IRequest<Result<MiResponse>>;

/// <summary>
/// Respuesta con [descripción de los datos devueltos].
/// </summary>
public record MiResponse(Guid Id, string Campo);
```

### Handlers (IRequestHandler)

```csharp
/// <summary>
/// Maneja la solicitud <see cref="MiQuery"/> y devuelve [descripción del resultado].
/// </summary>
```

### Domain entities

`internal` mutation methods must document **preconditions** and thrown exceptions:

```csharp
/// <summary>
/// Descripción de lo que modifica en la entidad.
/// </summary>
/// <param name="param">Descripción.</param>
/// <exception cref="ArgumentException">Cuando [condición].</exception>
```

### Validators (IValidator)

```csharp
/// <summary>
/// Valida que [descripción de las reglas que aplica].
/// </summary>
```

### Pipeline behaviors (IPipelineBehavior)

```csharp
/// <summary>
/// Comportamiento de canalización que [describe qué hace antes/después del handler].
/// </summary>
```

## Cross-reference usage

Use `<see cref="..."/>` to reference other project types or members when they add context:

```csharp
/// <summary>
/// Resultado devuelto por <see cref="MiQuery"/>.
/// </summary>
```

Use `<see langword="null"/>`, `<see langword="true"/>`, and `<see langword="false"/>` instead of plain text for C# keywords.
