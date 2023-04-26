using AutoMapper;
using Condominiums.Api.Models.DTOs.Residents;
using Condominiums.Api.Models.Entities;
using Condominiums.Api.Services.Base;
using Condominiums.Api.Stores;

namespace Condominiums.Api.Services;

/// <summary>
/// Defines the methods that allows to manage Residents.
/// </summary>
public interface IResidentService
{

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
    /// Allows to get a resident by Id.
    /// </summary>
    /// <param name="id">The resident's Id.</param>
    /// <returns>Execution result with Resident information in Extra property if found.</returns>
    Task<ServiceResult<ResidentDto>> GetAsync(string id);

    /// <summary>
    /// Allows to update a resident.
    /// </summary>
    /// <param name="id">The resident's Id.</param>
    /// <param name="updateResidentDto">The resident's information to update.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> UpdateAsync(string id, UpdateResidentDto updateResidentDto);
}

/// <summary>
/// Implements the methods that allows to manage Residents.
/// </summary>
public class ResidentService : IResidentService
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
}
