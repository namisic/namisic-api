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
using AutoMapper;
using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Options;
using Condominiums.Api.Stores.Settings;
using Microsoft.Extensions.Options;

namespace Condominiums.Api.Seeds.Settings;

public class GeneralSettingsSeed : IGeneralSettingsSeed
{
    private readonly IGeneralSettingsStore _generalSettingsStore;
    private readonly ILogger<GeneralSettingsSeed> _logger;
    private readonly IMapper _mapper;
    private readonly GeneralSettingsOptions _generalSettingsOptions;

    public GeneralSettingsSeed(
        IGeneralSettingsStore generalSettingsStore,
        ILogger<GeneralSettingsSeed> logger,
        IMapper mapper,
        IOptions<GeneralSettingsOptions> generalSettingsOptions
    )
    {
        _generalSettingsOptions = generalSettingsOptions.Value;
        _generalSettingsStore = generalSettingsStore;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task SeedAsync()
    {
        GeneralSettingsDto? generalSettingsDb = await _generalSettingsStore.GetAsync();

        if (generalSettingsDb == null)
        {
            _logger.LogInformation("Initializing '{name}' settings", _generalSettingsStore.Name);
            GeneralSettingsDto generalSettings = _mapper.Map<GeneralSettingsDto>(_generalSettingsOptions);
            await _generalSettingsStore.InsertAsync(generalSettings);
        }
    }
}
