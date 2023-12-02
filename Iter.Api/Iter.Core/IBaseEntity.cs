using System;

namespace Iter.Core
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime? ModifiedAt { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
