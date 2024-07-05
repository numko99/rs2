using Iter.Services.Interfaces;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Core.Dto;

namespace Iter.Services.Interface
{
    public interface IUserService : IBaseCrudService<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel, UserResponse>
    {
        Task NewPassword(string id);

        Task UpdateCurrentUser(CurrentUserUpsertRequest request);

        Task<UserStatisticResponse> GetCurrentUserStatistic();

        Task InsertClient(UserRegistrationDto? userRegistration);
    }
}