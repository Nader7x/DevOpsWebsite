using AutoMapper;
using Opsphere.Data.Models;
using Opsphere.Dtos.CardCommnets;

namespace Opsphere.Helpers;

public class MappingProfiles : Profile
{
     public MappingProfiles()
     {
          CreateMap<AddCommentDto, CardComment>();
          CreateMap<CardComment, CardCommentDto>();
     }
}