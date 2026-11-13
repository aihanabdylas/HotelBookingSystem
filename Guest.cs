
namespace HotelBookingSystem
{
    public class Guest
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Guest(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string GetGuestDetails()
        {
            return $"Guest: {Name} ({Email})";
        }
    }
}
