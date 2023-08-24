using AutoMapper;
using Condominiums.Api.Models.DTOs.Vehicles;
using Condominiums.Api.Models.Entities;
using Condominiums.Api.Services.Base;
using Condominiums.Api.Stores;

namespace Condominiums.Api.Services;

public interface IVehicleService
{
    /// <summary>
    /// Allows to obtain all the vehicles that belong to a vehicle by ID.
    /// </summary>
    /// <param name="id">Vehicle's id.</param>
    /// <returns>Execution result with vehicles in Extra property.</returns>
    Task<ServiceResult<List<VehicleDto>>> GetVehiclesAsync(string id);

    /// <summary>
    /// Allows to create a vehicle.
    /// </summary>
    /// <param name="createVehicleDto">Vehicle information.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> AddVehicleAsync(CreateVehicleDto createVehicleDto);

    /// <summary>
    /// Allows to update a vehicle.
    /// </summary>
    /// <param name="updateVehicleDto">Vehicle information.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> UpdateVehicleAsync(UpdateVehicleDto updateVehicleDto);

    /// <summary>
    /// Allows to delete a vehicle.
    /// </summary>
    /// <param name="deleteVehicleDto">Vehicle information.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> DeleteVehicleAsync(DeleteVehicleDto deleteVehicleDto);

    /// <summary>
    /// Allows to filter the plate numbers of vehicles given a portion of this.
    /// </summary>
    /// <param name="plateNumberHint">Plate number portion.</param>
    /// <returns>Execution result with a list of plate numbers in Extra property.</returns>
    Task<ServiceResult<List<string>>> FilterPlateNumbersAsync(string plateNumberHint);

    /// <summary>
    /// Validates if the vehicle plate number is unique.
    /// </summary>
    /// <param name="plateNumber">The license plate number to search.</param>
    /// <param name="ignoreId">Vehicle ID to ignore.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> ValidateUniqueVehiclePlateNumberAsync(string plateNumber, string? ignoreId = null);

    /// <summary>
    /// Allows to validate if a vehicle exists by searching for its license plate number.
    /// </summary>
    /// <param name="plateNumber">The license plate number to search.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> ValidateIfVehicleExistsAsync(string plateNumber);

    /// <summary>
    /// Allows to search a vehicle for its license plate number.
    /// </summary>
    /// <param name="plateNumber">The license plate number to search.</param>
    /// <returns>Execution result with the vehicle record in Extra property.</returns>
    Task<ServiceResult<VehicleDto>> GetVehicleByPlateNumberAsync(string plateNumber);
}

public class VehicleService : IVehicleService
{
    private readonly ILogger<VehicleService> _logger;
    private readonly IMapper _mapper;
    private readonly IVehicleStore _vehicleStore;
    private readonly IResidentService _residentService;

    public VehicleService(
        ILogger<VehicleService> logger,
        IMapper mapper,
        IVehicleStore vehicleStore,
        IResidentService residentService)
    {
        _logger = logger;
        _mapper = mapper;
        _vehicleStore = vehicleStore;
        _residentService = residentService;
    }

    public async Task<ServiceResult> AddVehicleAsync(CreateVehicleDto createVehicleDto)
    {
        _logger.LogDebug("Attempting to create a vehicle.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(createVehicleDto.ResidentId))
        {
            errorMessage = "The 'Resident Id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(createVehicleDto.Type))
        {
            errorMessage = "The 'Type' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(createVehicleDto.PlateNumber))
        {
            errorMessage = "The 'Plate Number' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        ServiceResult plateNumberIsUniqueResult = await ValidateUniqueVehiclePlateNumberAsync(createVehicleDto.PlateNumber);

        if (!plateNumberIsUniqueResult.Success) return plateNumberIsUniqueResult;

        try
        {
            ServiceResult vehicleExists = await _residentService.ExistsByIdAsync(createVehicleDto.ResidentId);

            if (!vehicleExists.Success) return vehicleExists;

            Vehicle vehicle = _mapper.Map<Vehicle>(createVehicleDto);
            await _vehicleStore.AddVehicleAsync(createVehicleDto.ResidentId, vehicle);
            _logger.LogInformation($"The vehicle '{createVehicleDto.PlateNumber}' was created.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error saving the vehicle.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> UpdateVehicleAsync(UpdateVehicleDto updateVehicleDto)
    {
        _logger.LogDebug("Attempting to update a vehicle.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(updateVehicleDto.ResidentId))
        {
            errorMessage = "The 'Resident Id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(updateVehicleDto.Type))
        {
            errorMessage = "The 'Type' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(updateVehicleDto.PlateNumber))
        {
            errorMessage = "The 'Plate Number' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        ServiceResult plateNumberIsUniqueResult = await ValidateUniqueVehiclePlateNumberAsync(
            updateVehicleDto.PlateNumber,
            updateVehicleDto.ResidentId
        );

        if (!plateNumberIsUniqueResult.Success) return plateNumberIsUniqueResult;

        try
        {
            ServiceResult vehicleExists = await _residentService.ExistsByIdAsync(updateVehicleDto.ResidentId);

            if (!vehicleExists.Success) return vehicleExists;

            Vehicle vehicle = _mapper.Map<Vehicle>(updateVehicleDto);
            await _vehicleStore.UpdateVehicleAsync(
                updateVehicleDto.ResidentId,
                updateVehicleDto.InitialPlateNumber,
                vehicle
            );
            _logger.LogInformation($"The vehicle '{updateVehicleDto.PlateNumber}' was updated.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error updating the vehicle.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> DeleteVehicleAsync(DeleteVehicleDto deleteVehicleDto)
    {
        _logger.LogDebug("Attempting to delete a vehicle.");
        string? errorMessage;
        if (string.IsNullOrEmpty(deleteVehicleDto.ResidentId))
        {
            errorMessage = "The 'Resident Id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(deleteVehicleDto.PlateNumber))
        {
            errorMessage = "The 'Plate Number' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            ServiceResult vehicleExists = await _residentService.ExistsByIdAsync(deleteVehicleDto.ResidentId);

            if (!vehicleExists.Success) return vehicleExists;

            await _vehicleStore.DeleteVehicleAsync(
                deleteVehicleDto.ResidentId,
                deleteVehicleDto.PlateNumber
            );
            _logger.LogInformation($"The vehicle '{deleteVehicleDto.PlateNumber}' was deleted.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error deleting the vehicle.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult<List<VehicleDto>>> GetVehiclesAsync(string id)
    {
        _logger.LogDebug("Attempting to get vehicles.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(id))
        {
            errorMessage = "The 'Vehicle Id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult<List<VehicleDto>>() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            ServiceResult vehicleExists = await _residentService.ExistsByIdAsync(id);

            if (!vehicleExists.Success) return new ServiceResult<List<VehicleDto>>()
            {
                ErrorMessage = vehicleExists.ErrorMessage,
                HttpStatusCode = vehicleExists.HttpStatusCode
            };

            List<Vehicle> vehicles = await _vehicleStore.GetVehiclesAsync(id);
            List<VehicleDto> vehiclesDto = _mapper.Map<List<VehicleDto>>(vehicles);
            _logger.LogInformation("Vehicles were getted.");
            return new ServiceResult<List<VehicleDto>>() { Extra = vehiclesDto };
        }
        catch (Exception ex)
        {
            errorMessage = "Error getting vehicles.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<List<VehicleDto>>() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult<List<string>>> FilterPlateNumbersAsync(string plateNumberHint)
    {
        _logger.LogDebug("Attempting to filter plate numbers.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(plateNumberHint))
        {
            errorMessage = "Please give a hint of the vehicle's license plate number.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult<List<string>>() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            List<string> plateNumbers = await _vehicleStore.FilterPlateNumbersAsync(plateNumberHint);
            _logger.LogInformation($"'{plateNumbers.Count}' plate numbers found for '{plateNumberHint}'.");
            return new ServiceResult<List<string>>() { Extra = plateNumbers };
        }
        catch (Exception ex)
        {
            errorMessage = "Error filtering plate numbers.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<List<string>>() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> ValidateUniqueVehiclePlateNumberAsync(string plateNumber, string? ignoreId = null)
    {
        _logger.LogDebug($"Attempting to validate if the vehicle plate number '{plateNumber}' is unique.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(plateNumber))
        {
            errorMessage = "Please indicate vehicle plate number.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            Resident? resident = await _vehicleStore.FindResidentByVehiclePlateNumberAsync(plateNumber, ignoreId);

            if (resident != null)
            {
                errorMessage = $"This vehicle license plate number is assigned to the resident '{resident.Name}' of the apartment or house '{resident.ApartmentNumber}'.";
                _logger.LogWarning(errorMessage);
                return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status404NotFound };
            }

            _logger.LogInformation($"The vehicle plate number '{plateNumber}' is unique.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = $"Error attempting to validate if the vehicle plate number '{plateNumber}' is unique.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> ValidateIfVehicleExistsAsync(string plateNumber)
    {
        _logger.LogDebug("Attempting to validate if the vehicle exists by plate number '{0}'.", plateNumber);
        string? errorMessage = null;

        if (string.IsNullOrEmpty(plateNumber))
        {
            errorMessage = "Please indicate vehicle plate number.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            bool exists = await _vehicleStore.ValidateIfVehicleExistsAsync(plateNumber);

            if (!exists)
            {
                errorMessage = "Vehicle not found.";
                _logger.LogWarning(errorMessage);
                return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status404NotFound };
            }

            _logger.LogInformation("The vehicle was found by '{0}' plate number.", plateNumber);
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = $"Error attempting to validate if the vehicle exists by plate number '{plateNumber}'.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult<VehicleDto>> GetVehicleByPlateNumberAsync(string plateNumber)
    {
        _logger.LogDebug("Attempting to get a vehicle by plate number '{0}'.", plateNumber);
        string? errorMessage = null;

        if (string.IsNullOrEmpty(plateNumber))
        {
            errorMessage = "Please indicate vehicle plate number.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult<VehicleDto>() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            Vehicle? vehicle = await _vehicleStore.GetVehicleByPlateNumberAsync(plateNumber);

            if (vehicle == null)
            {
                errorMessage = "Vehicle not found.";
                _logger.LogWarning(errorMessage);
                return new ServiceResult<VehicleDto>() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status404NotFound };
            }

            VehicleDto vehicleDto = _mapper.Map<VehicleDto>(vehicle);

            _logger.LogInformation("The vehicle was found by '{0}' plate number.", plateNumber);
            return new ServiceResult<VehicleDto>() { Extra = vehicleDto };
        }
        catch (Exception ex)
        {
            errorMessage = $"Error attempting to get a vehicle by plate number '{plateNumber}'.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<VehicleDto>() { ErrorMessage = errorMessage };
        }
    }
}