namespace GameStore.Api.Shared.FileUpload;

public class FileUploader(
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor,
    ILogger<FileUploader> logger)
{
    public void DeleteFile(string? fileUrl, string folder)
    {
        var pathPrefix = $"/{folder}/";
        if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            !uri.AbsolutePath.StartsWith(pathPrefix, StringComparison.Ordinal))
        {
            logger.LogDebug("Skipping image outside the upload folder: {ImageUri}", fileUrl);
            return;
        }

        var fileName = uri.AbsolutePath[pathPrefix.Length..];
        var extension = Path.GetExtension(fileName);
        if (fileName != Path.GetFileName(fileName) ||
            !Guid.TryParseExact(Path.GetFileNameWithoutExtension(fileName), "D", out _) ||
            extension is not (".jpg" or ".jpeg" or ".png"))
        {
            logger.LogWarning("Skipping image with an unmanaged file name: {ImageUri}", fileUrl);
            return;
        }

        File.Delete(Path.Combine(environment.WebRootPath, folder, fileName));
    }

    public async Task<FileUploadResult> UploadFileAsync(
        IFormFile file,
        string folder)
    {

        var result = new FileUploadResult();

        if (file == null || file.Length == 0)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "No file uploaded";
            return result;
        }

        if (file.Length > 10 * 1024 * 1024)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "File is too large";
            return result;
        }

        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(fileExtension) ||
            !permittedExtensions.Contains(fileExtension))
        {
            result.IsSuccess = false;
            result.ErrorMessage = "Invalid file type";
            return result;
        }

        var uploadFolder = Path.Combine(environment.WebRootPath, folder);

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var safeFileName = $"{Guid.NewGuid()}{fileExtension}";

        var fullPath = Path.Combine(uploadFolder, safeFileName);

        using var stream = new FileStream(fullPath, FileMode.Create);

        await file.CopyToAsync(stream);

        var httpContext = httpContextAccessor.HttpContext;

        var fileUrl = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}/{folder}/{safeFileName}";


        result.IsSuccess = true;
        result.FileUrl = fileUrl;
        return result;

    }
}
