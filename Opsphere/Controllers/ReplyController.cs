using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Dtos.ReplyDto;
using Opsphere.Helpers;

namespace Opsphere.Controllers;

[Route("Opsphere/reply"),  ApiController]

public class ReplyController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    [HttpPost("{commentId:int}")]
    public async Task<IActionResult> Create([FromRoute] int commentId, AddReplyDto replyDto)
    {
        var user = await _unitOfWork.UserRepository.Getbyusername(User.GetUsername());
        var reply = _mapper.Map<Reply>(replyDto);
        if (user != null) reply.UserId = user.Id;
        reply.CardCommentId = commentId;
        await _unitOfWork.ReplyRepository.AddAsync(reply);
        await _unitOfWork.CompleteAsync();
        return Ok();
    }
}