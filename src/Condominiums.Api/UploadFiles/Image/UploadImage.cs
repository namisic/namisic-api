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
using Condominiums.Api.Options;
using Microsoft.Extensions.Options;

namespace Condominiums.Api.UploadFiles.Image;

/// <summary>
/// Uploads images to the server.
/// </summary>
/// <param name="filesOptions">Files options configured.</param>
public class UploadImage(IOptions<FilesOptions> filesOptions) : UploadFileBase(filesOptions.Value.ImagesPath, s_validImageExtensions, filesOptions.Value.MaxSize), IUploadImage
{
    /// <summary>
    /// Valid image file extensions supported by the upload service.
    /// </summary>
    private static readonly IReadOnlyCollection<string> s_validImageExtensions =
    [
        ".jpg", ".jpeg", ".png", ".webp"
    ];
}
