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
    public async Task<IActionResult> GetComments([FromRoute]int cardId)
    {
        var userName = User.GetUsername();
        var comments = await _unitOfWork.CardCommentRepository.GetCardCommentsAsync(cardId);
        if (comments != null)
        {
            var commentsDto = comments.Select(c => _mapper.Map<CardCommentDto>(c)).ToList();
            commentsDto.ForEach(c => c.UserName = userName);
            return Ok(commentsDto);
        }

        return NotFound();
    }
    [HttpPost("{cardId:int}")]
    public async Task<IActionResult> AddComment([FromRoute]int cardId,[FromBody] AddCommentDto commentDto)
    {
        var user = await _unitOfWork.UserRepository.Getbyusername(User.GetUsername());
        var comment = _mapper.Map<CardComment>(commentDto);
        if (user != null) comment.UserId = user.Id;
        comment.CardId = cardId;
        await _unitOfWork.CardCommentRepository.AddAsync(comment);
        await _unitOfWork.CompleteAsync();
        return Ok();
    }
}