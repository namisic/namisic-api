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
    /// Allows to validate if a resident exists by its document type and document number.
    /// Optionally an Id can be specyfied to ignore it.
    /// </summary>
    /// <param name="documentType">The resident's document type to search.</param>
    /// <param name="documentNumber">The resident's document number to search.</param>
    /// <param name="ignoreId">Optional resident's Id to ignore.</param>
    /// <returns>Execution resul with Extra value: True if the resident exist by its document type and document number.</returns>
    Task<ServiceResult<bool>> ExistsByDocumentAsync(string? documentType, string? documentNumber, string? ignoreId = null);

    /// <summary>
    /// Allows to create a resident.
    /// </summary>
    /// <param name="createResidentDto">The resident's information.</param>
    /// <returns>Execution result.</returns>
    Task<ServiceResult> CreateAsync(CreateResidentDto createResidentDto);

    /// <summary>
    /// Allows to get a list with all the residents.
    /// </summary>
    /// <param name="filters">Filters to apply.</param>
    /// <returns>Execution result with Resident information in Extra property if found.</returns>
    Task<ServiceResult<List<ResidentDto>>> GetAsync(GetResidentsQuery filters);

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

        ServiceResult<bool> existResident = await ExistsByDocumentAsync(createResidentDto.DocumentType, createResidentDto.DocumentNumber);

        if (!existResident.Success)
        {
            _logger.LogWarning(existResident.ErrorMessage);
            return new ServiceResult() { ErrorMessage = existResident.ErrorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
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

    public async Task<ServiceResult<List<ResidentDto>>> GetAsync(GetResidentsQuery filters)
    {
        _logger.LogDebug("Attempting to get all the residents.");
        string? errorMessage = null;

        try
        {
            List<Resident> residents = await _residentStore.GetAsync(filters);
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
            return new ServiceResult() { ErrorMessage = errorMessage };
        }
    }

    public async Task<ServiceResult> UpdateAsync(string id, UpdateResidentDto updateResidentDto)
    {
        _logger.LogDebug("Attempting to update a resident.");
        string? errorMessage;

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

        ServiceResult<bool> existResident = await ExistsByDocumentAsync(
            updateResidentDto.DocumentType,
            updateResidentDto.DocumentNumber,
            id
        );

        if (!existResident.Success)
        {
            _logger.LogWarning(existResident.ErrorMessage);
            return new ServiceResult() { ErrorMessage = existResident.ErrorMessage, HttpStatusCode = StatusCodes.Status400BadRequest };
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

            _mapper.Map(updateResidentDto, resident);
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

    public async Task<ServiceResult<bool>> ExistsByDocumentAsync(string? documentType, string? documentNumber, string? ignoreId = null)
    {
        string? errorMessage = null;
        _logger.LogDebug("Attempting to validate if a resident exists by its document type and document number.");

        try
        {
            bool exist = false;

            if (!string.IsNullOrEmpty(documentType) && !string.IsNullOrEmpty(documentNumber))
            {
                exist = await _residentStore.ExistsByDocumentAsync(documentType, documentNumber, ignoreId);
            }

            if (exist)
            {
                errorMessage = "Ya existe un residente con el mismo tipo de documento y número de documento.";
            }

            return new ServiceResult<bool>() { Extra = exist, ErrorMessage = errorMessage };
        }
        catch (Exception ex)
        {
            errorMessage = "Error while attempting to validate if a resident exists by its document type and document number.";
            _logger.LogError(ex, errorMessage);
            return new ServiceResult<bool>() { ErrorMessage = errorMessage };
        }
    }
}
