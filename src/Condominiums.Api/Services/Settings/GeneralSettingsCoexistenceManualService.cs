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
using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Options;
using Condominiums.Api.Stores.Settings;
using Condominiums.Api.UploadFiles.CoexistenceManual;
using Microsoft.Extensions.Options;

namespace Condominiums.Api.Services.Settings;

public class GeneralSettingsCoexistenceManualService : IGeneralSettingsCoexistenceManualService
{
    private readonly IUploadCoexistenceManual _uploadCoexistenceManual;
    private readonly IGeneralSettingsStore _generalSettingsStore;
    private readonly FilesOptions _filesOptions;

    public GeneralSettingsCoexistenceManualService(
        IUploadCoexistenceManual uploadCoexistenceManual,
        IOptions<FilesOptions> filesOptions,
        IGeneralSettingsStore generalSettingsStore)
    {
        _uploadCoexistenceManual = uploadCoexistenceManual;
        _filesOptions = filesOptions.Value;
        _generalSettingsStore = generalSettingsStore;
    }

    public async Task<Result<string>> UploadCoexistenceManualAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default)
    {
        var uploadResult = await _uploadCoexistenceManual.UploadAsync(fileStream, extension, cancellationToken);

        if (uploadResult.IsFailure || string.IsNullOrEmpty(uploadResult.Value))
        {
            return Result<string>.Failure(uploadResult.Error ?? "Manual upload failed.");
        }

        string relativePath = uploadResult.Value.Replace("\\", "/");
        if (relativePath.StartsWith("/")) relativePath = relativePath[1..];
        string publicUri = $"{_filesOptions.StorageUri.TrimEnd('/')}/{relativePath.TrimStart('/')}";

        var generalSettings = await _generalSettingsStore.GetAsync() ?? new GeneralSettingsDto();
        generalSettings.CondominiumCoexistenceManualPath = publicUri;
        await _generalSettingsStore.UpdateAsync(generalSettings);

        return Result<string>.Success(publicUri);
    }
}
