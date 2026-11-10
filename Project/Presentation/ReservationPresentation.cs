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

        // Get flights
        List<FlightModel> flights = reservationLogic.GetFlights();

        if (flights.Count == 0)
        {
            Console.WriteLine("  > There are no flights available at the moment.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show flights
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

        // Select flight
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

        // Passenger information
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
                firstName = Console.ReadLine() ?? "";

                if (!reservationLogic.IsValidName(firstName))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > First name must contain at least 3 characters.");
                    Console.WriteLine();
                }
            }

            // Last name
            string lastName = "";
            while (!reservationLogic.IsValidName(lastName))
            {
                Console.Write("  Last name:      ");
                lastName = Console.ReadLine() ?? "";

                if (!reservationLogic.IsValidName(lastName))
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Last name must contain at least 3 characters.");
                    Console.WriteLine();
                }
            }

            // Date of birth
            string dateOfBirth = "";
            while (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
            {
                Console.Write("  Date of birth:  ");
                dateOfBirth = Console.ReadLine() ?? "";

                if (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
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
        double totalPrice = reservationLogic.CalculateTotalPrice(selectedFlight.Price, passengerCount);

        // Reservation overview
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("               RESERVATION OVERVIEW               ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine($"  Route:           {selectedStartCity} -> {selectedDestination}");
        Console.WriteLine($"  Date:            {flightLogic.GetDate(selectedFlight.DepartureDatetime)}");
        Console.WriteLine($"  Departure time:  {flightLogic.GetTime(selectedFlight.DepartureDatetime)}");
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
            Int64 reservationId = reservationLogic.MakeReservation(account.Id, selectedFlight.Id, totalPrice);

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

            // Show confirmation with flight, date, departure time, and passengers
            TicketPresentation.ShowConfirmation(tickets, selectedFlight);
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
