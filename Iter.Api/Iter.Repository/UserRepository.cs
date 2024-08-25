using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Iter.Core.Responses;
using Iter.Core.Enum;
using Iter.Core.Dto.User;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class UserRepository : BaseCrudRepository<User>, IUserRepository
    {
        private readonly IterContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(IterContext dbContext,  UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserRepository> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async override Task<User?> GetById(Guid id)
        {
            logger.LogInformation("GetById operation started for User ID: {Id}", id);

            try
            {
                var user = await dbContext.Users
                    .Include(u => u.Client)
                    .Include(u => u.Employee)
                    .ThenInclude(u => u.Agency)
                    .Where(u => u.Id == id.ToString())
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    logger.LogWarning("No user found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetById operation completed successfully for User ID: {Id}", id);
                }

                return user;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for User ID: {Id}", id);
                throw;
            }
        }

        public async Task<PagedResult<User>> Get(UserSearchRequestParameters search)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchParameters}", search);

            try
            {
                var query = dbContext.Set<User>().AsQueryable();
                PagedResult<User> result = new PagedResult<User>();

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
                    );
                }

                if (Guid.TryParse(search?.AgencyId, out var guid))
                {
                    query = query.Where(a => a.Employee != null && a.Employee.AgencyId == guid);
                }

                if (search.RoleId != null)
                {
                    query = query.Where(q => q.Role == search.RoleId);
                }

                query = query.Where(q => (q.Employee != null && q.Employee.IsDeleted == false) || (q.Client != null && q.Client.IsDeleted == false));

                result.Count = await query.CountAsync();

                query = query
                        .Include(u => u.Client)
                        .Include(u => u.Employee)
                        .Include(u => u.Employee)
                        .ThenInclude(u => u.Agency);

                if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
                {
                    query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
                }

                var users = await query.OrderByDescending(q => q.CreatedAt).ToListAsync();
                result.Result = users;

                logger.LogInformation("Get operation completed successfully with {Count} results.", users.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@SearchParameters}", search);
                throw;
            }
        }

        public async override Task DeleteAsync(User entity)
        {
            logger.LogInformation("DeleteAsync operation started for User ID: {Id}", entity.Id);

            try
            {
                if (entity.Employee != null)
                {
                    entity.Employee.IsDeleted = true;
                }

                if (entity.Client != null)
                {
                    entity.Client.IsDeleted = true;
                }

                dbContext.User.Update(entity);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("DeleteAsync operation completed successfully for User ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during DeleteAsync operation for User ID: {Id}", entity.Id);
                throw;
            }
        }

        public async Task<UserStatisticDto> GetCurrentUserStatistic(string id)
        {
            logger.LogInformation("GetCurrentUserStatistic operation started for User ID: {Id}", id);

            try
            {
                var user = await dbContext.User
                    .Where(u => u.Id == id)
                    .Include(nameof(Client))
                    .FirstOrDefaultAsync();

                var reservationCount = await dbContext.Reservation
                    .Where(r => r.ClientId == user.ClientId && r.ReservationStatusId != (int)Core.Enum.ReservationStatus.Cancelled)
                    .CountAsync();

                var arrangementCount = await dbContext.Arrangement
                    .Where(a => (a.StartDate < DateTime.Now) && a.Reservations.Any(x => x.ClientId == user.ClientId && x.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed))
                    .CountAsync();

                var result = new UserStatisticDto
                {
                    ArrangementsCount = arrangementCount,
                    ReservationCount = reservationCount,
                    FirstName = user.Client.FirstName,
                    LastName = user.Client.LastName,
                };

                logger.LogInformation("GetCurrentUserStatistic operation completed successfully for User ID: {Id}.", id);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCurrentUserStatistic operation for User ID: {Id}", id);
                throw;
            }
        }

        public async Task<UserStatisticDto> GetCurrentEmployeeStatistic(string id)
        {
            logger.LogInformation("GetCurrentEmployeeStatistic operation started for Employee ID: {Id}", id);

            try
            {
                var user = await dbContext.User
                    .Where(u => u.Id == id)
                    .Include(nameof(Employee))
                    .FirstOrDefaultAsync();

                var arrangementsQuery = dbContext.EmployeeArrangment
                    .Where(r => r.EmployeeId == user.EmployeeId && r.IsDeleted == false && r.Arrangement.StartDate < DateTime.Now);

                var count = arrangementsQuery.Count();
                var ratings = await arrangementsQuery
                 .Include(a => a.Arrangement)
                 .Select(x => x.Arrangement.Rating)
                 .ToListAsync();

                decimal avgRating = 0;

                if (ratings.Any())
                {
                    avgRating = ratings.Average();
                }

                var result = new UserStatisticDto
                {
                    AvgRating = avgRating,
                    ArrangementsCount = count,
                    FirstName = user.Employee.FirstName,
                    LastName = user.Employee.LastName,
                };

                logger.LogInformation("GetCurrentEmployeeStatistic operation completed successfully for Employee ID: {Id}.", id);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCurrentEmployeeStatistic operation for Employee ID: {Id}", id);
                throw;
            }
        }

        public async Task InsertClient(Client client)
        {
            logger.LogInformation("InsertClient operation started.");

            try
            {
                await dbContext.Client.AddAsync(client);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("InsertClient operation completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during InsertClient operation.");
                throw;
            }
        }

        public async Task<List<UserNamesDto>> GetUserNamesByIds(List<string> ids)
        {
            logger.LogInformation("GetUserNamesByIds operation started with IDs: {@Ids}", ids);

            try
            {
                if (ids == null || !ids.Any())
                {
                    logger.LogWarning("No IDs provided for GetUserNamesByIds operation.");
                    return new List<UserNamesDto>();
                }

                var users = await dbContext.User
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

                logger.LogInformation("GetUserNamesByIds operation completed successfully with {Count} users.", users.Count);

                return users;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetUserNamesByIds operation with IDs: {@Ids}", ids);
                throw;
            }
        }

        public async Task<int> GetCount()
        {
            logger.LogInformation("GetCount operation started.");

            try
            {
                var count = await dbContext.User.CountAsync();

                logger.LogInformation("GetCount operation completed successfully with count: {Count}.", count);

                return count;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCount operation.");
                throw;
            }
        }
    }
}
