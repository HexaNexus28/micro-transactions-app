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
    public class TransactMappingProfile : Profile
    {
        public TransactMappingProfile() {

            CreateMap<Transact, TransactResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<TransactResponseDto, Transact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
