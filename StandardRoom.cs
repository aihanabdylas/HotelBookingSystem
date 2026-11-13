
namespace HotelBookingSystem
{
    public class StandardRoom : Room
    {
        public StandardRoom(string roomNumber, double basePrice)
            : base(roomNumber, basePrice)
        {
        }

        public override string GetRoomDetails()
        {
            return base.GetRoomDetails() + " | Type: Standard";
        }
    }
}
