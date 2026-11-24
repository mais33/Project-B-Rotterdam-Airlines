
public static class ReservationPresentation
{
    private static ReservationLogic reservationLogic = new ReservationLogic();
    private static FlightLogic flightLogic = new FlightLogic();
    private static AirportLogic airportLogic = new AirportLogic();
    private static TicketLogic ticketLogic = new();

    public static int passengerCount = 0;
    public static Int64 reservationId = 0;

    public static void Start(AccountModel account)
    {
        passengerCount = 0;

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                MAKE A RESERVATION                ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        // Trip type
        Console.WriteLine("  Select trip type:\n");
        Console.WriteLine("  [1] One-way");
        Console.WriteLine("  [2] Round trip");
        Console.WriteLine("  [3] Back to Customer Menu");
        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");

        bool isRoundTrip = false;

        while (true)
        {
            Console.Write("  Choose an option (1-3): ");
            string tripInput = (Console.ReadLine() ?? "").Trim();

            if (tripInput == "1")
            {
                isRoundTrip = false;
                break;
            }
            else if (tripInput == "2")
            {
                isRoundTrip = true;
                break;
            }
            else if (tripInput == "3")
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("  > Invalid choice. Please choose 1, 2 or 3.");
            Console.WriteLine();
        }

        // Search flights
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                  SEARCH FLIGHTS                  ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        List<AirportModel> airports = airportLogic.GetAirports();
        List<string> cities = new List<string>();

        foreach (AirportModel airport in airports)
        {
            cities.Add(airport.City);
        }

        Console.WriteLine($"  Cities: {string.Join(", ", cities)}");
        Console.WriteLine();

        AirportModel startAirport;
        AirportModel destinationAirport;


        while (true)
        {
            startAirport = AskCity("  Coming from:     ");
            destinationAirport = AskCity("  Going to:        ");

            if (startAirport.Id != destinationAirport.Id)
            {
                break;
            }

            Console.WriteLine();
            Console.WriteLine(
                "  > Departure and destination cannot be the same.");
            Console.WriteLine();
        }

        DateTime departureDate = AskDate(
            "  Departure date (dd-mm-yyyy): ",
            null
        );

        DateTime returnDate = departureDate;

        if (isRoundTrip)
        {
            returnDate = AskDate(
                "  Return date (dd-mm-yyyy):    ",
                departureDate
            );
        }

        // Search outbound flights
        List<FlightModel> flights = flightLogic.SearchFlights(
            startAirport,
            destinationAirport,
            departureDate
        );

        if (flights.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine(
                "  > No flights found for this route and date.");
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("  Available flights:\n");

        for (int i = 0; i < flights.Count; i++)
        {
            string departureCity = airportLogic.GetDepartureCity(flights[i]);
            string destination = airportLogic.GetDestinationCity(flights[i]);
            Console.WriteLine($"  [{i + 1}] {departureCity} -> {destination}");
            Console.WriteLine($"      Departure:  {flights[i].DepartureDatetime}");
            Console.WriteLine($"      Price:      EUR {flights[i].Price:0.00}");
            Console.WriteLine();
        }

        int backOption = flights.Count + 1;

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"  [{backOption}] Back to Customer Menu");
        Console.WriteLine("--------------------------------------------------");

        int flightChoice = 0;

        while (true)
        {
            Console.Write($"  Choose a flight (1-{backOption}): ");
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out flightChoice))
            {
                if (flightChoice == backOption)
                {
                    return;
                }

                if (flightChoice >= 1 &&
                    flightChoice <= flights.Count)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine(
                $"  > Invalid choice. Please choose a number between 1 and {backOption}.");
            Console.WriteLine();
        }

        FlightModel selectedFlight = flights[flightChoice - 1];
        string selectedDepartureCity = airportLogic.GetDepartureCity(selectedFlight);
        string selectedDestination = airportLogic.GetDestinationCity(selectedFlight);

        // Return flight
        FlightModel? returnFlight = null;

        if (isRoundTrip)
        {
            List<FlightModel> returnFlights = flightLogic.SearchFlights(
                destinationAirport,
                startAirport,
                returnDate
            );

            returnFlights = reservationLogic.GetValidReturnFlights(selectedFlight, returnFlights);

            if (returnFlights.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "  > There are no return flights available for this route and date.");
                Console.WriteLine("  Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("  Available return flights:\n");

            for (int i = 0; i < returnFlights.Count; i++)
            {
                string returnDeparture = airportLogic.GetDepartureCity(returnFlights[i]);
                string returnDestination = airportLogic.GetDestinationCity(returnFlights[i]);
                Console.WriteLine($"  [{i + 1}] {returnDeparture} -> {returnDestination}");
                Console.WriteLine($"      Departure:  {returnFlights[i].DepartureDatetime}");
                Console.WriteLine($"      Price:      EUR {returnFlights[i].Price:0.00}");
                Console.WriteLine();
            }

            int returnBackOption = returnFlights.Count + 1;

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(
                $"  [{returnBackOption}] Back to Customer Menu");
            Console.WriteLine("--------------------------------------------------");

            int returnChoice = 0;

            while (true)
            {
                Console.Write(
                    $"  Choose a return flight (1-{returnBackOption}): ");

                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out returnChoice))
                {
                    if (returnChoice == returnBackOption)
                    {
                        return;
                    }

                    if (returnChoice >= 1 &&
                        returnChoice <= returnFlights.Count)
                    {
                        break;
                    }
                }

                Console.WriteLine();
                Console.WriteLine(
                    $"  > Invalid choice. Please choose a number between 1 and {returnBackOption}.");
                Console.WriteLine();
            }

            returnFlight = returnFlights[returnChoice - 1];
        }

        // Number of passengers
        Console.WriteLine();

        while (!reservationLogic.IsValidPassengerCount(passengerCount))
        {
            Console.Write("  Number of passengers: ");
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out passengerCount) ||
                !reservationLogic.IsValidPassengerCount(passengerCount))
            {
                Console.WriteLine();
                Console.WriteLine(
                    "  > Invalid number of passengers (must be at least 1). Please try again.");
                Console.WriteLine();
            }
        }

        // Passenger information
        List<string> passengerFirstNames = new List<string>();
        List<string> passengerLastNames = new List<string>();
        List<string> passengerDatesOfBirth = new List<string>();

        for (int i = 0; i < passengerCount; i++)
        {
            Console.WriteLine();
            Console.WriteLine($"  Passenger {i + 1}:");
            Console.WriteLine("  ------------------------------------------------");

            string firstName = "";

            while (!reservationLogic.IsValidName(firstName))
            {
                Console.Write("  First name:     ");
                firstName = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "  > First name is required. Please enter a first name.");
                }
                else if (!reservationLogic.IsValidName(firstName))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "  > Invalid first name (only letters, at least 3 characters).");
                }
            }

            string lastName = "";

            while (!reservationLogic.IsValidName(lastName))
            {
                Console.Write("  Last name:      ");
                lastName = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "  > Last name is required. Please enter a last name.");
                }
                else if (!reservationLogic.IsValidName(lastName))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "  > Invalid last name (only letters, at least 3 characters).");
                }
            }

            string dateOfBirth = "";

            while (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
            {
                Console.Write("  Date of birth (dd-mm-yyyy): ");
                dateOfBirth = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(dateOfBirth))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "  > Date of birth is required. Please enter a date of birth.");
                }
                else if (reservationLogic.IsFutureDate(dateOfBirth))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "  > Date of birth cannot be in the future.");
                }
                else if (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "  > Invalid date of birth. Please use dd-mm-yyyy (e.g. 20-05-1990).");
                }
            }

            passengerFirstNames.Add(firstName);
            passengerLastNames.Add(lastName);
            passengerDatesOfBirth.Add(dateOfBirth);
        }

        // Prepare passenger models
        List<PassengerModel> passengers = new List<PassengerModel>();

        for (int i = 0; i < passengerCount; i++)
        {
            passengers.Add(new PassengerModel(
                0,
                account.Id,
                passengerFirstNames[i],
                passengerLastNames[i],
                passengerDatesOfBirth[i]
            ));
        }

        // Base flight price
        double flightTotal = returnFlight != null
            ? reservationLogic.CalculateTotalPrice(
                selectedFlight.Price,
                returnFlight.Price,
                passengerCount)
            : reservationLogic.CalculateTotalPrice(
                selectedFlight.Price,
                passengerCount);

        // Outbound seats
        Console.Clear();
        Console.WriteLine("=== OUTBOUND SEAT SELECTION ===");
        Console.WriteLine(
            $"{selectedDepartureCity} -> {selectedDestination}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);

        SeatManager.Start(selectedFlight.Id);

        if (SeatManager.UserExited)
        {
            return;
        }

        List<Int64> outboundSeatIds =
            new List<Int64>(SeatManager.SelectedSeatIds);

        // Return seats
        List<Int64>? returnSeatIds = null;

        if (returnFlight != null)
        {
            Console.Clear();
            Console.WriteLine("=== RETURN SEAT SELECTION ===");

            Console.WriteLine(
                $"{airportLogic.GetDepartureCity(returnFlight)} -> " +
                $"{airportLogic.GetDestinationCity(returnFlight)}");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            SeatManager.Start(returnFlight.Id);

            if (SeatManager.UserExited)
            {
                return;
            }

            returnSeatIds =
                new List<Int64>(SeatManager.SelectedSeatIds);
        }

        // Calculate selected seat prices
        SeatLogic seatLogic = new SeatLogic();

        SeatModel[,] outboundSeatMap =
            seatLogic.LoadSeatMapByPlaneId(selectedFlight.PlaneId);

        double outboundSeatTotal = CalculateSeatTotal(
            outboundSeatMap,
            outboundSeatIds
        );

        SeatModel[,]? returnSeatMap = null;
        double returnSeatTotal = 0;

        if (returnFlight != null && returnSeatIds != null)
        {
            returnSeatMap =
                seatLogic.LoadSeatMapByPlaneId(returnFlight.PlaneId);

            returnSeatTotal = CalculateSeatTotal(
                returnSeatMap,
                returnSeatIds
            );
        }

        double totalPrice =
            flightTotal + outboundSeatTotal + returnSeatTotal;

        // Final overview
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("               RESERVATION OVERVIEW               ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        Console.WriteLine(
            $"  Route:           {selectedDepartureCity} -> {selectedDestination}");

        Console.WriteLine(
            $"  Date:            {flightLogic.GetDate(selectedFlight.DepartureDatetime)}");

        Console.WriteLine(
            $"  Departure time:  {flightLogic.GetTime(selectedFlight.DepartureDatetime)}");

        if (returnFlight != null)
        {
            string returnDepartureCity = airportLogic.GetDepartureCity(returnFlight);
            string returnDestinationCity = airportLogic.GetDestinationCity(returnFlight);
            Console.WriteLine();
            Console.WriteLine(
                $"  Return route:    {airportLogic.GetDepartureCity(returnFlight)} -> " +
                $"{airportLogic.GetDestinationCity(returnFlight)}");

            Console.WriteLine(
                $"  Return date:     {flightLogic.GetDate(returnFlight.DepartureDatetime)}");

            Console.WriteLine(
                $"  Return time:     {flightLogic.GetTime(returnFlight.DepartureDatetime)}");
        }

        Console.WriteLine();
        Console.WriteLine("  Passengers and seats:");

        for (int i = 0; i < passengerCount; i++)
        {
            Console.WriteLine(
                $"  {i + 1}. {passengerFirstNames[i]} " +
                $"{passengerLastNames[i]} " +
                $"({passengerDatesOfBirth[i]})");

            Console.WriteLine(
                $"     Outbound seat: {GetSeatNumber(outboundSeatMap, outboundSeatIds[i])}");

            if (returnSeatIds != null && returnSeatMap != null)
            {
                Console.WriteLine(
                    $"     Return seat:   {GetSeatNumber(returnSeatMap, returnSeatIds[i])}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"  Flight price:        EUR {flightTotal:0.00}");
        Console.WriteLine($"  Outbound seats:      EUR {outboundSeatTotal:0.00}");

        if (returnFlight != null)
        {
            Console.WriteLine(
                $"  Return seats:        EUR {returnSeatTotal:0.00}");
        }

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"  TOTAL PRICE:         EUR {totalPrice:0.00}");
        Console.WriteLine("--------------------------------------------------");

        // Confirm or discard
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("              CONFIRM RESERVATION                 ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine("  [1] Confirm reservation");
        Console.WriteLine("  [2] Discard reservation");
        Console.WriteLine();

        string confirmationChoice = "";

        while (confirmationChoice != "1" &&
               confirmationChoice != "2")
        {
            Console.Write("  Choose an option (1-2): ");
            confirmationChoice = (Console.ReadLine() ?? "").Trim();

            if (confirmationChoice != "1" &&
                confirmationChoice != "2")
            {
                Console.WriteLine(
                    "  > Invalid choice. Please enter 1 or 2.");
            }
        }

        if (confirmationChoice == "2")
        {
            Console.WriteLine();
            Console.WriteLine("  > Reservation discarded.");
            Console.WriteLine("  No reservation was saved.");
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey(true);
            return;
        }
    }
        private static AirportModel AskCity(string label)
    {
        while (true)
        {
            Console.Write(label);
            string input = (Console.ReadLine() ?? "").Trim();

            AirportModel? airport =
                airportLogic.FindAirportByCity(input);

            if (airport != null)
            {
                return airport;
            }

            Console.WriteLine();
            Console.WriteLine(
                "  > City not found. Please choose a city from the list.");
            Console.WriteLine();
        }
    }

    private static DateTime AskDate(
            string label,
            DateTime? departureDate)
    {
        while (true)
        {
            Console.Write(label);
            string input = (Console.ReadLine() ?? "").Trim();

            if (!flightLogic.TryParseDate(input, out DateTime date))
            {
                Console.WriteLine();
                Console.WriteLine(
                    "  > Invalid date. Please use dd-mm-yyyy (e.g. 25-12-2026).");
                Console.WriteLine();
            }
            else if (flightLogic.IsDateInPast(date))
            {
                Console.WriteLine();
                Console.WriteLine(
                    "  > Date cannot be in the past.");
                Console.WriteLine();
            }
            else if (departureDate.HasValue &&
                     !flightLogic.IsValidReturnDate(
                         departureDate.Value,
                         date))
            {
                Console.WriteLine();
                Console.WriteLine(
                    "  > Return date cannot be before the departure date.");
                Console.WriteLine();
            }
            else
            {
                return date;
            }
        }
    }

    // Calculate prices of selected seats for the overview
    private static double CalculateSeatTotal(
        SeatModel[,] seatMap,
        List<Int64> selectedSeatIds)
    {
        double total = 0;

        foreach (Int64 seatId in selectedSeatIds)
        {
            bool found = false;

            foreach (SeatModel? seat in seatMap)
            {
                if (seat != null && seat.Id == seatId)
                {
                    total += seat.Price;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    $"Selected seat {seatId} was not found.");
            }
        }

        return total;
    }

    // Get the visible seat number, such as 1A or 2F
    private static string GetSeatNumber(
        SeatModel[,] seatMap,
        Int64 seatId)
    {
        foreach (SeatModel? seat in seatMap)
        {
            if (seat != null && seat.Id == seatId)
            {
                return seat.SeatNumber;
            }
        }

        return "Unknown";
    }

}

