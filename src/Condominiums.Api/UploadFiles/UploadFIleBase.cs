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
/// Base class for file upload operations.
/// </summary>
/// <param name="BasePath">Base path to save the file.</param>
/// <param name="ValidExtensions">File extensions allowed to save.</param>
/// <param name="MaxSize">Max file size allowed to save in MB.</param>
public abstract class UploadFileBase(string BasePath, IReadOnlyCollection<string> ValidExtensions, short MaxSize) : IUploadFile
{
    public async Task<Result<string>> UploadAsync(FileStream fileStream, string extension, CancellationToken cancellationToken = default)
    {
        int maxSizeInBytes = MaxSize * 1024 * 1024; // Convert max size from MB to bytes

        if (fileStream.Length > maxSizeInBytes)
        {
            return Result<string>.Failure($"The max file size is {MaxSize} MB.");
        }

        if (string.IsNullOrWhiteSpace(extension) || !extension.StartsWith('.'))
        {
            return Result<string>.Failure("The file extension is invalid. It must start with a dot (.) and be followed by the file type.");
        }

        if (!ValidExtensions.Contains(extension.ToLowerInvariant()))
        {
            return Result<string>.Failure($"The file extension {extension} is not valid. Valid extensions are: {string.Join(", ", ValidExtensions)}.");
        }

        string fileName = Guid.NewGuid().ToString("N");
        string filePath = Path.Combine(BasePath, fileName + extension);

        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(BasePath);

            // Save the file to the specified path
            using (var fileStreamToSave = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(fileStreamToSave, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"An error occurred while uploading the file: {ex.Message}");
        }

        return Result<string>.Success(filePath);
    }
}
