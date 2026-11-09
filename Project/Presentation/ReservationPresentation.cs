public static class ReservationPresentation
{
    private static ReservationLogic reservationLogic = new ReservationLogic();
    private static FlightLogic flightLogic = new FlightLogic();

    public static void Start(AccountModel account)
    {
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
            string destination = flightLogic.GetDestinationCity(flights[i]);
            Console.WriteLine($"  [{i + 1}] Rotterdam -> {destination}");
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
        string selectedDestination = flightLogic.GetDestinationCity(selectedFlight);

        // Number of passengers
        Console.WriteLine();
        int passengerCount = 0;

        while (!reservationLogic.IsValidPassengerCount(passengerCount))
        {
            Console.Write("  Number of passengers: ");
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out passengerCount) || !reservationLogic.IsValidPassengerCount(passengerCount))
            {
                Console.WriteLine();
                Console.WriteLine("  > Invalid number of passengers (must be at least 1). Please try again.");
                Console.WriteLine();
            }
        }

        // Passenger information
        List<string> passengerFirstNames = new List<string>();
        List<string> passengerLastNames = new List<string>();
        List<string> passengerDatesOfBirth = new List<string>();

        bool confirmed = false;
        while (!confirmed)
        {
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
                    firstName = ReadDetail(passengerFirstNames, i);

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
                    lastName = ReadDetail(passengerLastNames, i);

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
                    dateOfBirth = ReadDetail(passengerDatesOfBirth, i);

                    if (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
                    {
                        Console.WriteLine();
                        Console.WriteLine("  > Invalid date of birth (e.g. 1990-05-20 or 20-05-1990).");
                        Console.WriteLine();
                    }
                }

                SaveDetail(passengerFirstNames, i, firstName);
                SaveDetail(passengerLastNames, i, lastName);
                SaveDetail(passengerDatesOfBirth, i, dateOfBirth);
            }

            // Calculate total price
            double totalPrice = reservationLogic.CalculateTotalPrice(selectedFlight.Price, passengerCount);

            // Reservation overview
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("               RESERVATION OVERVIEW               ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine($"  Route:           Rotterdam -> {selectedDestination}");
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
                confirmed = true;
                TicketLogic ticketLogic = new TicketLogic();
                List<TicketModel> tickets = ticketLogic.SaveBooking(
                    account.Id, selectedFlight.Id, totalPrice,
                    passengerFirstNames, passengerLastNames, passengerDatesOfBirth);
                long reservationId = tickets[0].ReservationId;
                tickets = ticketLogic.GetTicketsForAccount(account.Id)
                    .Where(ticket => ticket.ReservationId == reservationId).ToList();
                // Show confirmation with flight, date, departure time, and passengers
                TicketPresentation.ShowConfirmation(tickets, selectedFlight);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("  > Edit passenger details. Press Enter to keep a current value.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private static string ReadDetail(List<string> values, int index)
    {
        if (values.Count > index)
        {
            Console.Write($"[{values[index]}] ");
        }
        string input = Console.ReadLine() ?? "";
        if (input == "" && values.Count > index)
        {
            return values[index];
        }
        return input;
    }

    private static void SaveDetail(List<string> values, int index, string value)
    {
        if (values.Count > index)
        {
            values[index] = value;
        }
        else
        {
            values.Add(value);
        }
    }
}
