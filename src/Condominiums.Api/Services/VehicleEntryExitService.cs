using AutoMapper;
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

    public VehicleEntryExitService(
        ILogger<VehicleEntryExitService> logger,
        IMapper mapper,
        IVehicleEntryExitStore vehicleEntryExitStore,
        IResidentService residentService
    )
    {
        _logger = logger;
        _mapper = mapper;
        _vehicleEntryExitStore = vehicleEntryExitStore;
        _residentService = residentService;
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

        ServiceResult vehicleExists = await _residentService.ValidateIfVehicleExistsAsync(createVehicleEntryExitDto.PlateNumber);

        if (!vehicleExists.Success) return vehicleExists;

        try
        {
            VehicleEntryExit newRecord = _mapper.Map<VehicleEntryExit>(createVehicleEntryExitDto);
            newRecord.CreatedBy = userName;
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
}
