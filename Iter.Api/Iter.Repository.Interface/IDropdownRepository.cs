using Iter.Core.Enum;
using Iter.Core.Models;

namespace Iter.Repository.Interface
{
    public interface IDropdownRepository
    {
        Task<PagedResult<DropdownModel>> Get(int type, string? arrangementId = null);
    }
}