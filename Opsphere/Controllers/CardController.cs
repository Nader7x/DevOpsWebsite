using Microsoft.AspNetCore.Mvc;
using Opsphere.Dtos.Card;
using Opsphere.Interfaces;
using Opsphere.Mappers;

namespace Opsphere.Controllers;

[Route("Opsphere/card")]
[ApiController]
public class CardController : ControllerBase
{
    private readonly ICardRepository _cardRepo;

    public CardController(ICardRepository cardRepo)
    {
        _cardRepo = cardRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cards = await _cardRepo.GetAllAsync();
        var cardsDto = cards.Select( c => c.ToCardDto());

        return Ok(cardsDto);
    }

    [HttpPost("{projectId:int}")]
    public async Task<IActionResult> Create([FromRoute] int projectId , [FromBody] CreateCardDto cardDto)
    {
        //to-do check that the project exists via project Id
        var cardModel = cardDto.ToCardFromCreate(projectId);

        await _cardRepo.CreateAsync(cardModel);

        return Ok();
    }
}