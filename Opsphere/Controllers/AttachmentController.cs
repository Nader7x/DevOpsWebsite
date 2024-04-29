using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Opsphere.Dtos.Attachment;
using Opsphere.Interfaces;
using Opsphere.Mappers;

namespace Opsphere.Controllers;


[ApiController]
[Route("Opsphere/Attachment")]

public class AttachmentController(IUnitOfWork unitOfWork) : ControllerBase
{
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
}