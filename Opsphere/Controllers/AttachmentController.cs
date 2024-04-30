using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Opsphere.Data.Interfaces;
using Opsphere.Dtos.Attachment;
using Opsphere.Data.Interfaces;
using Opsphere.Mappers;

namespace Opsphere.Controllers;


[ApiController]
[Route("Opsphere/Attachment")]

public class AttachmentController(IUnitOfWork unitOfWork ,IWebHostEnvironment env, IFileProvider fileProvider) : ControllerBase
{
    private readonly IWebHostEnvironment _env = env;
    private readonly IFileProvider _fileProvider = fileProvider;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var attachmentModel = await unitOfWork.AttachmentRepository.GetByIdAsync(id);
        
        if (attachmentModel == null)
        {
            return NotFound();
        }

        return Ok(attachmentModel.ToAttachmentDto());
    }
    
    [HttpPost("{cardId}")]
    public async Task<IActionResult> Create([FromRoute] int cardId, [FromBody] CreateAttachmentDto attachmentDto)
    {
        var attachmentModel = attachmentDto.ToAttachmentFromCreate(cardId);
        await unitOfWork.AttachmentRepository.AddAsync(attachmentModel);
        await unitOfWork.CompleteAsync();
        return Ok();
    }
    [HttpPost]
    public IActionResult UploadFile(IFormFile file)
    {
        // Get the file path and name
        string filePath = Path.Combine(_env.ContentRootPath, "uploads", file.FileName);

        // Save the file to the file system
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Ok("File uploaded successfully!");
    }
}