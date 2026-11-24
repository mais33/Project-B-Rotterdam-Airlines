public class SeatLegend
{
    private SeatPriceLogic seatPriceLogic = new();

    public void DrawLegend(SeatModel[,] seatMap)
    {
        double standardPrice = seatPriceLogic.GetTierPrice(seatMap, SeatTier.Standard);
        double premiumPrice = seatPriceLogic.GetTierPrice(seatMap, SeatTier.Premium);
        double vipPrice = seatPriceLogic.GetTierPrice(seatMap, SeatTier.VIP);

        Console.WriteLine();
        Console.WriteLine("Legend");
        Console.WriteLine("------------------------------");
        DrawLegendLine("[S]", ConsoleColor.Blue, "Standard", $"EUR {standardPrice:F2}");
        DrawLegendLine("[P]", ConsoleColor.DarkYellow, "Premium", $"EUR {premiumPrice:F2}");
        DrawLegendLine("[V]", ConsoleColor.DarkMagenta, "VIP", $"EUR {vipPrice:F2}");
        DrawLegendLine("[X]", ConsoleColor.Red, "Reserved", "not available");
    }

    private void DrawLegendLine(string symbol, ConsoleColor color, string name, string info)
    {
        Console.ForegroundColor = color; // same colors as the seat map
        Console.Write(symbol);
        Console.ResetColor();
        Console.WriteLine($"  {name,-10} {info}");
    }
}
