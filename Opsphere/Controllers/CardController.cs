using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Opsphere.Data.Models;
using Opsphere.Data.Interfaces;
using Opsphere.Dtos.Card;
using Opsphere.Helpers;
using Opsphere.Mappers;
using Opsphere.Services;
using Status = Opsphere.Data.Models.Status;

namespace Opsphere.Controllers;

[Route("Opsphere/card"), ProducesResponseType(StatusCodes.Status200OK),
 ProducesResponseType(StatusCodes.Status401Unauthorized),
 ProducesResponseType(StatusCodes.Status500InternalServerError), ApiController]
public class CardController(IUnitOfWork unitOfWork) : ControllerBase
{

    private bool IsUserAllowedToEditCard(Card card)
    {
        var user = unitOfWork.UserRepository.Getbyusername(User.GetUsername());
        if (User.IsInRole("Developer") && card.AssignedDeveloperId != user.Id)
        {
            return false;
        }

        return true;
    }

    [HttpGet]
    [Authorize(Roles = "TeamLeader,Admin,Developer")]
    public async Task<IActionResult> GetAll()
    {
        var cards = await unitOfWork.CardRepository.GetAllAsync();
        var cardsDto = cards.Select(c => c.ToCardDto());
        return Ok(cardsDto);
    }

    [HttpGet("Card/{cardId:int}")]
    [Authorize(Roles = "TeamLeader,Admin")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById([FromRoute] int cardId)
    {
        var cardModel = await unitOfWork.CardRepository.GetByIdAsync(cardId);

        if (cardModel == null)
        {
            return NotFound();
        }

        return Ok(cardModel.ToCardDto());
    }

    [HttpGet("Developer/{devId:int}")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetDeveloperCards([FromRoute] int devId)
    {
        var cards = await unitOfWork.CardRepository.GetDeveloperCardsAsync(devId);
        var cardsDto = cards.Select(c => c.ToCardDto());
        return Ok(cardsDto);
    }

    [HttpPost("{projectId:int}")]
    [Authorize(Roles = "TeamLeader,Admin")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromRoute] int projectId, [FromBody] CreateCardDto cardDto)
    {
        var cardModel = cardDto.ToCardFromCreate(projectId);
        await unitOfWork.CardRepository.AddAsync(cardModel);
        await unitOfWork.CompleteAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "TeamLeader,Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

    [HttpPut("{id:int}")]
    [Authorize(Roles = "TeamLeader,Admin,Developer")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCardDto cardDto)
    {
        var user = await unitOfWork.UserRepository.Getbyusername(User.GetUsername());
        var cardModel = await unitOfWork.CardRepository.GetByIdAsync(id);

                                   
        if (cardModel == null)
        {
            return NotFound();
        }

        if (User.IsInRole("Developer") && cardModel.AssignedDeveloperId != user.Id)
        {
            return Forbid();
        }

        cardModel.Title = cardDto.Title;
        cardModel.Description = cardDto.Description;
        cardModel.Status = (Status)cardDto.Status;
        unitOfWork.CardRepository.UpdateAsync(cardModel);

        await unitOfWork.CompleteAsync();

        return Ok(cardModel);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "TeamLeader,Admin,Developer")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<Card>? patchDoc)
    {
        if (patchDoc != null)
        {
            var cardModel = await unitOfWork.CardRepository.GetByIdAsync(id);

            if (cardModel == null)
            {
                return NotFound();
            }

            if (!IsUserAllowedToEditCard(cardModel))
            {
                return Forbid();
            }

            if (patchDoc.Operations[0].path == "/AssignedDeveloperId")
            {
                if (cardModel.AssignedDeveloperId != null)
                {
                    return BadRequest("Cannot assign developer to a card that is already assigned");
                }
            }

            patchDoc.ApplyTo(cardModel, ModelState);

            await unitOfWork.CompleteAsync();

            return !ModelState.IsValid ? BadRequest(ModelState) : new ObjectResult(cardModel);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}