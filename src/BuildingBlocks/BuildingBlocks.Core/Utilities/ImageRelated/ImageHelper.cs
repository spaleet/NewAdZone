using BuildingBlocks.Core.Exceptions.Base;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Core.Utilities.ImageRelated;

public static class ImageHelper
{
    /// <summary>
    /// Uploads Image to provided path
    /// </summary>
    /// <param name="file"></param>
    /// <param name="path">upload path</param>
    /// <param name="name">custom file name</param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns>New File Name</returns>
    public static string UploadImage(this IFormFile file, string path, string? name = null, int? width = null, int? height = null)
    {
        if (file is null || !file.IsImage())
            throw new BadRequestException("فایل مشکل دارد");

        // create upload directory
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // create image file title
        string fileName = file.GenerateImagePath();

        if (!string.IsNullOrEmpty(name))
            fileName = name;
        // TODO = auto remove file extention

        if (width == null && height == null)
        {
            // create upload path
            string uploadPath = path + fileName;

            // upload
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
        else
        {
            // create temp upload path
            string tempPath = path + "temp" + fileName;

            // upload temp
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // resize
            var resizer = new ImageOptimizer();

            // create upload path
            string uploadPath = path + fileName;

            // crop image
            resizer.ImageResizer(tempPath, uploadPath, width, height);

            // delete temp file
            File.Delete(tempPath);

            return fileName;
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