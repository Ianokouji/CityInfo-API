using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        // Injects the fileExtensionContentProvider to the method as a dependency
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet]
        public IActionResult GetFile(int id) 
        {
            // Getting the file from the directory
            string pathTofile = "ServerFiles/gojo.png";

            // Check wether the file exists
            if (!System.IO.File.Exists(pathTofile))
            {
                return NotFound();
            }

            // Allows us to read the content type of the file and dynamically use it for the response
            if (!_fileExtensionContentTypeProvider.TryGetContentType(pathTofile, out var contentType)) 
            {
                contentType = "application/octet-stream";
            }

            // Reads file then returns as output
            var bytes = System.IO.File.ReadAllBytes(pathTofile);
            return File(bytes,contentType, Path.GetFileName(pathTofile));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile (IFormFile file)
        {
            // Perform validation
            if(file.Length == 0 | file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or invalid file has been uploaded!");
            }

            // WARNING: UNSAFE CODE - Always store files in secure no-run storages
            // Secure with designated path name
            // Secure with designated extension name
            string path = Path.Combine(Directory.GetCurrentDirectory(),"ServerFiles" ,$"uploaded_file_{Guid.NewGuid}.pdf");

            // Copy file content to newly created file that can be stored in the server
            // WARNING: STILL UNSAFE
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            };

            return Ok("Your file has been uploaded successfully!");


        }
    }
}
