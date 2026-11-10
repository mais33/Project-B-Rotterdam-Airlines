public static class FlightSearch
{
    private static FlightLogic flightLogic = new FlightLogic();
    private static ReservationLogic reservationLogic = new ReservationLogic();

    public static void Start()
    {
        bool searching = true;

        while (searching)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                  SEARCH FLIGHTS                  ");
            Console.WriteLine("==================================================");
            Console.WriteLine();

            bool isRoundTrip = AskIsRoundTrip();

            List<AirportModel> airports = flightLogic.GetAirports();
            List<string> cities = new List<string>();
            foreach (AirportModel airport in airports)
            {
                cities.Add(airport.City);
            }

            Console.WriteLine();
            Console.WriteLine($"  Cities: {string.Join(", ", cities)}");
            Console.WriteLine();

            AirportModel start = AskCity("  Coming from:     ");
            AirportModel destination = AskCity("  Going to:        ");
            DateTime departureDate = AskDepartureDate();

            DateTime returnDate = departureDate;
            if (isRoundTrip)
            {
                returnDate = AskReturnDate(departureDate);
            }

            int passengerCount = AskPassengerCount();

            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                  SEARCH RESULTS                  ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine($"  Passengers: {passengerCount}");
            Console.WriteLine();

            List<FlightModel> outboundFlights = flightLogic.SearchFlights(start, destination, departureDate);
            ShowFlights("Outbound flights", start, destination, outboundFlights);

            if (isRoundTrip)
            {
                List<FlightModel> returnFlights = flightLogic.SearchFlights(destination, start, returnDate);
                ShowFlights("Return flights", destination, start, returnFlights);
            }

            Console.WriteLine("--------------------------------------------------");
            searching = AskSearchAgain();
        }
    }

    private static bool AskIsRoundTrip()
    {
        Console.WriteLine("  [1] One-way");
        Console.WriteLine("  [2] Round trip");
        Console.WriteLine();

        while (true)
        {
            Console.Write("  Choose a trip type (1-2): ");
            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                return false;
            }

            if (choice == "2")
            {
                return true;
            }

            ShowError("Invalid choice. Please choose 1 or 2.");
        }
    }

    private static AirportModel AskCity(string label)
    {
        while (true)
        {
            Console.Write(label);
            string input = Console.ReadLine() ?? "";
            AirportModel? airport = flightLogic.FindAirportByCity(input);

            if (airport != null)
            {
                return airport;
            }

            ShowError("City not found. Please choose a city from the list.");
        }
    }

    private static DateTime AskDepartureDate()
    {
        while (true)
        {
            Console.Write("  Departure date:  ");
            string input = Console.ReadLine() ?? "";

            if (!flightLogic.TryParseDate(input, out DateTime date))
            {
                ShowError("Invalid date (e.g. 2026-12-25).");
            }
            else if (flightLogic.IsDateInPast(date))
            {
                ShowError("Departure date cannot be in the past.");
            }
            else
            {
                return date;
            }
        }
    }

    private static DateTime AskReturnDate(DateTime departureDate)
    {
        while (true)
        {
            Console.Write("  Return date:     ");
            string input = Console.ReadLine() ?? "";

            if (!flightLogic.TryParseDate(input, out DateTime date))
            {
                ShowError("Invalid date (e.g. 2026-12-25).");
            }
            else if (!flightLogic.IsValidReturnDate(departureDate, date))
            {
                ShowError("Return date cannot be before the departure date.");
            }
            else
            {
                return date;
            }
        }
    }

    private static int AskPassengerCount()
    {
        while (true)
        {
            Console.Write("  Passengers:      ");
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int passengerCount) && reservationLogic.IsValidPassengerCount(passengerCount))
            {
                return passengerCount;
            }

            ShowError("Invalid number of passengers (must be at least 1).");
        }
    }

    private static void ShowFlights(string title, AirportModel start, AirportModel destination, List<FlightModel> flights)
    {
        Console.WriteLine($"  {title}:");
        Console.WriteLine();

        if (flights.Count == 0)
        {
            Console.WriteLine("  > No flights found.");
            Console.WriteLine();
            return;
        }

        foreach (FlightModel flight in flights)
        {
            Console.WriteLine($"  Route:           {start.City} -> {destination.City}");
            Console.WriteLine($"  Date:            {flightLogic.GetDate(flight.DepartureDatetime)}");
            Console.WriteLine($"  Departure time:  {flightLogic.GetTime(flight.DepartureDatetime)}");
            Console.WriteLine($"  Price:           EUR {flight.Price:0.00}");
            Console.WriteLine();
        }
    }

    private static bool AskSearchAgain()
    {
        while (true)
        {
            Console.Write("  Search again? (yes/no): ");
            string answer = Console.ReadLine() ?? "";

            if (answer == "yes")
            {
                return true;
            }

            if (answer == "no")
            {
                return false;
            }

            ShowError("Invalid input. Please enter 'yes' or 'no'.");
        }
    }

    private static void ShowError(string message)
    {
        Console.WriteLine();
        Console.WriteLine($"  > {message}");
        Console.WriteLine();
    }
}
