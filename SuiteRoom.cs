
namespace HotelBookingSystem
{
    public class SuiteRoom : Room
    {
        private double LuxuryFee;

        public SuiteRoom(string roomNumber, double basePrice, double luxuryFee)
            : base(roomNumber, basePrice)
        {
            LuxuryFee = luxuryFee;
        }

        public override double CalculateTotalPrice(int nights)
        {
            return (BasePricePerNight + LuxuryFee) * nights;
        }

        public override string GetRoomDetails()
        {
            return base.GetRoomDetails()
                + $" | Type: Suite | Luxury Fee: ${LuxuryFee}/night";
        }
    }
}
