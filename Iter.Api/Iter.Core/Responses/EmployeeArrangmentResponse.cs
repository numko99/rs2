using Iter.Core.Responses;

namespace Iter.Core
{    public class EmployeeArrangmentResponse    {        public Guid Id { get; set; }        public EmployeeResponse Employee { get; set; }        public ArrangementSearchResponse Arrangement { get; set; }        public decimal Rating { get; set; }    }}