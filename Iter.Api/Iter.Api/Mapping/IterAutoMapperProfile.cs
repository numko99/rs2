using AutoMapper;

using Iter.Core;
using Iter.Core.Dto;
using Iter.Core.EntityModels;
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
            this.CreateMap<Accommodation?, AccommodationResponse?>();
            this.CreateMap<AccommodationUpsertRequest?, Accommodation?>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.DateCreated))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Arrangement?, ArrangementResponse?>()
                .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Agency.Name));

            this.CreateMap<ArrangementUpsertRequest?, Arrangement?>();

            this.CreateMap<ArrangmentPriceUpsertRequest?, ArrangementPrice?>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.DateCreated))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<ArrangmentPriceUpsertRequest?, ArrangementPrice?>()
.ForMember(dest => dest.Id, opt => opt.Ignore())
.ForMember(dest => dest.DateCreated, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.DateCreated))
.ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Destination?, DestinationResponse?>();
            this.CreateMap<DestinationUpsertRequest?, Destination?>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.DateCreated))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<EmployeeArrangment?, EmployeeArrangmentResponse?>();
            this.CreateMap<EmployeeArrangmentUpsertRequest?, EmployeeArrangment?>();


            this.CreateMap<Reservation?, ReservationResponse?>();
            this.CreateMap<ReservationUpsertRequest?, Reservation?>();


            this.CreateMap<UserRegistrationDto?, User?>();

            this.CreateMap<Address?, AddressResponse?>();
            this.CreateMap<AgencyInsertRequest, Address>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.DateCreated))
                 .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => DateTime.Now));
            this.CreateMap<Agency?, AgencyResponse?>()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logo == null ? null : Convert.ToBase64String(src.Logo)));
            this.CreateMap<AgencyInsertRequest, Agency>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.DateCreated))
                .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<ReservationStatus, DropdownModel>();
        }
    }
}
