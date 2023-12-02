using AutoMapper;

using Iter.Core;
using Iter.Core.Dto;
using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using System.Net;

namespace Iter.Api.Mapping
{
    public class IterAutoMapperProfile : Profile
    {
        public IterAutoMapperProfile()
        {
            this.CreateMap();
        }

        private void CreateMap()
        {
            this.CreateMap<UserRegistrationDto?, User?>();

            this.CreateMap<Address?, AddressResponse?>();
            this.CreateMap<AddressInsertRequest?, Address?>();

            this.CreateMap<Agency?, AgencyResponse?>();
            CreateMap<AgencyInsertRequest, Agency>()
              .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Address, opt => opt.MapFrom((src, dest) =>
             {
                 if (dest.Address != null)
                 {
                     dest.Address.City = src.City;
                     dest.Address.Country = src.Country;
                     dest.Address.Street = src.Street;
                     dest.Address.HouseNumber = src.HouseNumber;
                     dest.Address.PostalCode = src.PostalCode;
                     dest.DateModified = DateTime.Now;
                     return dest.Address;
                 }
                 else
                 {
                     var address = new Address
                     {
                         Street = src.Street,
                         HouseNumber = src.HouseNumber,
                         City = src.City,
                         PostalCode = src.PostalCode,
                         Country = src.Country,
                         IsDeleted = false,
                         DateCreated = DateTime.Now,
                         DateModified = DateTime.Now
                     };
                     return address;
                 }

             }))
              .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
              .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
              .ForMember(dest => dest.DateModified, opt => opt.Ignore());
        }
    }
}
