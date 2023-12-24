using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using Entities = Condominiums.Api.Models.Entities;

namespace Condominiums.Api.Stores.Settings.Base;

public abstract class BaseSettingsStore<TSettings> : ISettingsStore<TSettings>
    where TSettings : class
{
    protected BaseSettingsStore(string name, IMongoDatabase database)
    {
        Name = name;
        Collection = database.GetCollection<Entities.Settings>("settings");
    }

    public string Name { get; }
    public IMongoCollection<Entities.Settings> Collection { get; }

    public async Task<TSettings?> GetAsync()
    {
        FilterDefinition<Entities.Settings> filter = Builders<Entities.Settings>.Filter.Eq(s => s.Name, Name);
        Entities.Settings? dbSettings = await Collection.Find(filter).FirstOrDefaultAsync();

        if (dbSettings == null) return null;

        Type settinsType = typeof(TSettings);
        object settings = Activator.CreateInstance(settinsType)!;
        PropertyInfo[] properties = settinsType.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (dbSettings.Value.TryGetValue(property.Name, out BsonValue value))
            {
                property.SetValue(settings, value.ToString());
            }
        }

        return (TSettings)settings;
    }

    public Task UpdateAsync(TSettings settings)
    {
        FilterDefinition<Entities.Settings> filter = Builders<Entities.Settings>.Filter.Eq(s => s.Name, Name);
        Dictionary<string, string> values = settings.GetType()
            .GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(settings)!.ToString()!);
        var bsonDocument = new BsonDocument(values);
        UpdateDefinition<Entities.Settings> updateDefinition = Builders<Entities.Settings>.Update
            .Set(s => s.Value, bsonDocument);

        return Collection.UpdateOneAsync(filter, updateDefinition);
    }
}
