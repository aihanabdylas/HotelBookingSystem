
namespace HotelBookingSystem
{
    public abstract class Room
    {
        protected string RoomNumber;
        protected double BasePricePerNight;
        protected bool IsAvailable;

        public Room(string roomNumber, double basePrice)
        {
            RoomNumber = roomNumber;
            BasePricePerNight = basePrice;
            IsAvailable = true;
        }

        public string GetRoomNumber()
        {
            return RoomNumber;
        }

        public bool CheckAvailability()
        {
            return IsAvailable;
        }

        public void SetAvailability(bool status)
        {
            IsAvailable = status;
        }

        public virtual double CalculateTotalPrice(int nights)
        {
            return BasePricePerNight * nights;
        }

        public virtual string GetRoomDetails()
        {
            return $"Room {RoomNumber} | Base Price: ${BasePricePerNight}/night";
        }
    }
}
