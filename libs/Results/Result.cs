using System.Text.Json.Serialization;

namespace Results;

/// <summary>
/// Representa el resultado tipado de una operación.
/// </summary>
/// <typeparam name="T">Tipo del valor contenido en el resultado.</typeparam>
public class Result<T> : IResult
{
    /// <summary>
    /// Obtiene el valor devuelto por la operación.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Obtiene el tipo del valor encapsulado.
    /// </summary>
    [JsonIgnore] public Type ValueType => typeof(T);

    /// <summary>
    /// Obtiene el estado final de la operación.
    /// </summary>
    public ResultStatus Status { get; protected set; }

    /// <summary>
    /// Indica si la operación finalizó correctamente.
    /// </summary>
    public bool IsSuccess => Status == ResultStatus.Ok;

    /// <summary>
    /// Obtiene el mensaje descriptivo de éxito.
    /// </summary>
    public string? SuccessMessage { get; protected set; } = string.Empty;

    /// <summary>
    /// Obtiene el identificador de correlación asociado al resultado.
    /// </summary>
    public string? CorrelationId { get; protected set; } = string.Empty;

    /// <summary>
    /// Obtiene la colección de errores asociados a la operación.
    /// </summary>
    public IEnumerable<string> Errors { get; protected set; } = new List<string>();

    /// <summary>
    /// Obtiene los errores de validación detallados.
    /// </summary>
    public List<ResultValidationError> ValidationErrors { get; protected set; } = [];

    /// <summary>
    /// Inicializa una nueva instancia vacía de <see cref="Result{T}"/>.
    /// </summary>
    protected Result()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia con un valor exitoso.
    /// </summary>
    /// <param name="value">Valor devuelto por la operación.</param>
    public Result(T value)
    {
        Value = value;
    }

    /// <summary>
    /// Inicializa una nueva instancia con un valor y un mensaje de éxito.
    /// </summary>
    /// <param name="value">Valor devuelto por la operación.</param>
    /// <param name="successMessage">Mensaje descriptivo de éxito.</param>
    protected internal Result(T value, string successMessage)
        : this(value)
    {
        SuccessMessage = successMessage;
    }

    /// <summary>
    /// Inicializa una nueva instancia con un estado específico.
    /// </summary>
    /// <param name="status">Estado del resultado.</param>
    protected Result(ResultStatus status)
    {
        Status = status;
    }

    /// <summary>
    /// Convierte implícitamente un resultado en su valor contenido.
    /// </summary>
    /// <param name="result">Resultado a convertir.</param>
    /// <returns>Valor contenido en el resultado.</returns>
    public static implicit operator T(Result<T> result)
    {
        return result.Value!;
    }

    /// <summary>
    /// Convierte implícitamente un valor en un resultado exitoso.
    /// </summary>
    /// <param name="value">Valor a encapsular.</param>
    /// <returns>Resultado exitoso con el valor indicado.</returns>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    /// Convierte un resultado no tipado en un resultado tipado con el mismo estado y metadatos.
    /// </summary>
    /// <param name="result">Resultado de origen.</param>
    /// <returns>Resultado tipado equivalente.</returns>
    public static implicit operator Result<T?>(Result result)
    {
        return new Result<T?>(default(T))
        {
            Status = result.Status,
            Errors = result.Errors,
            SuccessMessage = result.SuccessMessage,
            CorrelationId = result.CorrelationId,
            ValidationErrors = result.ValidationErrors
        };
    }

    /// <summary>
    /// Obtiene el valor encapsulado como <see cref="object"/>.
    /// </summary>
    /// <returns>Valor contenido en el resultado.</returns>
    public object GetValue()
    {
        return Value!;
    }

    /// <summary>
    /// Convierte el resultado actual en un resultado paginado.
    /// </summary>
    /// <param name="pagedInfo">Información de paginación asociada.</param>
    /// <returns>Resultado paginado con el mismo contenido y metadatos.</returns>
    public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
    {
        return new PagedResult<T>(pagedInfo, Value!)
        {
            Status = Status,
            SuccessMessage = SuccessMessage,
            CorrelationId = CorrelationId,
            Errors = Errors,
            ValidationErrors = ValidationErrors
        };
    }

    /// <summary>
    /// Crea un resultado exitoso con el valor indicado.
    /// </summary>
    /// <param name="value">Valor de la operación.</param>
    /// <returns>Resultado exitoso.</returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    /// Crea un resultado exitoso con un mensaje descriptivo.
    /// </summary>
    /// <param name="value">Valor de la operación.</param>
    /// <param name="successMessage">Mensaje de éxito.</param>
    /// <returns>Resultado exitoso.</returns>
    public static Result<T> Success(T value, string successMessage)
    {
        return new Result<T>(value, successMessage);
    }

    /// <summary>
    /// Crea un resultado de error con uno o varios mensajes.
    /// </summary>
    /// <param name="errorMessages">Mensajes de error asociados.</param>
    /// <returns>Resultado en estado de error.</returns>
    public static Result<T> Error(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Error)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado inválido a partir de un error de validación.
    /// </summary>
    /// <param name="validationError">Error de validación asociado.</param>
    /// <returns>Resultado en estado inválido.</returns>
    public static Result<T> Invalid(ResultValidationError validationError)
    {
        return new Result<T>(ResultStatus.Invalid)
        {
            ValidationErrors = { validationError }
        };
    }

    /// <summary>
    /// Crea un resultado inválido a partir de varios errores de validación.
    /// </summary>
    /// <param name="validationErrors">Errores de validación asociados.</param>
    /// <returns>Resultado en estado inválido.</returns>
    public static Result<T> Invalid(params ResultValidationError[] validationErrors)
    {
        return new Result<T>(ResultStatus.Invalid)
        {
            ValidationErrors = [..validationErrors]
        };
    }

    /// <summary>
    /// Crea un resultado inválido a partir de una lista de errores de validación.
    /// </summary>
    /// <param name="validationErrors">Lista de errores de validación.</param>
    /// <returns>Resultado en estado inválido.</returns>
    public static Result<T> Invalid(List<ResultValidationError> validationErrors)
    {
        return new Result<T>(ResultStatus.Invalid)
        {
            ValidationErrors = validationErrors
        };
    }

    /// <summary>
    /// Crea un resultado en estado no encontrado.
    /// </summary>
    /// <returns>Resultado en estado no encontrado.</returns>
    public static Result<T> NotFound()
    {
        return new Result<T>(ResultStatus.NotFound);
    }

    /// <summary>
    /// Crea un resultado en estado no encontrado con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado no encontrado.</returns>
    public static Result<T> NotFound(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.NotFound)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado en estado prohibido.
    /// </summary>
    /// <returns>Resultado en estado prohibido.</returns>
    public static Result<T> Forbidden()
    {
        return new Result<T>(ResultStatus.Forbidden);
    }

    /// <summary>
    /// Crea un resultado en estado no autorizado.
    /// </summary>
    /// <returns>Resultado en estado no autorizado.</returns>
    public static Result<T> Unauthorized()
    {
        return new Result<T>(ResultStatus.Unauthorized);
    }

    /// <summary>
    /// Crea un resultado en estado de conflicto.
    /// </summary>
    /// <returns>Resultado en estado de conflicto.</returns>
    public static Result<T> Conflict()
    {
        return new Result<T>(ResultStatus.Conflict);
    }

    /// <summary>
    /// Crea un resultado en estado de conflicto con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado de conflicto.</returns>
    public static Result<T> Conflict(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Conflict)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado de error crítico con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado de error crítico.</returns>
    public static Result<T> CriticalError(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.CriticalError)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado en estado no disponible con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado no disponible.</returns>
    public static Result<T> Unavailable(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Unavailable)
        {
            Errors = errorMessages
        };
    }
}

/// <summary>
/// Representa un resultado no tipado de una operación.
/// </summary>
public class Result : Result<Result>
{
    /// <summary>
    /// Inicializa una nueva instancia vacía de <see cref="Result"/>.
    /// </summary>
    public Result()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia con un estado específico.
    /// </summary>
    /// <param name="status">Estado del resultado.</param>
    protected internal Result(ResultStatus status)
        : base(status)
    {
    }

    /// <summary>
    /// Crea un resultado exitoso sin valor.
    /// </summary>
    /// <returns>Resultado exitoso.</returns>
    public static Result Success()
    {
        return new Result();
    }

    /// <summary>
    /// Crea un resultado exitoso con un mensaje descriptivo.
    /// </summary>
    /// <param name="successMessage">Mensaje de éxito.</param>
    /// <returns>Resultado exitoso.</returns>
    public static Result SuccessWithMessage(string successMessage)
    {
        return new Result
        {
            SuccessMessage = successMessage
        };
    }

    /// <summary>
    /// Crea un resultado exitoso tipado.
    /// </summary>
    /// <typeparam name="T">Tipo del valor contenido.</typeparam>
    /// <param name="value">Valor de la operación.</param>
    /// <returns>Resultado exitoso tipado.</returns>
    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    /// Crea un resultado exitoso tipado con un mensaje descriptivo.
    /// </summary>
    /// <typeparam name="T">Tipo del valor contenido.</typeparam>
    /// <param name="value">Valor de la operación.</param>
    /// <param name="successMessage">Mensaje de éxito.</param>
    /// <returns>Resultado exitoso tipado.</returns>
    public static Result<T> Success<T>(T value, string successMessage)
    {
        return new Result<T>(value, successMessage);
    }

    /// <summary>
    /// Crea un resultado de error con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes de error.</param>
    /// <returns>Resultado en estado de error.</returns>
    public new static Result Error(params string[] errorMessages)
    {
        return new Result(ResultStatus.Error)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado de error con identificador de correlación.
    /// </summary>
    /// <param name="correlationId">Identificador de correlación.</param>
    /// <param name="errorMessages">Mensajes de error.</param>
    /// <returns>Resultado en estado de error.</returns>
    public static Result ErrorWithCorrelationId(string correlationId, params string[] errorMessages)
    {
        return new Result(ResultStatus.Error)
        {
            CorrelationId = correlationId,
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado inválido a partir de un error de validación.
    /// </summary>
    /// <param name="validationError">Error de validación asociado.</param>
    /// <returns>Resultado en estado inválido.</returns>
    public new static Result Invalid(ResultValidationError validationError)
    {
        return new Result(ResultStatus.Invalid)
        {
            ValidationErrors = { validationError }
        };
    }

    /// <summary>
    /// Crea un resultado inválido a partir de varios errores de validación.
    /// </summary>
    /// <param name="validationErrors">Errores de validación asociados.</param>
    /// <returns>Resultado en estado inválido.</returns>
    public new static Result Invalid(params ResultValidationError[] validationErrors)
    {
        return new Result(ResultStatus.Invalid)
        {
            ValidationErrors = [..validationErrors]
        };
    }

    /// <summary>
    /// Crea un resultado inválido a partir de una lista de errores de validación.
    /// </summary>
    /// <param name="validationErrors">Lista de errores de validación.</param>
    /// <returns>Resultado en estado inválido.</returns>
    public new static Result Invalid(List<ResultValidationError> validationErrors)
    {
        return new Result(ResultStatus.Invalid)
        {
            ValidationErrors = validationErrors
        };
    }

    /// <summary>
    /// Crea un resultado en estado no encontrado.
    /// </summary>
    /// <returns>Resultado en estado no encontrado.</returns>
    public new static Result NotFound()
    {
        return new Result(ResultStatus.NotFound);
    }

    /// <summary>
    /// Crea un resultado en estado no encontrado con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado no encontrado.</returns>
    public new static Result NotFound(params string[] errorMessages)
    {
        return new Result(ResultStatus.NotFound)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado en estado prohibido.
    /// </summary>
    /// <returns>Resultado en estado prohibido.</returns>
    public new static Result Forbidden()
    {
        return new Result(ResultStatus.Forbidden);
    }

    /// <summary>
    /// Crea un resultado en estado no autorizado.
    /// </summary>
    /// <returns>Resultado en estado no autorizado.</returns>
    public new static Result Unauthorized()
    {
        return new Result(ResultStatus.Unauthorized);
    }

    /// <summary>
    /// Crea un resultado en estado de conflicto.
    /// </summary>
    /// <returns>Resultado en estado de conflicto.</returns>
    public new static Result Conflict()
    {
        return new Result(ResultStatus.Conflict);
    }

    /// <summary>
    /// Crea un resultado en estado de conflicto con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado de conflicto.</returns>
    public new static Result Conflict(params string[] errorMessages)
    {
        return new Result(ResultStatus.Conflict)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado en estado no disponible con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado no disponible.</returns>
    public new static Result Unavailable(params string[] errorMessages)
    {
        return new Result(ResultStatus.Unavailable)
        {
            Errors = errorMessages
        };
    }

    /// <summary>
    /// Crea un resultado de error crítico con mensajes asociados.
    /// </summary>
    /// <param name="errorMessages">Mensajes asociados.</param>
    /// <returns>Resultado en estado de error crítico.</returns>
    public new static Result CriticalError(params string[] errorMessages)
    {
        return new Result(ResultStatus.CriticalError)
        {
            Errors = errorMessages
        };
    }
}