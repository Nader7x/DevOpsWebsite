using Opsphere.Data.Models;
using Opsphere.Dtos.Card;
using Status = Opsphere.Dtos.Card.Status;

namespace Opsphere.Mappers;

public static class CardMapper
{
    public static CardDto ToCardDto(this Card cardModel)
    {
        return new CardDto
        {
            CardId = cardModel.Id,
            Title = cardModel.Title,
            Description = cardModel.Description,
            Status = (Status)cardModel.Status,
            ProjectId = cardModel.ProjectId,
            AssignedDeveloperId = cardModel.AssignedDeveloperId
        };
    }

    public static Card ToCardFromCreate(this CreateCardDto cardDto, int projectId)
    {
        return new Card
        {
            Description = cardDto.Description,
            ProjectId = projectId,
            Title = cardDto.Title,
            Status = Data.Models.Status.Todo,
        };
    }

    
}