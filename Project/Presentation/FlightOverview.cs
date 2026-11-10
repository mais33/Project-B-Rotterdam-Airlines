public class FlightOverview
{
    private static FlightLogic flightLogic = new FlightLogic();

    public static void Start()
    {
        while (true)
        {
            ShowOverview();

            Console.WriteLine("  [1] Search flights");
            Console.WriteLine("  [2] Back");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-2): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    FlightSearch.Start();
                    break;

                case "2":
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose 1 or 2.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ShowOverview()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                 FLIGHT OVERVIEW                  ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine("  Discover our available flights:\n");

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

                string departure = flightLogic.GetDepartureCity(flight);
                string destination = flightLogic.GetDestinationCity(flight);

                Console.WriteLine($"  [{i + 1}] {departure} -> {destination}");
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
    }
}