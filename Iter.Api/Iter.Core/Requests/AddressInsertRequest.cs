﻿using System.ComponentModel.DataAnnotations;

namespace Iter.Core.Requests
{
    public class AddressInsertRequest
    {
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}
