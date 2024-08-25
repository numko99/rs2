using AutoMapper;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Helper;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Identity;
using Iter.Core.Responses;
using Iter.Core.Dto;
using System.Data;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<UserService> logger;

        public UserService(IUserRepository clientRepository, IMapper mapper, IUserRepository userRepository, IRabbitMQProducer rabbitMQProducer, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserAuthenticationService userAuthenticationService, ILogger<UserService> logger) : base(clientRepository, mapper, logger)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.rabbitMQProducer = rabbitMQProducer;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userAuthenticationService = userAuthenticationService;
            this.logger = logger;
        }

        public override async Task<PagedResult<UserResponse>> Get(UserSearchModel searchObject)
        {
            logger.LogInformation("Fetching users with search criteria: {@SearchObject}", searchObject);

            try
            {
                var data = await this.userRepository.Get(this.mapper.Map<UserSearchRequestParameters>(searchObject));
                logger.LogInformation("Fetched {Count} users.", data.Result.Count);

                return this.mapper.Map<PagedResult<UserResponse>>(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching users.");
                throw;
            }
        }

        public override async Task Insert(UserUpsertRequest request)
        {
            logger.LogInformation("Inserting a new user with email: {Email}", request.Email);

            try
            {
                var password = RandomGeneratorHelper.GenerateRandomPassword();
                User user = null;

                if ((Roles)request.Role == Roles.TouristGuide || (Roles)request.Role == Roles.Coordinator)
                {
                    var entity = this.mapper.Map<Employee>(request);
                    entity.User.Employee = entity;
                    user = entity.User;
                }
                else if ((Roles)request.Role == Roles.Client)
                {
                    var entity = this.mapper.Map<Client>(request);
                    entity.User.Client = entity;
                    user = entity.User;
                }

                user.IsActive = true;
                user.EmailConfirmed = true;
                user.CreatedAt = DateTime.Now;
                user.ModifiedAt = DateTime.Now;

                var result = await this.userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    logger.LogWarning("Failed to create user with email: {Email}. Errors: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new DuplicateNameException(result.Errors.ToString());
                }

                var role = (Roles)request.Role;
                await this.userManager.AddToRoleAsync(user, role.ToString());

                var emailMessage = new Core.Models.EmailMessage()
                {
                    Email = request.Email,
                    Subject = "Korisnički račun platforme ITer",
                    Content = $"<p>Obavještavamo Vas da je korisnički račun za pristup ITer platformi kreiran...</p>" +
                               $"<p>Email: <b>{request.Email}</b></p>" +
                               $"<p>Lozinka: <b>{password}</b></p>" +
                               "<p>Iz sigurnosnih razloga, molimo Vas da prilikom prve prijave promijenite ovu privremenu lozinku.</p>"
                };

                rabbitMQProducer.SendMessage(emailMessage);
                logger.LogInformation("User with email: {Email} created successfully.", request.Email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while inserting a new user.");
                throw;
            }
        }

        public override async Task Update(Guid id, UserUpsertRequest request)
        {
            logger.LogInformation("Updating user with id: {Id}", id);

            try
            {
                var user = await this.userRepository.GetById(id);

                if ((Roles)user.Role == Roles.TouristGuide || (Roles)user.Role == Roles.Coordinator)
                {
                    var employee = user.Employee;
                    this.mapper.Map(request, employee);
                    employee.User.Employee = employee;
                    user = employee.User;
                }
                else if ((Roles)request.Role == Roles.Client)
                {
                    var client = user.Client;
                    this.mapper.Map(request, client);
                    client.User.Client = client;
                    user = client.User;
                }

                await this.userManager.UpdateAsync(user);
                var role = (Roles)request.Role;
                await this.userManager.AddToRoleAsync(user, role.ToString());
                logger.LogInformation("User with id: {Id} updated successfully.", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating user with id: {Id}.", id);
                throw;
            }
        }

        public async Task NewPassword(string id)
        {
            logger.LogInformation("Generating new password for user with id: {Id}", id);

            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    logger.LogWarning("User with id: {Id} not found.", id);
                    throw new ArgumentException("User not found.");
                }

                var newPassword = RandomGeneratorHelper.GenerateRandomPassword();
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, resetToken, newPassword);

                var emailMessage = new Core.Models.EmailMessage()
                {
                    Email = user.Email,
                    Subject = "Promjena lozinke",
                    Content = $"<p>Obavještavamo Vas da je za Vaš korisnički račun generisana nova lozinka...</p>" +
                              $"<p>Lozinka: <b>{newPassword}</b></p>" +
                              "<p>Iz sigurnosnih razloga, molimo Vas da prilikom prve prijave promijenite ovu privremenu lozinku.</p>"
                };

                rabbitMQProducer.SendMessage(emailMessage);
                logger.LogInformation("New password for user with id: {Id} generated and sent via email.", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while generating a new password for user with id: {Id}.", id);
                throw;
            }
        }

        public async Task UpdateCurrentUser(CurrentUserUpsertRequest request)
        {
            logger.LogInformation("Updating current user.");

            try
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
                else if ((Roles)user.Role == Roles.Client)
                {
                    var client = user.Client;
                    this.mapper.Map(request, client);
                    client.User.Client = client;
                    user = client.User;
                }

                await this.userManager.UpdateAsync(user);
                logger.LogInformation("Current user updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating current user.");
                throw;
            }
        }

        public async Task<UserStatisticResponse> GetCurrentUserStatistic()
        {
            logger.LogInformation("Fetching statistics for current user.");

            try
            {
                var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();
                UserStatisticResponse? response = null;

                if (currentUser.ClientId != null)
                {
                    var data = await this.userRepository.GetCurrentUserStatistic(currentUser.Id);
                    response = this.mapper.Map<UserStatisticResponse>(data);
                }
                else if (currentUser.EmployeeId != null)
                {
                    var data = await this.userRepository.GetCurrentEmployeeStatistic(currentUser.Id);
                    response = this.mapper.Map<UserStatisticResponse>(data);
                }

                logger.LogInformation("Statistics for current user fetched successfully.");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching statistics for current user.");
                throw;
            }
        }

        public async Task InsertClient(UserRegistrationDto userRegistration)
        {
            logger.LogInformation("Inserting new client with email: {Email}", userRegistration.Email);

            try
            {
                var user = await this.userManager.FindByEmailAsync(userRegistration.Email);
                var client = this.mapper.Map<Client>(userRegistration);
                client.User = user;
                await this.userRepository.InsertClient(client);
                logger.LogInformation("Client with email: {Email} inserted successfully.", userRegistration.Email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while inserting client with email: {Email}.", userRegistration.Email);
                throw;
            }
        }

        public async Task<List<UserNamesResponse>> GetUserNamesByIds(List<string> Ids)
        {
            logger.LogInformation("Fetching user names for ids: {Ids}", string.Join(", ", Ids));

            try
            {
                if (Ids == null || Ids.Count == 0)
                {
                    logger.LogWarning("No ids provided for fetching user names.");
                    return new List<UserNamesResponse>();
                }

                var users = await this.userRepository.GetUserNamesByIds(Ids);
                var response = this.mapper.Map<List<UserNamesResponse>>(users);
                logger.LogInformation("Fetched user names for {Count} ids.", response.Count);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching user names.");
                throw;
            }
        }
    }
}
