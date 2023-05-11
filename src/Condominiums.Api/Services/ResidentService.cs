using AutoMapper;
using Condominiums.Api.Models.DTOs.Residents;
using Condominiums.Api.Models.DTOs.Vehicles;
using Condominiums.Api.Models.Entities;
using Condominiums.Api.Services.Base;
using Condominiums.Api.Stores;
using MongoDB.Driver;

namespace Condominiums.Api.Services;

/// <summary>
/// Defines the methods that allows to manage Residents.
/// </summary>
public interface IResidentService
{
    #region Resident operations

    /// <summary>
    /// Allows to delete a resident by Id.
    /// </summary>
    /// <param name="id">The resident's Id.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> DeleteAsync(string id);

    /// <summary>
    /// Allows to create a resident.
    /// </summary>
    /// <param name="createResidentDto">The resident's information.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> CreateAsync(CreateResidentDto createResidentDto);

    /// <summary>
    /// Allows to get a list with all the residents.
    /// </summary>
    /// <returns>Execution result with Resident information in Extra property if found.</returns>
    Task<ServiceResult<List<ResidentDto>>> GetAsync();

    /// <summary>
    /// Allows to get a resident by Id.
    /// </summary>
    /// <param name="id">The resident's Id.</param>
    /// <returns>Execution result with Resident information in Extra property if found.</returns>
    Task<ServiceResult<ResidentDto>> GetAsync(string id);

    /// <summary>
    /// Allows to check if a document exists by Id.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> ExistsByIdAsync(string id);

    /// <summary>
    /// Allows to update a resident.
    /// </summary>
    /// <param name="id">The resident's Id.</param>
    /// <param name="updateResidentDto">The resident's information to update.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> UpdateAsync(string id, UpdateResidentDto updateResidentDto);

    #endregion

    #region Vehicles operations

    /// <summary>
    /// Allows to obtain all the vehicles that belong to a resident by ID.
    /// </summary>
    /// <param name="id">Resident's id.</param>
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

    #endregion
}

/// <summary>
/// Implements the methods that allows to manage Residents.
/// </summary>
public partial class ResidentService : IResidentService
{
    private readonly ILogger<ResidentService> _logger;
    private readonly IMapper _mapper;
    private readonly IResidentStore _residentStore;

    public ResidentService(ILogger<ResidentService> logger, IMapper mapper, IResidentStore residentStore)
    {
        _logger = logger;
        _mapper = mapper;
        _residentStore = residentStore;
    }

    #region Resident operations

    public async Task<ServiceResult> CreateAsync(CreateResidentDto createResidentDto)
    {
        _logger.LogDebug("Attempting to create a resident.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(createResidentDto.ApartmentNumber))
        {
            errorMessage = "The 'apartment number' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(createResidentDto.Name))
        {
            errorMessage = "The 'name' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            Resident resident = _mapper.Map<Resident>(createResidentDto);
            await _residentStore.InsertOneAsync(resident);
            _logger.LogInformation($"The resident '{createResidentDto.Name}' was created.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error saving the resident.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> DeleteAsync(string id)
    {
        _logger.LogDebug("Attempting to delete a resident.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(id))
        {
            errorMessage = "The 'id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            Resident? resident = await _residentStore.GetByIdAsync(id);

            if (resident == null)
            {
                errorMessage = "Resident not found.";
                _logger.LogWarning(errorMessage);
                return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status404NotFound };
            }

            await _residentStore.DeleteOneAsync(id);
            _logger.LogInformation($"Resident '{resident.Name}' deleted.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error deleting the resident.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult<ResidentDto>> GetAsync(string id)
    {
        _logger.LogDebug("Attempting to get a resident.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(id))
        {
            errorMessage = "The 'id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult<ResidentDto>() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            Resident? resident = await _residentStore.GetByIdAsync(id);

            if (resident == null)
            {
                errorMessage = "Resident not found.";
                _logger.LogWarning(errorMessage);
                return new ServiceResult<ResidentDto>() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status404NotFound };
            }

            ResidentDto residentDto = _mapper.Map<ResidentDto>(resident);
            _logger.LogInformation($"Resident '{residentDto.Name}' found.");
            return new ServiceResult<ResidentDto>() { Extra = residentDto };
        }
        catch (Exception ex)
        {
            errorMessage = "Error searching the resident.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<ResidentDto>() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult<List<ResidentDto>>> GetAsync()
    {
        _logger.LogDebug("Attempting to get all the residents.");
        string? errorMessage = null;

        try
        {
            SortDefinition<Resident> sort = Builders<Resident>.Sort
                .Ascending(r => r.ApartmentNumber)
                .Ascending(r => r.Name);
            List<Resident> residents = await _residentStore.GetAllAsync(sort);
            List<ResidentDto> residentsDto = _mapper.Map<List<ResidentDto>>(residents);
            _logger.LogInformation($"All residents getted.");
            return new ServiceResult<List<ResidentDto>>() { Extra = residentsDto };
        }
        catch (Exception ex)
        {
            errorMessage = "Error getting residents.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<List<ResidentDto>>() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> ExistsByIdAsync(string id)
    {
        _logger.LogDebug("Attempting to check if resident exists by Id.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(id))
        {
            errorMessage = "The 'id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            bool exists = await _residentStore.ExistsByIdAsync(id);

            if (!exists)
            {
                errorMessage = "Resident not found.";
                _logger.LogWarning(errorMessage);
                return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status404NotFound };
            }

            _logger.LogInformation("Resident found.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error searching the resident.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<ResidentDto>() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> UpdateAsync(string id, UpdateResidentDto updateResidentDto)
    {
        _logger.LogDebug("Attempting to update a resident.");
        string? errorMessage = null;

        if (string.IsNullOrEmpty(id))
        {
            errorMessage = "The 'id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(updateResidentDto.ApartmentNumber))
        {
            errorMessage = "The 'apartment number' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(updateResidentDto.Name))
        {
            errorMessage = "The 'name' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            Resident? resident = await _residentStore.GetByIdAsync(id);

            if (resident == null)
            {
                errorMessage = "Resident not found.";
                _logger.LogWarning(errorMessage);
                return new ServiceResult() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status404NotFound };
            }

            resident.ApartmentNumber = updateResidentDto.ApartmentNumber;
            resident.Name = updateResidentDto.Name;
            await _residentStore.UpdateOneAsync(resident);
            _logger.LogInformation($"Resident '{resident.Name}' updated.");
            return new ServiceResult();
        }
        catch (Exception ex)
        {
            errorMessage = "Error updating the resident.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    #endregion

    #region Vehicles operations

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

        try
        {
            ServiceResult residentExists = await ExistsByIdAsync(createVehicleDto.ResidentId);

            if (!residentExists.Success) return residentExists;

            Vehicle vehicle = _mapper.Map<Vehicle>(createVehicleDto);
            await _residentStore.AddVehicleAsync(createVehicleDto.ResidentId, vehicle);
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

        try
        {
            ServiceResult residentExists = await ExistsByIdAsync(updateVehicleDto.ResidentId);

            if (!residentExists.Success) return residentExists;

            Vehicle vehicle = _mapper.Map<Vehicle>(updateVehicleDto);
            await _residentStore.UpdateVehicleAsync(
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
        string? errorMessage = null;

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
            ServiceResult residentExists = await ExistsByIdAsync(deleteVehicleDto.ResidentId);

            if (!residentExists.Success) return residentExists;

            await _residentStore.DeleteVehicleAsync(
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
            errorMessage = "The 'Resident Id' field is required.";
            _logger.LogWarning(errorMessage);
            return new ServiceResult<List<VehicleDto>>() { ErrorMessage = errorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            ServiceResult residentExists = await ExistsByIdAsync(id);

            if (!residentExists.Success) return new ServiceResult<List<VehicleDto>>()
            {
                ErrorMessage = residentExists.ErrorMessage,
                HttpStatusCode = residentExists.HttpStatusCode
            };

            List<Vehicle> vehicles = await _residentStore.GetVehiclesAsync(id);
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

    #endregion
}
