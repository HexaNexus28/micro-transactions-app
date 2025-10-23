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
    public  class ItemMappingProfile : Profile
    {
        public ItemMappingProfile() {

            CreateMap<ItemResponseDto, Item>()
                .ForMember(dest => dest.Transactions, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Item, ItemResponseDto>();
        }
    }
}
