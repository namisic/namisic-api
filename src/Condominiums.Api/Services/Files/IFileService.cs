namespace Condominiums.Api.Services.Files;

public interface IFileService
{
    Task SaveAsync(Stream stream, string path, CancellationToken cancellationToken = default);
}
