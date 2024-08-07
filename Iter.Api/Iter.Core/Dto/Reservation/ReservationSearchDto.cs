﻿using Iter.Core.Dto;

namespace Iter.Core
{
    public class ReservationSearchDto
    {
        public Guid ReservationId { get; set; }

        public ImageDto MainImage { get; set; }

        public string ArrangementName { get; set; }

        public Guid ArrangementId { get; set; }

        public DateTime ArrangementStartDate { get; set; }

        public DateTime? ArrangementStartEndDate { get; set; }

        public int ReservationStatusId { get; set; }

        public string ReservationStatusName { get; set; }

        public string AgencyName { get; set; }

        public decimal ArrangementPrice { get; set; }

        public decimal TotalPaid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? UserId { get; set; }

        public DateTime ReservationDate { get; set; }

        public string ReservationNumber { get; set; }

        public string? TransactionId { get; set; }
    }
}
