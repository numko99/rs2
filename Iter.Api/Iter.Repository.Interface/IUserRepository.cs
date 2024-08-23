using Iter.Core;
using Iter.Core.Dto.User;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.Responses;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IUserRepository : IBaseCrudRepository<User>
    {
        Task<UserStatisticDto> GetCurrentUserStatistic(string id);

        Task<UserStatisticDto> GetCurrentEmployeeStatistic(string id);

        Task InsertClient(Client client);

        Task<List<UserNamesDto>> GetUserNamesByIds(List<string> Ids);

        Task<PagedResult<User>> Get(UserSearchRequestParameters search);

        Task<int> GetCount();
    }
}