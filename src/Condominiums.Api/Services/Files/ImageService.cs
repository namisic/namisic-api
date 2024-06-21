using Condominiums.Api.Enums;

namespace Condominiums.Api.Services.Files;

public class ImageService : BaseFileService, IImageService
{
    private readonly IReadOnlyDictionary<ImageType, string> _folders = new Dictionary<ImageType, string>
    {
        {ImageType.HomeBackground, "default/" },
    }.AsReadOnly();


    private readonly IReadOnlyDictionary<ImageType, string> _fileNames = new Dictionary<ImageType, string>
    {
        {ImageType.HomeBackground, "home_background" },
    }.AsReadOnly();

    public ImageService()
    {
    }

    public Task SaveAsync(Stream stream, ImageType imageType, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
