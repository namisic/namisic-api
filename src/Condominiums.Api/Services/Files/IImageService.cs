using Condominiums.Api.Enums;

namespace Condominiums.Api.Services.Files;

public interface IImageService : IFileService
{
    Task SaveAsync(Stream stream, ImageType imageType, CancellationToken cancellationToken = default);
}
