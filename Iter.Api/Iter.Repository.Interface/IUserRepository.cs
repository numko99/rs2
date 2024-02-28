using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IUserRepository : IBaseCrudRepository<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel>
    {
    }
}