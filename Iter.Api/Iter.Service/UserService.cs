using AutoMapper;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Helper;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Identity;
using Iter.Core.Responses;
using Iter.Core.Dto;
using System.Data;
using Iter.Core.Search_Responses;

namespace Iter.Services
{
    public class UserService : BaseCrudService<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel, UserResponse>, IUserService
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IUserAuthenticationService userAuthenticationService;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserService(IUserRepository clientRepository, IMapper mapper, IUserRepository userRepository, IRabbitMQProducer rabbitMQProducer, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserAuthenticationService userAuthenticationService) : base(clientRepository, mapper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.rabbitMQProducer = rabbitMQProducer;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userAuthenticationService = userAuthenticationService;
        }

        public override async Task Insert(UserUpsertRequest request)
        {
            try
            {

                var password = PasswordGeneratorHelper.GenerateRandomPassword();
                var user = new User();
                if ((Roles)request.Role == Roles.TouristGuide || (Roles)request.Role == Roles.Coordinator)
                {
                    var entity = this.mapper.Map<Employee>(request);
                    entity.User.Employee = entity;
                    user = entity.User;

                }

                if ((Roles)request.Role == Roles.Client)
                {
                    var entity = this.mapper.Map<Client>(request);
                    entity.User.Client = entity;
                    user = entity.User;
                }

                user.IsActive = true;
                user.EmailConfirmed = true;
                var result = await this.userManager.CreateAsync(user, password);

                if (!result.Succeeded) {
                    throw new DuplicateNameException(result.Errors.ToString());
                }
                var role = (Roles)request.Role;
                await this.userManager.AddToRoleAsync(user, role.ToString());

                var emailMessage = new Core.Models.EmailMessage()
                {
                    Email = request.Email,
                    Subject = "Korisnički račun platforme ITer",
                    Content = "<p>Obavještavamo Vas da je korisnički račun za pristup ITer platformi kreiran. Kako bismo vam omogućili korištenje naše platforme, u nastavku su navedeni vaši pristupni podaci:</p>" +
                       $"<p style='margin-bottom:5px'>Email: <b>{request.Email}</b></p>" +
                       $"<p style='margin-top:0px'>Lozinka: <b>{password}</b></p>" +
                       "<p>Iz sigurnosnih razloga, molimo Vas da prilikom prve prijave promijenite ovu privremenu lozinku. To možete učiniti prateći upute u sekciji \"Postavke računa\" nakon što se prijavite.</p>"
                };

                rabbitMQProducer.SendMessage(emailMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task Update(Guid id, UserUpsertRequest request)
        {
            var user = await this.userRepository.GetById(id);
            if ((Roles)user.Role == Roles.TouristGuide || (Roles)user.Role == Roles.Coordinator)
            {
                var employee = user.Employee;
                this.mapper.Map(request, employee);
                employee.User.Employee = employee;
                user = employee.User;
            }

            if ((Roles)request.Role == Roles.Client)
            {
                var client = user.Client;
                this.mapper.Map(request, client);
                client.User.Client = client;
                user = client.User;
            }

            //await this.userRepository.UpdateAsync(user);
            await this.userManager.UpdateAsync(user);
            var role = (Roles)request.Role;
            await this.userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task NewPassword(string id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var newPassword = PasswordGeneratorHelper.GenerateRandomPassword();
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, resetToken, newPassword);

            var emailMessage = new Core.Models.EmailMessage()
            {
                Email = user.Email,
                Subject = "Promjena lozinke",
                Content = "<p>Obavještavamo Vas da je za Vaš korisnički račun na platformi ITer generisana nova lozinka. Vaša ažurirana lozinka za platformi je sljedeća:</p>" +
                 $"<p style='margin-top:0px'>Lozinka: <b>{newPassword}</b></p>" +
                 "<p>Iz sigurnosnih razloga, molimo Vas da prilikom prve prijave promijenite ovu privremenu lozinku. To možete učiniti prateći upute u sekciji \"Postavke računa\" nakon što se prijavite.</p>"
            };

            rabbitMQProducer.SendMessage(emailMessage);
        }

        public async Task UpdateCurrentUser(CurrentUserUpsertRequest request)
        {
            var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();
            var user = await this.userRepository.GetById(new Guid(currentUser.Id));
            if ((Roles)user.Role == Roles.TouristGuide || (Roles)user.Role == Roles.Coordinator)
            {
                var employee = user.Employee;
                this.mapper.Map(request, employee);
                employee.User.Employee = employee;
                user = employee.User;
            }

            if ((Roles)user.Role == Roles.Client)
            {
                var client = user.Client;
                this.mapper.Map(request, client);
                client.User.Client = client;
                user = client.User;
            }

            //await this.userRepository.UpdateAsync(user);
            await this.userManager.UpdateAsync(user);
        }


        public async Task<UserStatisticResponse> GetCurrentUserStatistic()
        {
            var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();
            if (currentUser.ClientId != null)
            {
                return await this.userRepository.GetCurrentUserStatistic(currentUser.Id);
            }

            if (currentUser.EmployeeId != null)
            {
                return await this.userRepository.GetCurrentEmployeeStatistic(currentUser.Id);
            }

            return null;
        }

        public async Task InsertClient(UserRegistrationDto userRegistration)
        {
            if (userRegistration == null)
            {
                return;
            }

            var user = await this.userManager.FindByEmailAsync(userRegistration.Email);
            var client = this.mapper.Map<Client>(userRegistration);
            client.User = user;
            await this.userRepository.InsertClient(client);
        }

        public async Task<List<UserNamesResponse>> GetUserNamesByIds(List<string> Ids)
        {
            if (Ids == null || Ids.Count == 0)
            {
                return new List<UserNamesResponse>();
            }

            var users = await this.userRepository.GetUserNamesByIds(Ids);
            var response = this.mapper.Map<List<UserNamesResponse>>(users);

            return response;
        }
    }
}