using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.Entities;

namespace Transaction.Core.Mapping
{
    public  class AuthTokenMappingProfile : Profile
    {
        public AuthTokenMappingProfile() {

            CreateMap<AuthToken, AuthTokenResponseDto>();
            CreateMap<AuthTokenResponseDto, AuthToken>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
