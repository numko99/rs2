using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Iter.Core.Search_Models;
using AutoMapper;
using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Iter.Core.Responses;
using Iter.Core.Enum;
using Iter.Core.Dto.User;

namespace Iter.Repository
{
    public class UserRepository : BaseCrudRepository<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel, UserResponse>, IUserRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserRepository(IterContext dbContext, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async override Task<User?> GetById(Guid id)
        {
            var user = await dbContext.Users
           .Include(u => u.Client)
           .Include(u => u.Employee)
           .ThenInclude(u => u.Agency)
           .Where(u => u.Id == id.ToString()).FirstOrDefaultAsync();

            return user;
        }
        public async override Task<PagedResult<UserResponse>> Get(UserSearchModel search)
        {
            try
            {
            var query = dbContext.Users
                .Include(u => u.Client)
                .Include(u => u.Employee)
                .Include(u => u.Employee)
                .ThenInclude(u => u.Agency)
                .AsQueryable();

            PagedResult<UserResponse> result = new PagedResult<UserResponse>();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                var lowerSearchName = search.Name.ToLower();
                query = query.Where(a =>
                    (a.Client != null &&
                     (a.Client.FirstName + " " + a.Client.LastName).ToLower().StartsWith(lowerSearchName) ||
                     (a.Client.LastName + " " + a.Client.FirstName).ToLower().StartsWith(lowerSearchName)) ||
                    (a.Employee != null &&
                     (a.Employee.LastName + " " + a.Employee.FirstName).ToLower().StartsWith(lowerSearchName) ||
                     (a.Employee.FirstName + " " + a.Employee.LastName).ToLower().StartsWith(lowerSearchName))
                ).AsQueryable();
            }

            if (Guid.TryParse(search?.AgencyId, out var guid))
            {
                query = query.Where(a => a.Employee != null && a.Employee.AgencyId == guid).AsQueryable();
            }

            if (search.RoleId != null)
            {
                query = query.Where(q => q.Role == search.RoleId).AsQueryable();
            }

            query = query.Where(q => (q.Employee != null && q.Employee.IsDeleted == false) || (q.Client != null && q.Client.IsDeleted == false)).AsQueryable();

            result.Count = await query.CountAsync();

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }

            var users = await query.OrderByDescending(q => q.Employee != null ? q.Employee.CreatedAt : q.Client.CreatedAt).ToListAsync();

            var userReponses = mapper.Map<List<UserResponse>>(users);

            result.Result = userReponses;
            return result;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }

        public async virtual Task DeleteAsync(User entity)
        {
            if (entity.Employee != null)
            {
                entity.Employee.IsDeleted = true;
            }

            if (entity.Client != null)
            {
                entity.Client.IsDeleted = true;
            }
            this.dbContext.User.Update(entity);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<UserStatisticResponse> GetCurrentUserStatistic(string id)
        {
            var user = await this.dbContext.User.Where(u => u.Id == id).Include(nameof(Client)).FirstOrDefaultAsync();
            var reservationCount = await this.dbContext.Reservation.Where(r => r.ClientId == user.ClientId && r.ReservationStatusId != (int)Core.Enum.ReservationStatus.Cancelled).CountAsync();
            var arrangementCount = await this.dbContext.Arrangement.Where(a => (a.StartDate < DateTime.Now) && a.Reservations.Any(x => x.ClientId == user.ClientId && x.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed)).CountAsync();
            return new UserStatisticResponse()
            {
                ArrangementsCount = arrangementCount,
                ReservationCount = reservationCount,
                FirstName = user.Client.FirstName,
                LastName = user.Client.LastName,
            };
        }

        public async Task<UserStatisticResponse> GetCurrentEmployeeStatistic(string id)
        {
            var user = await this.dbContext.User.Where(u => u.Id == id).Include(nameof(Employee)).FirstOrDefaultAsync();
            var arrangementsQuery = this.dbContext.EmployeeArrangment.Where(r => r.EmployeeId == user.EmployeeId && r.IsDeleted == false && r.Arrangement.StartDate < DateTime.Now).AsQueryable();
            var count = arrangementsQuery.Count();
            var avgRating = await arrangementsQuery.Include(a => a.Arrangement).Select(x => x.Arrangement.Rating).AverageAsync();
            return new UserStatisticResponse()
            {
                AvgRating = avgRating,
                ArrangementsCount = arrangementsQuery.Count(),
                FirstName = user.Employee.FirstName,
                LastName = user.Employee.LastName,
            };
        }

        public async Task InsertClient(Client client)
        {
            await this.dbContext.Client.AddAsync(client);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<List<UserNamesDto>> GetUserNamesByIds(List<string> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<UserNamesDto>();
            }

            var users = await this.dbContext.User
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Agency)
                .Where(u => ids.Contains(u.Id))
                .Select(x => new UserNamesDto
                {
                    Id = x.Id,
                    FirstName = x.Role == (int)Roles.Client ? x.Client.FirstName : x.Employee.FirstName,
                    LastName = x.Role == (int)Roles.Client ? x.Client.LastName : x.Employee.LastName,
                    AgencyName = (x.Role == (int)Roles.TouristGuide || x.Role == (int)Roles.Coordinator) ? x.Employee.Agency.Name : null,
                })
                .ToListAsync();

            return users;
        }
    }
}