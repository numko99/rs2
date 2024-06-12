using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Core.EntityModels
{
    public abstract class Person
    {
        public Guid Id { get; set; }

        public DateTime BirthDate { get; set; }

        public string ResidencePlace { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public User? User { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
