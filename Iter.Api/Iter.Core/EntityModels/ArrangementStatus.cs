using Iter.Core.EntityModels;

namespace Iter.Core.EntityModels
{
    public class ArrangementStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Arrangement> Arrangements { get; set; }
    }
}
