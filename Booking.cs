
namespace HotelBookingSystem
{
    public class Booking
    {
        private Guest Guest;
        private Room Room;
        private int NumberOfNights;

        public double TotalPrice { get; private set; }

        public bool IsActive { get; private set; }

        public Booking(Guest guest, Room room, int nights)
        {
            Guest = guest;
            Room = room;
            NumberOfNights = nights;
            IsActive = false;
        }

        public bool ProcessBooking(out string message)
        {
            if (NumberOfNights <= 0)
            {
                message = "Booking failed: Number of nights must be at least 1.";
                return false;
            }

            if (IsActive)
            {
                message = "This booking has already been processed.";
                return false;
            }

            if (!Room.CheckAvailability())
            {
                message = "Booking failed: This room is already booked.";
                return false;
            }

            TotalPrice = Room.CalculateTotalPrice(NumberOfNights);

            Room.SetAvailability(false);

            IsActive = true;

            message = "Booking successfully confirmed for "
                + Guest.Name + "!";

            return true;
        }

        public bool CancelBooking(out string message)
        {
            if (!IsActive)
            {
                message = "This booking has already been cancelled or was never confirmed.";
                return false;
            }

            Room.SetAvailability(true);

            IsActive = false;

            message = "Booking successfully cancelled.";

            return true;
        }
    }
}
