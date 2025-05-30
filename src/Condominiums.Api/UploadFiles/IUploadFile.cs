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
using Condominiums.Api.Base;

namespace Condominiums.Api.UploadFiles;

/// <summary>
/// Defines the method that allows to upload a file.
/// </summary>
public interface IUploadFile
{
    /// <summary>
    /// Uploads a file asynchronously.
    /// </summary>
    /// <param name="fileStream">The file stream to upload.</param>
    /// <param name="extension">The file extension, including the dot (e.g., ".jpg").</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the upload operation.</returns>
    Task<Result<string>> UploadAsync(FileStream fileStream, string extension, CancellationToken cancellationToken = default);
}
