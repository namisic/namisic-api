using AutoMapper;
using Condominiums.Api.Models.Entities;

namespace Condominiums.Api.Models.DTOs.Vehicles;

public class VehiclesProfile : Profile
{
    public VehiclesProfile()
    {
        CreateMap<CreateVehicleDto, Vehicle>();
        CreateMap<UpdateVehicleDto, Vehicle>();
        CreateMap<Vehicle, VehicleDto>();
    }
}
