public class FlightOverview
{
    private static FlightLogic flightLogic = new FlightLogic();

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                 FLIGHT OVERVIEW                  ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine("  Discover our flights departing from Rotterdam:\n");

        List<FlightModel> flights = flightLogic.GetFlights();

        if (flights.Count == 0)
        {
            Console.WriteLine("  > No flights available at the moment.");
            Console.WriteLine();
        }
        else
        {
            for (int i = 0; i < flights.Count; i++)
            {
                FlightModel flight = flights[i];
                string startCity = flightLogic.GetStartCity(flight);
                string destination = flightLogic.GetDestinationCity(flight);

                Console.WriteLine($"  [{i + 1}] {startCity} -> {destination}");
                Console.WriteLine($"      Departure:  {flight.DepartureDatetime}");
                if (!string.IsNullOrWhiteSpace(flight.ArrivalDatetime))
                {
                    Console.WriteLine($"      Arrival:    {flight.ArrivalDatetime}");
                }
                Console.WriteLine($"      Price:      EUR {flight.Price:0.00}");
                Console.WriteLine();
            }
        }

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("  Press any key to return...");
        Console.ReadKey();
    }
}
