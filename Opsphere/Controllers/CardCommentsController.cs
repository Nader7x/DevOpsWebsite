using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Dtos.CardCommnets;
using Opsphere.Helpers;

namespace Opsphere.Controllers;

[Authorize(Roles = "Admin,TeamLeader,Developer")]
[ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized),
 ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
[Route("Opsphere/CardComments")]
public class CardCommentsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpGet("CardComments/{cardId:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComments([FromRoute] int cardId)
    {
        var comments = await _unitOfWork.CardCommentRepository.GetCardCommentsAsync(cardId);
        if (comments != null)
        {
            var commentsDto = comments.Select(c =>
            {
                var dto = _mapper.Map<CardCommentDto>(c);
                dto.UserName = c?.User?.Username;
                return dto;
            }).ToList();

            return Ok(commentsDto);
        }

        return NotFound();
    }

    [HttpPost("{cardId:int}")]
    public async Task<IActionResult> AddComment([FromRoute] int cardId, [FromBody] AddCommentDto commentDto)
    {
        var comment = _mapper.Map<CardComment>(commentDto);
        comment.UserId = int.Parse(User.GetNameId() ?? string.Empty);
        comment.CardId = cardId;
        await _unitOfWork.CardCommentRepository.AddAsync(comment);
        await _unitOfWork.CompleteAsync();
        return Ok();
    }
}