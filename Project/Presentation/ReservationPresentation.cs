public static class ReservationPresentation
{
    private static ReservationLogic reservationLogic =
        new ReservationLogic();

    public static void Start(AccountModel account)
    {
        Console.WriteLine();
        Console.WriteLine("=== Make a Reservation ===");

        // Get flights
        List<FlightModel> flights = reservationLogic.GetFlights();

        if (flights.Count == 0)
        {
            Console.WriteLine("There are no flights available.");
            return;
        }

        // Show flights
        Console.WriteLine();
        Console.WriteLine("Available flights:");

        for (int i = 0; i < flights.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. Departure: {flights[i].DepartureDatetime} - " +
                $"Price: €{flights[i].Price}"
            );
        }

        // Select flight
        int flightChoice = 0;

        while (flightChoice < 1 || flightChoice > flights.Count)
        {
            Console.Write("Choose a flight: ");

            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out flightChoice) ||
                flightChoice < 1 ||
                flightChoice > flights.Count)
            {
                Console.WriteLine(
                    "Invalid flight. Please try again."
                );
            }
        }

        FlightModel selectedFlight =
            flights[flightChoice - 1];

        // Number of passengers
        int passengerCount = 0;

        while (!reservationLogic.IsValidPassengerCount(passengerCount))
        {
            Console.Write("Number of passengers: ");

            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out passengerCount) ||
                !reservationLogic.IsValidPassengerCount(passengerCount))
            {
                Console.WriteLine(
                    "Invalid number of passengers. Please try again."
                );
            }
        }

        // Passenger information
        List<string> passengerFirstNames =
            new List<string>();

        List<string> passengerLastNames =
            new List<string>();

        List<string> passengerDatesOfBirth =
            new List<string>();

        for (int i = 0; i < passengerCount; i++)
        {
            Console.WriteLine();
            Console.WriteLine($"Passenger {i + 1}");

            // First name
            string firstName = "";

            while (!reservationLogic.IsValidName(firstName))
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine() ?? "";

                if (!reservationLogic.IsValidName(firstName))
                {
                    Console.WriteLine(
                        "First name must contain at least 3 characters. Please try again."
                    );
                }
            }

            // Last name
            string lastName = "";

            while (!reservationLogic.IsValidName(lastName))
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine() ?? "";

                if (!reservationLogic.IsValidName(lastName))
                {
                    Console.WriteLine(
                        "Last name must contain at least 3 characters. Please try again."
                    );
                }
            }

            // Date of birth
            string dateOfBirth = "";

            while (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
            {
                Console.Write("Date of birth: ");
                dateOfBirth = Console.ReadLine() ?? "";

                if (!reservationLogic.IsValidDateOfBirth(dateOfBirth))
                {
                    Console.WriteLine(
                        "Invalid date of birth. Please try again."
                    );
                }
            }

            passengerFirstNames.Add(firstName);
            passengerLastNames.Add(lastName);
            passengerDatesOfBirth.Add(dateOfBirth);
        }

        // Calculate total price
        double totalPrice =
            reservationLogic.CalculateTotalPrice(
                selectedFlight.Price,
                passengerCount
            );

        // Reservation overview
        Console.WriteLine();
        Console.WriteLine("=== Reservation Overview ===");

        Console.WriteLine(
            $"Flight: {selectedFlight.Id}"
        );

        Console.WriteLine(
            $"Date and departure time: " +
            $"{selectedFlight.DepartureDatetime}"
        );

        Console.WriteLine();
        Console.WriteLine("Passengers:");

        for (int i = 0; i < passengerCount; i++)
        {
            Console.WriteLine(
                $"{i + 1}. " +
                $"{passengerFirstNames[i]} " +
                $"{passengerLastNames[i]} - " +
                $"{passengerDatesOfBirth[i]}"
            );
        }

        Console.WriteLine();
        Console.WriteLine(
            $"Total price: €{totalPrice}"
        );

        // Confirmation
        Console.WriteLine();

        string confirmation = "";

        while (confirmation != "yes" &&
               confirmation != "no")
        {
            Console.Write(
                "Confirm reservation? (yes/no): "
            );

            confirmation =
                Console.ReadLine() ?? "";

            if (confirmation != "yes" &&
                confirmation != "no")
            {
                Console.WriteLine(
                    "Invalid input. Please enter yes or no."
                );
            }
        }

        if (confirmation == "yes")
        {
            // Save reservation
            reservationLogic.MakeReservation(
                account.Id,
                selectedFlight.Id,
                totalPrice
            );

            // Save passengers
            for (int i = 0; i < passengerCount; i++)
            {
                reservationLogic.AddPassenger(
                    account.Id,
                    passengerFirstNames[i],
                    passengerLastNames[i],
                    passengerDatesOfBirth[i]
                );
            }

            Console.WriteLine();
            Console.WriteLine(
                "Reservation confirmed."
            );
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine(
                "Reservation not confirmed."
            );
        }
    }
}