using AutoMapper;

using Iter.Core;
using Iter.Core.Dto;
using Iter.Core.EntityModels;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Core.ReportDatasetModels;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Iter.Api.Mapping
{
    public class IterAutoMapperProfile : Profile
    {
        public IterAutoMapperProfile()
        {
            this.CreateMap();
            this.CreateAgencyMap();
            this.CreateAccomodationMap();
            this.CreateAddressMap();
            this.CreateArrangementMap();
            this.CreateDestinationMap();
        }

        private void CreateAgencyMap()
        {
            this.CreateMap<Agency?, AgencyResponse?>()
                 .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Image));

            this.CreateMap<AgencyInsertRequest, Agency>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                   .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Logo))
                   .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                   .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

        }

        private void CreateAccomodationMap()
        {
            this.CreateMap<Accommodation?, AccommodationResponse?>();
            this.CreateMap<AccommodationUpsertRequest?, Accommodation?>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
        }

        private void CreateAddressMap()
        {
            this.CreateMap<Address?, AddressResponse?>();
            this.CreateMap<AddressInsertRequest, Address>()
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                 .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
        }

        private void CreateArrangementMap()
        {
            this.CreateMap<Arrangement?, ArrangementResponse?>()
                .ForMember(dest => dest.Prices, opt => opt.MapFrom(src => src.ArrangementPrices))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ArrangementImages))
                .ForMember(dest => dest.ArrangementStatusId, opt => opt.MapFrom(src => src.ArrangementStatusId))
                .ForMember(dest => dest.ArrangementStatusName, opt => opt.MapFrom(src => src.ArrangementStatus.Name));
            this.CreateMap<ArrangementUpsertRequest?, Arrangement?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ArrangementPrices, opt => opt.MapFrom(src => src.Prices))
                .ForMember(dest => dest.ArrangementImages, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Destinations, opt => opt.Ignore())
                .AfterMap((src, dest, context) => {
                    if (dest.Id == default)
                    {
                        dest.ArrangementStatusId = (int)Core.Enum.ArrangementStatus.InPreparation;
                        dest.Destinations = context.Mapper.Map<List<Destination>>(src.Destinations);
                    }

                });

            this.CreateMap<ArrangmentPriceUpsertRequest?, ArrangementPrice?>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => new Guid(src.Id)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<ArrangementPrice?, ArrangementPriceResponse?>();

            this.CreateMap<ImageUpsertRequest, ArrangementImage?>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.Image.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.Image.ImageContent, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<ImageUpsertRequest, Image?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.ImageContent, opt => opt.MapFrom(src => src.Image));


            this.CreateMap<Image, ImageResponse?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForPath(dest => dest.Image, opt => opt.MapFrom(src => src.ImageContent));

            this.CreateMap<ArrangementImage, ImageResponse?>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Image.Name))
                .ForPath(dest => dest.Image, opt => opt.MapFrom(src => src.Image.ImageContent));

        }

        private void CreateDestinationMap()
        {
            this.CreateMap<Destination?, DestinationResponse?>();
            this.CreateMap<DestinationUpsertRequest?, Destination?>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
        }

        private void CreateMap()
        {
            this.CreateMap<User?, UserResponse?>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FirstName : src.Client.FirstName ))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.LastName : src.Client.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BirthDate : src.Client.BirthDate))
                .ForMember(dest => dest.BirthPlace, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BirthPlace : src.Client.BirthPlace))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Address : src.Client.Address))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Address : src.Client.Address))
                .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Agency : null));
            this.CreateMap<Client?, UserResponse?>();

            this.CreateMap<UserUpsertRequest?, Employee?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)))
                .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.Role, opt => opt.MapFrom(src => src.Role))
                .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.AgencyId, opt => opt.MapFrom(src => new Guid(src.AgencyId)));

            this.CreateMap<UserUpsertRequest?, Client?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)))
                .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.Role, opt => opt.MapFrom(src => src.Role))
                .ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<EmployeeArrangment?, EmployeeArrangmentResponse?>();
            this.CreateMap<EmployeeArrangmentUpsertRequest?, EmployeeArrangment?>();


            this.CreateMap<Reservation?, ReservationResponse?>()
                .ForPath(dest => dest.User, opt => opt.MapFrom(src => src.Client))
                .ForPath(dest => dest.ReservationStatusName, opt => opt.MapFrom(src => src.ReservationStatus.Name));


            this.CreateMap<ReservationResponse?, UserPaymentModel?>()
                .ForPath(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForPath(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForPath(dest => dest.ArrangementId, opt => opt.MapFrom(src => src.Arrangement.Id))
                .ForPath(dest => dest.ArrangementName, opt => opt.MapFrom(src => src.Arrangement.Name));

            this.CreateMap<ReservationInsertRequest?, Reservation?>()
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                 .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<ReservationUpdateRequest?, Reservation?>()
              .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
              .ForMember(dest => dest.ClientId, opt => opt.Ignore())
              .ForMember(dest => dest.ArrangmentId, opt => opt.Ignore())
              .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
              .ForMember(dest => dest.ArrangementPriceId, opt => opt.MapFrom(src => new Guid(src.ArrangementPriceId)))
              .ForMember(dest => dest.ReservationStatusId, opt => opt.MapFrom(src => int.Parse(src.ReservationStatusId)));


            this.CreateMap<UserRegistrationDto?, User?>();
            this.CreateMap<Core.EntityModels.ReservationStatus, DropdownModel>();
            this.CreateMap<Agency, DropdownModel>();
            this.CreateMap<Core.EntityModels.ArrangementStatus, DropdownModel>();
            this.CreateMap<ArrangementPrice, DropdownModel>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.AccommodationType));

            this.CreateMap<Arrangement, DropdownModel>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            this.CreateMap<Client, DropdownModel>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
