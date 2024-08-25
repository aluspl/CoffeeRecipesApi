using Api.App.Common.Consts;
using Api.App.Common.Controller;
using Api.App.Common.Exceptions;
using Api.App.Domain.Media.Handlers.Results;
using Api.App.Domain.Roaster.Models;
using Api.App.Media.Handlers.Commands;
using Api.App.Roaster.Handlers.Commands;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Roaster.Controllers;

[Area("Roasters")]
[Route("[area]/[controller]")]
public class FileController(IMessageBus bus) : ApiKeyController
{
    [HttpPost("{roasterId:guid}")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UploadFile(Guid roasterId)
    {
        var file = GetFile();
        var extensions = ValidateExtensions(file);
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        
        var result = await bus.InvokeAsync<FileUploaded>(new CommandUploadRoasterFile(roasterId, extensions, stream.ToArray()));
        var response = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandUpdateRoasterCover(roasterId, result.Id));
        return Ok(result);
    }

    private static string ValidateExtensions(IFormFile file)
    {
        if (file == null)
        {
            throw new BusinessException("File must not be NUll");
        }
        var extensions = Path.GetExtension(file.FileName);
        if (extensions != FileConstants.AllowedImageFormat)
        {
            throw new BusinessException("Image extensions must be png");
        }
        
        return extensions;
    }
    
    private IFormFile GetFile()
    {
        if (!HttpContext.Request.HasFormContentType)
        {
            throw new NotFoundException("File not exists in form");
        }

        IFormFile form = HttpContext.Request.Form.Files.FirstOrDefault();

        if (form == null)
        {
            throw new NotFoundException("File not exists in form");
        }

        return form;
    }
}