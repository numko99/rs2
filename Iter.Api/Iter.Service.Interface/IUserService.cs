using Iter.Services.Interfaces;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Core;

namespace Iter.Services.Interface
{
    public interface IUserService : IBaseCrudService<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel>
    {
        Task NewPassword(string id);
    }
}