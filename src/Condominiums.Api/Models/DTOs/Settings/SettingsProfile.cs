using AutoMapper;
using Condominiums.Api.Options;

namespace Condominiums.Api.Models.DTOs.Settings;

public class SettingsProfile : Profile
{
    public SettingsProfile()
    {
        CreateMap<GeneralSettingsOptions, GeneralSettingsDto>();
    }
}
