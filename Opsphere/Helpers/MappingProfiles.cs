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
          
          CreateMap<Reply, GetReplyDto>();
          
          CreateMap<User, ReplyUserDto>();
          
          CreateMap<AddCommentDto, CardComment>();
          
          CreateMap<CardComment, CardCommentDto>()
               .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));
          
          CreateMap<AddReplyDto, Reply>();
     }
}