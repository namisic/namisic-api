using Condominiums.Api.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Condominiums.Api.Models.Entities;

public class Settings : IHasId
{
    public Settings(string name, BsonDocument value)
    {
        Name = name;
        Value = value;
    }

    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("value")]
    public BsonDocument Value { get; set; }
}
