using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Condominiums.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResidentController : ControllerBase
{
    private readonly IResidentStore _residentStore;

    public ResidentController(IResidentStore residentStore)
    {
        _residentStore = residentStore;
    }

    [HttpPost]
    public async Task Post(Resident newResident)
    {
        newResident.Id = ObjectId.GenerateNewId();
        await _residentStore.InsertOneAsync(newResident);
    }
}
