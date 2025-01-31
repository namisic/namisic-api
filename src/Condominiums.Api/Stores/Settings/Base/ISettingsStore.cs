namespace Condominiums.Api.Stores.Settings.Base;

public interface ISettingsStore<TSettings> where TSettings : class
{
    string Name { get; }
    Task<TSettings?> GetAsync();
    Task InsertAsync(TSettings settings);
    Task UpdateAsync(TSettings settings);
}
