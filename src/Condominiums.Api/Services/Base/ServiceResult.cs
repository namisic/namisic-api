namespace Condominiums.Api.Services.Base;

/// <summary>
/// Represents a execution result.
/// </summary>
public class ServiceResult
{
    private bool _success = false;
    private int _httpStatusCode = StatusCodes.Status500InternalServerError;

    /// <summary>
    /// Indicates if execution was successfull. Default is <see langword="false"/>.
    /// </summary>
    public bool Success { get => string.IsNullOrEmpty(ErrorMessage); }

    /// <summary>
    /// Http status code that could represent the execution result. If Success property is <see langword="true"/>
    /// then is considered as HTTP Status 200 OK.
    /// </summary>
    public int HttpStatusCode { get => _success ? StatusCodes.Status200OK : _httpStatusCode; set => _httpStatusCode = value; }

    /// <summary>
    /// Error message to show when execution fails. Default <see langword="null"/>.
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Represents a execution result.
/// </summary>
/// <typeparam name="TExtra">Extra information type.</typeparam>
public class ServiceResult<TExtra> : ServiceResult
{
    /// <summary>
    /// Extra information.
    /// </summary>
    public TExtra? Extra { get; set; }
}
