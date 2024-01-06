using Iter.Core.EntityModels;

namespace Iter.Core.Requests
{    public class EmployeeArrangmentResponse    {        public Guid Id { get; set; }        public User Employee { get; set; }        public string EmployeeId { get; set; }        public ArrangementResponse Arrangement { get; set; }        public Guid ArrangementId { get; set; }        public decimal Rating { get; set; }        public bool IsDeleted { get; set; }        public DateTime DateCreated { get; set; }        public DateTime DateModified { get; set; }    }}