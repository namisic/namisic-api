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
using Condominiums.Api.Seeds.Base;
using Condominiums.Api.Seeds.Settings;

namespace Condominiums.Api.Seeds;

public class Farmer : IFarmer
{
    private readonly ILogger<Farmer> _logger;
    private readonly IGeneralSettingsSeed _generalSettingsSeed;

    public Farmer(ILogger<Farmer> logger, IGeneralSettingsSeed generalSettingsSeed)
    {
        _logger = logger;
        _generalSettingsSeed = generalSettingsSeed;
    }

    public async Task PlantAsync()
    {
        _logger.LogTrace("Seeding started.");

        try
        {
            await _generalSettingsSeed.SeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error while seeding: {error}{ln}Details: {details}", ex.Message, Environment.NewLine, ex.StackTrace);
        }

        _logger.LogTrace("Seeding finished.");
    }
}
