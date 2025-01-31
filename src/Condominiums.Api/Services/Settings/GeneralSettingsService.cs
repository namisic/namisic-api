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
