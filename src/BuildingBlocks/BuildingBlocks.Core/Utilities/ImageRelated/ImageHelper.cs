using BuildingBlocks.Core.Utilities.ImageRelated;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BuildingBlocks.Core.Utilities.ImageRelated;

public static class ImageHelper
{
    public static async Task<bool> UploadImage(this IFormFile file, string path, string? name, int? width, int? height)
    {
        if (file is null || !file.IsImage())
            return false;

        // create upload directory
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // create image file title
        string fileName = file.GenerateImagePath();

        if (!string.IsNullOrEmpty(name))
            fileName = name;

        if (width == null && height == null)
        {
            // create upload path
            string uploadPath = path + fileName;

            // upload
            using var stream = new FileStream(uploadPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return true;
        } else
        {
            // create temp upload path
            string tempPath = path + "temp" + fileName;

            // upload temp
            using var stream = new FileStream(tempPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // resize
            var resizer = new ImageOptimizer();

            // create upload path
            string uploadPath = path + fileName;

            // crop image
            resizer.ImageResizer(tempPath, uploadPath, width, height);

            // delete temp file
            File.Delete(tempPath);

            return true;
        }

    }

    public static void DeleteImage(string fileName, string path)
    {
        // create full path
        string fullPath = path + fileName;

        if (!string.IsNullOrEmpty(fileName))
        {
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }

    public static string ConvertToBase64(string imgPath)
    {
        byte[] imageArray = File.ReadAllBytes(imgPath);
        string base64ImageRepresentation = Convert.ToBase64String(imageArray);
        string contentType = FileHelper.GetMimeType(Path.GetExtension(imgPath));

        return $"data:{contentType};base64,{base64ImageRepresentation}";
    }
}