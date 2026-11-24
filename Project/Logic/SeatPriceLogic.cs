public class SeatPriceLogic
{
    public double GetTierPrice(SeatModel[,] seatMap, SeatTier tier)
    {
        foreach (var seat in seatMap)
        {
            if (seat != null && seat.Tier == tier)
            {
                return seat.Price;
            }
        }
        return 0;
    }
}
