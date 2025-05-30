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
using Condominiums.Api.Models.Base;
using MongoDB.Driver;

namespace Condominiums.Api.Stores.Base;

/// <summary>
/// Defines the general methods to perform the storage of an entity.
/// </summary>
/// <typeparam name="TCollection">The entity type.</typeparam>
public interface IStore<TCollection> where TCollection : IHasId
{
    /// <summary>
    /// Allows to get all documents of entity type.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>The document of entity type.</returns>
    Task<List<TCollection>> GetAllAsync(SortDefinition<TCollection>? sort = null);

    /// <summary>
    /// Allows to get a document of entity type by Id.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>The document of entity type.</returns>
    Task<TCollection?> GetByIdAsync(string id);

    /// <summary>
    /// Allows to check if a document exists by Id.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns><see langword="true"/> when exists, otherwise <see langword="false"/>.</returns>
    Task<bool> ExistsByIdAsync(string id);

    /// <summary>
    /// Allows to insert one document of entity type.
    /// </summary>
    /// <param name="document">The entity type.</param>
    Task InsertOneAsync(TCollection document);

    /// <summary>
    /// Allows to delete a document of entity type by Id.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>Task result.</returns>
    Task DeleteOneAsync(string id);
}
