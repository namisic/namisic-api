namespace Condominiums.Api.Stores.Settings.Base;

public interface ISettingsStore<TSettings> where TSettings : class
{
    Task<TSettings?> GetAsync();
    Task UpdateAsync(TSettings settings);
}
