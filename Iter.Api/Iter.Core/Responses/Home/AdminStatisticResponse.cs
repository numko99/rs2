using Iter.Core.Responses.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Core.Responses
{
    public class AdminStatisticResponse
    {
        public int ReservationCount { get; set; }

        public int ArrangementCount { get; set; }

        public int UsersCount { get; set; }

        public decimal TotalAmount { get; set; }

        public int AgenciesCount { get; set; }

        public List<ReservationDiagramResponse> Reservations { get; set; }
    }
}
