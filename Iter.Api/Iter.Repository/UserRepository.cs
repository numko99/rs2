using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Iter.Core.Search_Models;
using AutoMapper;
using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Iter.Repository
{
    public class UserRepository : BaseCrudRepository<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel>, IUserRepository
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
           .ThenInclude(u => u.Address)
           .Include(u => u.Employee)
           .ThenInclude(u => u.Address)
           .Include(u => u.Employee)
           .ThenInclude(u => u.Agency)
           .Where(u => u.Id == id.ToString()).FirstOrDefaultAsync();

            return user;
        }
        public async override Task<PagedResult<UserResponse>> Get(UserSearchModel search)
        {
            var query = dbContext.Users
                .Include(u => u.Client)
                .ThenInclude(u => u.Address)
                .Include(u => u.Employee)
                .ThenInclude(u => u.Address)
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
    }
}