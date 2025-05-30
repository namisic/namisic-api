/*
API de Nami SIC, la aplicación de código abierto que permite administrar Condominios fácilmente.
Copyright (C) 2025  Oscar David Díaz Fortaleché  lechediaz@gmail.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
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
