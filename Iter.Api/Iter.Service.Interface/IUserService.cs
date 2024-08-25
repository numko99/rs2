using Iter.Services.Interfaces;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Dto;

namespace Iter.Services.Interface
{
    public interface IUserService : IBaseCrudService<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel, UserResponse>
    {
        Task NewPassword(string id);

        Task UpdateCurrentUser(CurrentUserUpsertRequest request);

        Task<UserStatisticResponse> GetCurrentUserStatistic();

        Task InsertClient(UserRegistrationDto? userRegistration);

        Task<List<UserNamesResponse>> GetUserNamesByIds(List<string> Ids);
    }
}