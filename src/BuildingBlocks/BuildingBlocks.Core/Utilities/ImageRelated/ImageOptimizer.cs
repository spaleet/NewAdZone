using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace BuildingBlocks.Core.Utilities.ImageRelated;

public class ImageOptimizer
{
    public void ImageResizer(string inputImagePath, string outputImagePath, int? width, int? height)
    {
        int customWidth = width ?? 100;

        int customHeight = height ?? 100;

        using var image = Image.Load(inputImagePath);
        image.Mutate(x => x.Resize(customWidth, customHeight));

        image.Save(outputImagePath, new JpegEncoder { Quality = 100 });
    }
}