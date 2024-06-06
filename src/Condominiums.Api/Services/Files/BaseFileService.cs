
namespace Condominiums.Api.Services.Files;

public abstract class BaseFileService : IFileService
{
    public async Task SaveAsync(Stream stream, string path, CancellationToken cancellationToken = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using (FileStream fileStream = File.Create(path))
        {
            await stream.CopyToAsync(fileStream, cancellationToken);
        }
    }
}
