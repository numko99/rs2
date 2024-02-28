namespace Iter.Core.EntityModels
{
    public class Client : Person
    {
        public List<Reservation> Reservations { get; set; }
    }
}
