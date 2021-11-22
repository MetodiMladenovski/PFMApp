using System.Collections.Generic;
using AutoMapper;
using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Mappings

{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransactionEntity, Transaction>().ReverseMap();
            CreateMap<CategoryEntity, Category>().ReverseMap();
        }
    }


}