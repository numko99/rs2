using AutoMapper;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Helper;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Iter.Services
{
    public class UserService : BaseCrudService<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel>, IUserService
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserService(IUserRepository clientRepository, IMapper mapper, IUserRepository userRepository, IEmailService emailService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(clientRepository, mapper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public override async Task Insert(UserUpsertRequest request)
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

            await this.userRepository.AddAsync(user);
            await this.userManager.CreateAsync(user, password);
            var role = (Roles)request.Role;
            await this.userManager.AddToRoleAsync(user, role.ToString());
            
            await emailService.SendEmailAsync(request.Email, 
                "Korisnički račun platforme ITer", 
                "<p>Obavještavamo Vas da je korisnički račun za pristup ITer platformi kreiran. Kako bismo vam omogućili korištenje naše platforme, u nastavku su navedeni vaši pristupni podaci:</p>" +
                   $"<p style='margin-bottom:5px'>Email: <b>{request.Email}</b></p>" +
                   $"<p style='margin-top:0px'>Lozinka: <b>{password}</b></p>" +
                   "<p>Iz sigurnosnih razloga, molimo Vas da prilikom prve prijave promijenite ovu privremenu lozinku. To možete učiniti prateći upute u sekciji \"Postavke računa\" nakon što se prijavite.</p>");
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

            await this.userRepository.UpdateAsync(user);
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

            await emailService.SendEmailAsync(user.Email,
              "Promjena lozinke",
              "<p>Obavještavamo Vas da je za Vaš korisnički račun na platformi ITer generisana nova lozinka. Vaša ažurirana lozinka za platformi je sljedeća:</p>" +
                 $"<p style='margin-top:0px'>Lozinka: <b>{newPassword}</b></p>" +
                 "<p>Iz sigurnosnih razloga, molimo Vas da prilikom prve prijave promijenite ovu privremenu lozinku. To možete učiniti prateći upute u sekciji \"Postavke računa\" nakon što se prijavite.</p>");


        }
    }
}