using Microsoft.AspNetCore.Mvc;
using Opsphere.Dtos.Card;
using Opsphere.Interfaces;
using Opsphere.Mappers;

namespace Opsphere.Controllers;

[Route("Opsphere/card")]
[ApiController]
public class CardController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cards = await unitOfWork.CardRepository.GetAllAsync();
        var cardsDto =  cards.Select( c => c.ToCardDto());
        return Ok(cardsDto);
    }
    [HttpPost("{projectId:int}")]
    public async Task<IActionResult> Create([FromRoute] int projectId , [FromBody] CreateCardDto cardDto)
    {
        //to-do check that the project exists via project Id
        var cardModel = cardDto.ToCardFromCreate(projectId);
        await unitOfWork.CardRepository.AddAsync(cardModel);
        await unitOfWork.CompleteAsync();
        return Ok();
    }
}