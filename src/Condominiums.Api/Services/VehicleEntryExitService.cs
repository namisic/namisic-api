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
using Condominiums.Api.Models.DTOs.Vehicles;
using Condominiums.Api.Models.DTOs.VehiclesEntryExit;
using Condominiums.Api.Models.Entities;
using Condominiums.Api.Services.Base;
using Condominiums.Api.Stores;

namespace Condominiums.Api.Services;

/// <summary>
/// Defines the methods that allows to manage vehicle entry or exit.
/// </summary>
public interface IVehicleEntryExitService
{
    /// <summary>
    /// Allows to create a record of vehicle entry or exit.
    /// </summary>
    /// <param name="createVehicleEntryExitDto">The event information.</param>
    /// <param name="userName">name of the user who performed the action.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> CreateAsync(CreateVehicleEntryExitDto createVehicleEntryExitDto, string userName);

    /// <summary>
    /// Allows to query vehicle entry and exit records by using filters.
    /// </summary>
    /// <param name="filters">filters to be performed in the query.</param>
    /// <returns>Records of vehicle entries and exits matching the given filters.</returns>
    Task<ServiceResult<List<VehicleEntryExitDto>>> FilterAsync(VehicleEntryExitFilters filters);
}

/// <summary>
/// Implements the methods that allows to manage vehicle entry or exit.
/// </summary>
public class VehicleEntryExitService : IVehicleEntryExitService
{
    private readonly ILogger<VehicleEntryExitService> _logger;
    private readonly IMapper _mapper;
    private readonly IVehicleEntryExitStore _vehicleEntryExitStore;
    private readonly IResidentService _residentService;
    private readonly IVehicleService _vehicleService;

    public VehicleEntryExitService(
        ILogger<VehicleEntryExitService> logger,
        IMapper mapper,
        IVehicleEntryExitStore vehicleEntryExitStore,
        IResidentService residentService,
        IVehicleService vehicleService
    )
    {
        _logger = logger;
        _mapper = mapper;
        _vehicleEntryExitStore = vehicleEntryExitStore;
        _residentService = residentService;
        _vehicleService = vehicleService;
    }

    public async Task<ServiceResult> CreateAsync(CreateVehicleEntryExitDto createVehicleEntryExitDto, string userName)
    {
        _logger.LogDebug("Attempting to create a record of vehicle entry or exit.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(createVehicleEntryExitDto.PlateNumber))
        {
            errorMessage = "The 'plate number' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(createVehicleEntryExitDto.Type))
        {
            errorMessage = "The 'type' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        ServiceResult<VehicleDto> vehicleByPlateNumberResult = await _vehicleService.GetVehicleByPlateNumberAsync(createVehicleEntryExitDto.PlateNumber);

        if (!vehicleByPlateNumberResult.Success) return vehicleByPlateNumberResult;

        try
        {
            VehicleDto vehicle = vehicleByPlateNumberResult.Extra!;
            VehicleEntryExit newRecord = _mapper.Map<VehicleEntryExit>(createVehicleEntryExitDto);
            newRecord.CreatedBy = userName;
            newRecord.VehicleType = vehicle.Type;
            await _vehicleEntryExitStore.InsertOneAsync(newRecord);
            _logger.LogInformation("The record of vehicle entry or exit was created.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error saving the record of vehicle entry or exit.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult<List<VehicleEntryExitDto>>> FilterAsync(VehicleEntryExitFilters filters)
    {
        _logger.LogDebug("Attempting query vehicle entry or exit records.");

        try
        {
            List<VehicleEntryExit> records = await _vehicleEntryExitStore.FilterAsync(filters);
            List<VehicleEntryExitDto> recordsMapped = _mapper.Map<List<VehicleEntryExitDto>>(records);
            _logger.LogInformation("Vehicle entry and exit records correctly consulted.");
            return new ServiceResult<List<VehicleEntryExitDto>>() { Extra = recordsMapped };
        }
        catch (Exception ex)
        {
            string errorMessage = "Error querying vehicle entry or exit records.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<List<VehicleEntryExitDto>>() { ErrorMessage = errorMessage };
        }
    }
}
