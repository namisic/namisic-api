using AutoMapper;
using Condominiums.Api.Models.Entities;

namespace Condominiums.Api.Models.DTOs.Residents;

public class ResidentsProfile : Profile
{
    public ResidentsProfile()
    {
        CreateMap<CreateResidentDto, Resident>();
    }
}
