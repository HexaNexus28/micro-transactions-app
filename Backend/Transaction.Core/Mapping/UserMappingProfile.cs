using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.Entities;

namespace Transaction.Core.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() {

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.AuthTokens, opt => opt.MapFrom(src => src.AuthTokens))
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));

            CreateMap<UserResponseDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


        }
    }
}
