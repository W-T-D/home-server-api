using HomeServer.Api.Models.Files;
using HomeServer.Models.Files;
using HomeServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeServer.Api.Controllers;

[Route("[controller]")]
public class FilesController(IFilesService filesService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload(FileUploadRequest request, CancellationToken ctx)
    {
        await using var stream = request.File.OpenReadStream();
        var ms = new MemoryStream();
        await stream.CopyToAsync(ms, ctx);
        
        var file = new FileDto
        {
            Data = ms,
            Info = new FileInfoDto
            {
                Name = request.File.FileName,
                ContentType = request.File.ContentType
            }
        };

        var fileInfoResult = await filesService.UploadAsync(file, ctx);

        if (fileInfoResult.IsFailure)
        {
            return BadRequest(fileInfoResult.Error);
        }
        
        return Ok(fileInfoResult.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList(SearchFileRequest request, CancellationToken ctx)
    {
        var filesResult = await filesService.GetFileListAsync(SearchFileRequest.ToDto(request), ctx);

        if (filesResult.IsFailure)
        { 
            return BadRequest(filesResult.Error);
        }

        return Ok(filesResult.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Download(Guid id, CancellationToken ctx)
    {
        var fileResult = await filesService.GetByIdAsync(id, ctx);

        if (fileResult.IsFailure)
        {
            return BadRequest(fileResult.Error);
        }
        
        return File(fileResult.Value.Data.ToArray(), fileResult.Value.Info.ContentType, fileResult.Value.Info.Name);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ctx)
    {
        var result = await filesService.DeleteAsync(id, ctx);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}