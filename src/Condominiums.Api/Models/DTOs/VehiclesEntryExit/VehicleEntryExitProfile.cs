using AutoMapper;
using Condominiums.Api.Models.Entities;

namespace Condominiums.Api.Models.DTOs.VehiclesEntryExit;

public class VehicleEntryExitProfile : Profile
{
    public VehicleEntryExitProfile()
    {
        CreateMap<CreateVehicleEntryExitDto, VehicleEntryExit>();
        CreateMap<VehicleEntryExit, VehicleEntryExitDto>();
    }
}
