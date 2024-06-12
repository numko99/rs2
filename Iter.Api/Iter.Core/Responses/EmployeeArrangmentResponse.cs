using Iter.Core.EntityModels;
using Iter.Core.Models;

namespace Iter.Core
{    public class EmployeeArrangmentResponse    {        public Guid Id { get; set; }        public DropdownModel Employee { get; set; }        public ArrangementSearchResponse Arrangement { get; set; }        public decimal Rating { get; set; }    }}