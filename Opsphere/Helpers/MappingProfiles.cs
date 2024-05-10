using AutoMapper;
using Opsphere.Data.Models;
using Opsphere.Dtos.CardCommnets;
using Opsphere.Dtos.ReplyDto;
using Opsphere.Dtos.User;

namespace Opsphere.Helpers;

public class MappingProfiles : Profile
{
     public MappingProfiles()
     {
          CreateMap<AddCommentDto, CardComment>();
          CreateMap<CardComment, CardCommentDto>();
          CreateMap<User, DevDto>();
          CreateMap<AddReplyDto, Reply>();
     }
}