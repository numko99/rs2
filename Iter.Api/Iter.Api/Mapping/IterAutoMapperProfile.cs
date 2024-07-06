using AutoMapper;

using Iter.Core;
using Iter.Core.Dto;
using Iter.Core.Dto.User;
using Iter.Core.EntityModels;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Core.ReportDatasetModels;
using Iter.Core.RequestParameterModels;
using Iter.Core.Responses;
using Iter.Core.Search_Models;
using Iter.Core.Search_Responses;
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
            this.CreateEmployeeArrangementMap();
            this.CreateSearchModelsMap();

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
           .ConvertUsing(typeof(PagedResultConverter<,>));
        }

        private void CreateSearchModelsMap()
        {
            this.CreateMap<ArrangmentSearchModel, ArrangmentSearchParameters?>();
            this.CreateMap<EmployeeArrangementSearchModel, EmployeeArrangementRequestParameters?>();
            this.CreateMap<ReservationSearchModel, ReservationSearchRequesParameters?>();
            this.CreateMap<UserSearchModel, UserSearchRequestParameters?>();
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
            this.CreateMap<Address?, AddressResponse?>()
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name.ToString()))
                 .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name));
            this.CreateMap<AddressInsertRequest, Address>()
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                 .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
                 .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => int.Parse(src.CityId)));
        }

        private void CreateArrangementMap()
        {
            this.CreateMap<Arrangement?, ArrangementResponse?>()
                .ForMember(dest => dest.Prices, opt => opt.MapFrom(src => src.ArrangementPrices))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ArrangementImages))
                .ForMember(dest => dest.ArrangementStatusId, opt => opt.MapFrom(src => src.ArrangementStatusId))
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.EmployeeArrangments.Where(x => x.IsDeleted == false).Select(x => x.Employee.FirstName + " " + x.Employee.LastName)))
                .ForMember(dest => dest.ArrangementStatusName, opt => opt.MapFrom(src => src.ArrangementStatus.Name));
            this.CreateMap<ArrangementUpsertRequest?, Arrangement?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ArrangementPrices, opt => opt.MapFrom(src => src.Prices))
                .ForMember(dest => dest.ArrangementImages, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.ArrangementStatusId, opt => opt.Ignore())
                .ForMember(dest => dest.Destinations, opt => opt.Ignore())
                .AfterMap((src, dest, context) => {
                    if (dest.Id == default)
                    {
                        dest.ArrangementStatusId = (int)Core.Enum.ArrangementStatus.InPreparation;
                        dest.Destinations = context.Mapper.Map<List<Destination>>(src.Destinations);
                    }
                    dest.ArrangementStatus = null;

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


            this.CreateMap<Image, ImageDto?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForPath(dest => dest.Image, opt => opt.MapFrom(src => src.ImageContent));

            this.CreateMap<Image, ImageResponse?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForPath(dest => dest.Image, opt => opt.MapFrom(src => src.ImageContent));
            this.CreateMap<ImageDto, ImageResponse?>();

            this.CreateMap<ArrangementImage, ImageDto?>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Image.Name))
                .ForPath(dest => dest.Image, opt => opt.MapFrom(src => src.Image.ImageContent));

            this.CreateMap<ArrangementImage, ImageResponse?>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Image.Name))
                .ForPath(dest => dest.Image, opt => opt.MapFrom(src => src.Image.ImageContent));
        }

        private void CreateDestinationMap()
        {
            this.CreateMap<Destination?, DestinationResponse?>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.City.CountryId));

            this.CreateMap<DestinationUpsertRequest?, Destination?>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => int.Parse(src.City)))
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
        }

        private void CreateMap()
        {
            this.CreateMap<User?, UserResponse?>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FirstName : src.Client.FirstName ))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.LastName : src.Client.LastName))
                .ForMember(dest => dest.ResidencePlace, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.ResidencePlace : src.Client.ResidencePlace))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.BirthDate : src.Client.BirthDate))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Agency : null));
            this.CreateMap<Client?, UserResponse?>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : null));
            this.CreateMap<UserNamesDto?, UserNamesResponse?>();
            this.CreateMap<UserStatisticDto?, UserStatisticResponse?>();

            this.CreateMap<UserUpsertRequest?, Employee?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)))
                .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.Role, opt => opt.MapFrom(src => src.Role))
                .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.AgencyId, opt => opt.MapFrom(src => new Guid(src.AgencyId)))
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<CurrentUserUpsertRequest?, Employee?>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.BirthDate, "dd.MM.yyyy", CultureInfo.InvariantCulture)))
               .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
               .ForPath(dest => dest.User.Role, opt => opt.Ignore())
               .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
               .ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.AgencyId, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<UserUpsertRequest?, Client?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)))
                .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.Role, opt => opt.MapFrom(src => src.Role))
                .ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));


            this.CreateMap<CurrentUserUpsertRequest?, Client?>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.BirthDate, "dd.MM.yyyy", CultureInfo.InvariantCulture)))
                .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.Role, opt => opt.Ignore())
                .ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<EmployeeArrangment?, EmployeeArrangmentResponse?>();
            this.CreateMap<EmployeeArrangmentUpsertRequest?, EmployeeArrangment?>();


            this.CreateMap<Reservation?, ReservationResponse?>()
                .ForPath(dest => dest.User, opt => opt.MapFrom(src => src.Client))
                .ForPath(dest => dest.ReservationStatusName, opt => opt.MapFrom(src => src.ReservationStatus.Name));


            this.CreateMap<ReservationSearchResponse?, UserPaymentModel?>()
                .ForPath(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.ArrangementId, opt => opt.MapFrom(src => src.ArrangementId.ToString()))
                .ForPath(dest => dest.ArrangementName, opt => opt.MapFrom(src => src.ArrangementName))
                .ForPath(dest => dest.TotalPaid, opt => opt.MapFrom(src => src.TotalPaid))
                .ForPath(dest => dest.ReservationNumber, opt => opt.MapFrom(src => src.ReservationNumber));

            this.CreateMap<ReservationSearchDto?, ReservationSearchResponse?>();

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


            this.CreateMap<UserRegistrationDto?, User?>()
                .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            this.CreateMap<UserRegistrationDto?, Client?>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.BirthDate, "dd.MM.yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.Id == default ? DateTime.Now : dest.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Core.EntityModels.ReservationStatus, LookupModel>();
            this.CreateMap<Agency, LookupModel>();
            this.CreateMap<Country, LookupModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id.ToString())));
            this.CreateMap<City, LookupModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id.ToString())));
            this.CreateMap<Core.EntityModels.ArrangementStatus, LookupModel>();
            this.CreateMap<ArrangementPrice, LookupModel>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.AccommodationType));

            this.CreateMap<Arrangement, LookupModel>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            this.CreateMap<Client, LookupModel>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            this.CreateMap<LookupModel, DropdownModel>();
        }

        private void CreateEmployeeArrangementMap()
        {
            this.CreateMap<EmployeeArrangment?, EmployeeArrangmentResponse?>();
            this.CreateMap<Employee?, UserResponse?>();
            this.CreateMap<Employee?, DropdownModel?>()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            this.CreateMap<Arrangement?, ArrangementSearchDto?>()
                .ForPath(dest => dest.AgencyName, opt => opt.MapFrom(src => src.Agency.Name))
                .ForPath(dest => dest.AgencyRating, opt => opt.MapFrom(src => src.Agency.Rating))
                .ForPath(dest => dest.ArrangementStatusId, opt => opt.MapFrom(src => src.ArrangementStatusId))
                .ForPath(dest => dest.ArrangementStatusId, opt => opt.Ignore())
                .ForPath(dest => dest.MainImage, opt => opt.MapFrom(src => src.ArrangementImages.Where(x => x.IsMainImage).FirstOrDefault()));
            this.CreateMap<ArrangementSearchDto?, ArrangementSearchResponse?>();

            this.CreateMap<EmployeeArrangmentUpsertRequest?, EmployeeArrangment?>()
                .ForPath(dest => dest.ArrangementId, opt => opt.MapFrom(src => new Guid(src.ArrangementId)))
                .ForPath(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForPath(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
