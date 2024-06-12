using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Core.Responses;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IUserRepository : IBaseCrudRepository<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel, UserResponse>
    {
        Task<UserStatisticResponse> GetCurrentUserStatistic(string id);

        Task<UserStatisticResponse> GetCurrentEmployeeStatistic(string id);

        Task InsertClient(Client client);
    }
}