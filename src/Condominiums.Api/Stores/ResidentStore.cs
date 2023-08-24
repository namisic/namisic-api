using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Condominiums.Api.Stores;

/// <summary>
/// Defines the custom methods to perform the storage of residents.
/// </summary>
public interface IResidentStore : IStore<Resident>
{
    /// <summary>
    /// Allows to validate if a resident exists by its document type and document number.
    /// Optionally an Id can be specyfied to ignore it.
    /// </summary>
    /// <param name="documentType"></param>
    /// <param name="documentNumber"></param>
    /// <param name="ignoreId"></param>
    /// <returns></returns>
    Task<bool> ExistsByDocumentAsync(string documentType, string documentNumber, string? ignoreId = null);

    /// <summary>
    /// Allows to update a resident.
    /// </summary>
    /// <param name="resident">The resident information to update.</param>
    Task UpdateOneAsync(Resident resident);
}

/// <summary>
/// Implements the custom methods to perform the storage of residents.
/// </summary>
public class ResidentStore : StoreBase<Resident>, IResidentStore
{
    public ResidentStore(IMongoDatabase database) : base("residents", database)
    {

    }

    public async Task<bool> ExistsByDocumentAsync(string documentType, string documentNumber, string? ignoreId = null)
    {
        documentType = documentType.Trim().ToLower();
        documentNumber = documentType.Trim().ToLower();
        FilterDefinitionBuilder<Resident> filterBuilder = Builders<Resident>.Filter;
        FilterDefinition<Resident> baseFilter = filterBuilder.And(
            filterBuilder.Eq(f => f.DocumentType, documentType),
            filterBuilder.Eq(f => f.DocumentNumber, documentNumber)
        );
        FilterDefinition<Resident> filter = baseFilter;

        if (!string.IsNullOrEmpty(ignoreId))
        {
            ObjectId residentId = ObjectId.Parse(ignoreId);
            filter = filterBuilder.And(filterBuilder.Ne(f => f.Id, residentId), baseFilter);
        }

        long count = await Collection.CountDocumentsAsync(filter);
        return count > 0;
    }

    public Task UpdateOneAsync(Resident resident)
    {
        FilterDefinition<Resident> filter = Builders<Resident>.Filter.Eq(r => r.Id, resident.Id);
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .Set(r => r.Name, resident.Name)
            .Set(r => r.ApartmentNumber, resident.ApartmentNumber);
        return Collection.UpdateOneAsync(filter, update);
    }
}
