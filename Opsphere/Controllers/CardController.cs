using Microsoft.AspNetCore.Mvc;
using Opsphere.Dtos.Card;
using Opsphere.Interfaces;
using Opsphere.Mappers;
using Status = Opsphere.Data.Models.Status;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var cardModel = await unitOfWork.CardRepository.GetByIdAsync(id);

        if (cardModel == null)
        {
            return NotFound();
        }

        return Ok(cardModel.ToCardDto());
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var cardModel = await unitOfWork.CardRepository.GetByIdAsync(id);

        if (cardModel == null)
        {
            return NotFound();
        }

        unitOfWork.CardRepository.DeleteAsync(cardModel);
        await unitOfWork.CompleteAsync();

        return Ok(cardModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCardDto cardDto)
    {
        var cardModel = await unitOfWork.CardRepository.GetByIdAsync(id);

        if (cardModel == null)
        {
            return NotFound();
        }

        cardModel.Title = cardDto.Title;
        cardModel.Description = cardDto.Description;
        cardModel.CommentSection = cardDto.Comment;

        if (cardDto.Status == "done")
            cardModel.Status = Status.Done;
        else if (cardDto.Status == "inprogress")
            cardModel.Status = Status.InProgress;
        else
            cardModel.Status = Status.Todo;
        
        
        unitOfWork.CardRepository.UpdateAsync(cardModel);
        await unitOfWork.CompleteAsync();

        return Ok(cardModel);

    }
}