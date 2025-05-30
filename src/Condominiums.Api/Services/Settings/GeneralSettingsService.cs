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
using Condominiums.Api.Exceptions;
using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Stores.Settings;

namespace Condominiums.Api.Services.Settings;

public class GeneralSettingsService : IGeneralSettingsService
{
    private readonly IGeneralSettingsStore _generalSettingsStore;
    private readonly IMapper _mapper;

    public GeneralSettingsService(IGeneralSettingsStore generalSettingsStore, IMapper mapper)
    {
        _generalSettingsStore = generalSettingsStore;
        _mapper = mapper;
    }

    public async Task<GeneralSettingsDto> GetAsync()
    {
        GeneralSettingsDto? generalSettings = await _generalSettingsStore.GetAsync();

        if (generalSettings == null)
        {
            throw new NotFoundException("General settings not found.");
        }

        return generalSettings!;
    }

    public async Task UpdateAsync(UpdateGeneralSettingsDto updateGeneralSettingsDto)
    {
        GeneralSettingsDto? generalSettingsDb = await _generalSettingsStore.GetAsync();

        if (generalSettingsDb == null)
        {
            throw new NotFoundException("General settings not found.");
        }

        _mapper.Map(updateGeneralSettingsDto, generalSettingsDb);

        await _generalSettingsStore.UpdateAsync(generalSettingsDb!);
    }
}
