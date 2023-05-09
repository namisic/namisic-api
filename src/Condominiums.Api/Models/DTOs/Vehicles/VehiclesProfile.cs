using AutoMapper;
using Condominiums.Api.Models.Entities;

namespace Condominiums.Api.Models.DTOs.Vehicles;

public class VehiclesProfile : Profile
{
    public VehiclesProfile()
    {
        CreateMap<CreateVehicleDto, Vehicle>();
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.Id, config => config.MapFrom(src => src.Id.ToString()));
    }
}
