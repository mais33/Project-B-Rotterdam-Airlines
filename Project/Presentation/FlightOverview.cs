public class FlightOverview
{
    private static FlightLogic flightLogic = new FlightLogic();

    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("==============================================");
        Console.WriteLine("              ROTTERDAM AIRLINES");
        Console.WriteLine("==============================================");
        Console.WriteLine("Discover our best flights departing from Rotterdam.");
        Console.WriteLine();

        foreach (FlightModel flight in flightLogic.GetFlights())
        {
            Console.WriteLine($"  Rotterdam -> {flightLogic.GetDestinationCity(flight)}");
            Console.WriteLine($"  Departure:  {flight.DepartureDatetime}");
            Console.WriteLine($"  Price:      EUR {flight.Price:0.00}");
            Console.WriteLine("----------------------------------------------");
        }
    }
}
