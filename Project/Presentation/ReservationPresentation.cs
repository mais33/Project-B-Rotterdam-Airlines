public static class ReservationPresentation
{
    private static ReservationLogic reservationLogic = new ReservationLogic();
    private static FlightLogic flightLogic = new FlightLogic();

    public static int passengerCount = 0;

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

        // Get flights
        List<FlightModel> flights = reservationLogic.GetFlights();

        if (flights.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("  > There are no flights available at the moment.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show outbound flights
        Console.WriteLine();
        Console.WriteLine("  Available flights:\n");

        for (int i = 0; i < flights.Count; i++)
        {
            string startCity = flightLogic.GetStartCity(flights[i]);
            string destination = flightLogic.GetDestinationCity(flights[i]);
            Console.WriteLine($"  [{i + 1}] {startCity} -> {destination}");
            Console.WriteLine($"      Departure:  {flights[i].DepartureDatetime}");
            Console.WriteLine($"      Price:      EUR {flights[i].Price:0.00}");
            Console.WriteLine();
        }

        int backOption = flights.Count + 1;
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"  [{backOption}] Back to Customer Menu");
        Console.WriteLine("--------------------------------------------------");

        // Select outbound flight
        int flightChoice = 0;

        while (true)
        {
            Console.Write($"  Choose a flight (1-{backOption}): ");
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out flightChoice))
            {
                if (flightChoice == backOption)
                {
                    return; // Go back to customer menu
                }
                else if (flightChoice >= 1 && flightChoice <= flights.Count)
                {
                    break; // Valid flight selected
                }
            }

            Console.WriteLine();
            Console.WriteLine($"  > Invalid choice. Please choose a number between 1 and {backOption}.");
            Console.WriteLine();
        }

        FlightModel selectedFlight = flights[flightChoice - 1];
        string selectedStartCity = flightLogic.GetStartCity(selectedFlight);
        string selectedDestination = flightLogic.GetDestinationCity(selectedFlight);
        int availableSeats = reservationLogic.GetAvailableSeatCount(selectedFlight);

        if (availableSeats <= 0)
        {
            Console.WriteLine();
            Console.WriteLine("  > Sorry, there are no seats available on this flight.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Select return flight if round trip
        FlightModel? returnFlight = null;

        if (isRoundTrip)
        {
            List<FlightModel> returnFlights = reservationLogic.GetReturnFlights(selectedFlight);

            if (returnFlights.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("  > There are no return flights available for this route.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("  Available return flights:\n");

            for (int i = 0; i < returnFlights.Count; i++)
            {
                string returnDeparture = flightLogic.GetDepartureCity(returnFlights[i]);
                string returnDestination = flightLogic.GetDestinationCity(returnFlights[i]);
                Console.WriteLine($"  [{i + 1}] {returnDeparture} -> {returnDestination}");
                Console.WriteLine($"      Departure:  {returnFlights[i].DepartureDatetime}");
                Console.WriteLine($"      Price:      EUR {returnFlights[i].Price:0.00}");
                Console.WriteLine();
            }

            int returnBackOption = returnFlights.Count + 1;
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"  [{returnBackOption}] Back to Customer Menu");
            Console.WriteLine("--------------------------------------------------");

            int returnChoice = 0;

            while (true)
            {
                Console.Write($"  Choose a return flight (1-{returnBackOption}): ");
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out returnChoice))
                {
                    if (returnChoice == returnBackOption)
                    {
                        return;
                    }
                    else if (returnChoice >= 1 && returnChoice <= returnFlights.Count)
                    {
                        break;
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"  > Invalid choice. Please choose a number between 1 and {returnBackOption}.");
                Console.WriteLine();
            }

            returnFlight = returnFlights[returnChoice - 1];
        }

        // Number of passengers
        Console.WriteLine();

        while (!reservationLogic.IsValidPassengerCount(passengerCount) || passengerCount > availableSeats)
        {
            Console.Write("  Number of passengers: ");
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out passengerCount) || !reservationLogic.IsValidPassengerCount(passengerCount))
            {
                passengerCount = 0;
                Console.WriteLine();
                Console.WriteLine("  > Invalid number of passengers (must be at least 1). Please try again.");
                Console.WriteLine();
            }
            else if (passengerCount > availableSeats)
            {
                Console.WriteLine();
                Console.WriteLine($"  > Not enough seats available on this flight (available: {availableSeats}).");
                Console.WriteLine();
            }
        }

        // Passenger information (entered once, used for both flights on a round trip)
        List<string> passengerFirstNames = new List<string>();
        List<string> passengerLastNames = new List<string>();
        List<string> passengerDatesOfBirth = new List<string>();

        for (int i = 0; i < passengerCount; i++)
        {
            Console.WriteLine();
            Console.WriteLine($"  Passenger {i + 1}:");
            Console.WriteLine("  ------------------------------------------------");

            // First name
            string firstName = "";
            while (!reservationLogic.IsValidName(firstName))
            {
                Console.Write("  First name:     ");
                firstName = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > First name is required. Please enter a first name.");
                    Console.WriteLine();
                }
                else if (!reservationLogic.IsValidName(firstName))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid first name (must contain at least 3 letters).");
                    Console.WriteLine();
                }
            }

            // Last name
            string lastName = "";
            while (!reservationLogic.IsValidName(lastName))
            {
                Console.Write("  Last name:      ");
                lastName = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Last name is required. Please enter a last name.");
                    Console.WriteLine();
                }
                else if (!reservationLogic.IsValidName(lastName))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid last name (must contain at least 3 letters).");
                    Console.WriteLine();
                }
            }

            // Date of birth
            string dateOfBirth = "";
            while (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
            {
                Console.Write("  Date of birth:  ");
                dateOfBirth = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(dateOfBirth))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Date of birth is required. Please enter a date of birth.");
                    Console.WriteLine();
                }
                else if (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid date of birth (e.g. 1990-05-20 or 20-05-1990).");
                    Console.WriteLine();
                }
            }

            passengerFirstNames.Add(firstName);
            passengerLastNames.Add(lastName);
            passengerDatesOfBirth.Add(dateOfBirth);
        }

        // Calculate total price
        double totalPrice = returnFlight != null
            ? reservationLogic.CalculateTotalPrice(selectedFlight.Price, returnFlight.Price, passengerCount)
            : reservationLogic.CalculateTotalPrice(selectedFlight.Price, passengerCount);

        // Reservation overview
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("               RESERVATION OVERVIEW               ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine($"  Route:           {selectedStartCity} -> {selectedDestination}");
        Console.WriteLine($"  Date:            {flightLogic.GetDate(selectedFlight.DepartureDatetime)}");
        Console.WriteLine($"  Departure time:  {flightLogic.GetTime(selectedFlight.DepartureDatetime)}");

        if (returnFlight != null)
        {
            string returnDepartureCity = flightLogic.GetDepartureCity(returnFlight);
            string returnDestinationCity = flightLogic.GetDestinationCity(returnFlight);
            Console.WriteLine();
            Console.WriteLine($"  Return route:    {returnDepartureCity} -> {returnDestinationCity}");
            Console.WriteLine($"  Return date:     {flightLogic.GetDate(returnFlight.DepartureDatetime)}");
            Console.WriteLine($"  Return time:     {flightLogic.GetTime(returnFlight.DepartureDatetime)}");
        }

        Console.WriteLine();
        Console.WriteLine("  Passengers:");

        for (int i = 0; i < passengerCount; i++)
        {
            Console.WriteLine($"  {i + 1}. {passengerFirstNames[i]} {passengerLastNames[i]} ({passengerDatesOfBirth[i]})");
        }

        Console.WriteLine();
        Console.WriteLine($"  Total price:    EUR {totalPrice:0.00}");
        Console.WriteLine("--------------------------------------------------");

        SeatManager.Start();
        if (SeatManager.UserExited)
        {
            return;
        }

        // Confirmation
        string confirmation = "";

        while (confirmation != "yes" && confirmation != "no")
        {
            Console.Write("  Confirm reservation? (yes/no): ");
            confirmation = (Console.ReadLine() ?? "").Trim().ToLower();

            if (confirmation != "yes" && confirmation != "no")
            {
                Console.WriteLine();
                Console.WriteLine("  > Invalid input. Please enter 'yes' or 'no'.");
                Console.WriteLine();
            }
        }

        if (confirmation == "yes")
        {
            // Save reservation
            double outboundTotal = reservationLogic.CalculateTotalPrice(selectedFlight.Price, passengerCount);
            Int64 reservationId = reservationLogic.MakeReservation(account.Id, selectedFlight.Id, outboundTotal);

            // Save passengers
            List<Int64> passengerIds = new List<Int64>();
            for (int i = 0; i < passengerCount; i++)
            {
                Int64 passengerId = reservationLogic.AddPassenger(
                    passengerFirstNames[i],
                    passengerLastNames[i],
                    passengerDatesOfBirth[i]
                );
                passengerIds.Add(passengerId);
            }

            // Create tickets with unique ticket number for every passenger
            TicketLogic ticketLogic = new TicketLogic();
            string bookingReference = ticketLogic.MakeBookingReference(reservationId);
            List<TicketModel> tickets = new List<TicketModel>();

            for (int i = 0; i < passengerCount; i++)
            {
                TicketModel ticket = ticketLogic.CreateTicket(
                    reservationId,
                    passengerIds[i],
                    account.Id,
                    selectedFlight,
                    bookingReference,
                    passengerFirstNames[i],
                    passengerLastNames[i]
                );
                tickets.Add(ticket);
            }

            // If round trip, use the same passengers for the return flight reservation
            List<TicketModel>? returnTickets = null;

            if (returnFlight != null)
            {
                double returnTotal = reservationLogic.CalculateTotalPrice(returnFlight.Price, passengerCount);
                Int64 returnReservationId = reservationLogic.MakeReservation(account.Id, returnFlight.Id, returnTotal);
                returnTickets = new List<TicketModel>();

                for (int i = 0; i < passengerCount; i++)
                {
                    TicketModel returnTicket = ticketLogic.CreateTicket(
                        returnReservationId,
                        passengerIds[i],
                        account.Id,
                        returnFlight,
                        bookingReference,
                        passengerFirstNames[i],
                        passengerLastNames[i]
                    );
                    returnTickets.Add(returnTicket);
                }
            }

            // Show confirmation with flight, date, departure time, and passengers
            TicketPresentation.ShowConfirmation(tickets, selectedFlight, returnTickets, returnFlight);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("  > Reservation cancelled.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to continue...");
            Console.ReadKey();
        }
    }
}
