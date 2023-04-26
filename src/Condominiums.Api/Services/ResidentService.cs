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
    /// Allows to create a resident.
    /// </summary>
    /// <param name="createResidentDto">The resident's information.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> CreateAsync(CreateResidentDto createResidentDto);
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
}
