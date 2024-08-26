using Api.App.Common.Consts;
using Api.App.Common.Controller;
using Api.App.Common.Exceptions;
using Api.App.Domain.Media.Handlers;
using Api.App.Domain.Media.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Api.App.Domain.Roaster.Models.Records;
using Api.App.Roaster.Handlers.Commands;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Roaster.Controllers;

[Area("Roasters")]
[Route("[area]/[controller]")]
public class FileController(IMessageBus bus, IFileSetter fileSetter) : ApiKeyController
{
    [HttpPost("{roasterId:guid}")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UploadFile(Guid roasterId)
    {
        using var stream = new MemoryStream();
        var file = GetFile();
        var extensions = ValidateExtensions(file);
        await file.CopyToAsync(stream);

        var result = await fileSetter.Upload(new UploadFileModel(roasterId, extensions, stream));
        var response = await bus.InvokeAsync<CoffeeRoasterUpdated>(new CommandUpdateRoasterCover(roasterId, result.ImageUrl, result.ThumbnailUrl));
        return Ok(response);
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

        var form = HttpContext.Request.Form.Files.FirstOrDefault();

        if (form == null)
        {
            throw new NotFoundException("File not exists in form");
        }

        return form;
    }
}