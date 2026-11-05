public static class AdminFlightPresentation
{
    private static AdminFlightLogic logic =
        new AdminFlightLogic();

    public static void Start()
    {
        Console.Clear();

        Console.WriteLine();
        Console.WriteLine("=== Add New Flight ===");
        Console.WriteLine();

        // Get available airports from the Logic layer.
        List<AirportModel> airports = logic.GetAirports();

        // Return to the menu if no airports are available.
        if (airports.Count == 0)
        {
            Console.WriteLine("No airports available.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show all available airports.
        Console.WriteLine("Available airports:");

        foreach (AirportModel airport in airports)
        {
            Console.WriteLine(
                $"{airport.Id}. {airport.City} - {airport.Name}"
            );
        }

        // Ask the admin to choose a departure airport.
        long departureAirportId = 0;

        while (true)
        {
            Console.Write("Choose departure airport ID: ");

            string input = Console.ReadLine() ?? "";

            if (long.TryParse(input, out departureAirportId) &&
                logic.IsValidAirport(departureAirportId, airports))
            {
                break;
            }

            Console.WriteLine("Invalid airport. Please try again.");
        }

        // Ask the admin to choose a different destination airport.
        long destinationAirportId = 0;

        while (true)
        {
            Console.Write("Choose destination airport ID: ");

            string input = Console.ReadLine() ?? "";

            if (long.TryParse(input, out destinationAirportId) &&
                logic.IsValidAirport(destinationAirportId, airports) &&
                destinationAirportId != departureAirportId)
            {
                break;
            }

            Console.WriteLine(
                "Invalid destination airport. Please choose a different airport."
            );
        }

        // Get available planes from the Logic layer.
        List<PlaneModel> planes = logic.GetPlanes();

        // Return to the menu if no planes are available.
        if (planes.Count == 0)
        {
            Console.WriteLine("No planes available.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show all available planes.
        Console.WriteLine();
        Console.WriteLine("Available planes:");

        foreach (PlaneModel plane in planes)
        {
            Console.WriteLine(
                $"{plane.Id}. {plane.Name} - {plane.Model} - Capacity: {plane.Capacity}"
            );
        }

        // Ask the admin to choose a plane.
        long planeId = 0;

        while (true)
        {
            Console.Write("Choose plane ID: ");

            string input = Console.ReadLine() ?? "";

            if (long.TryParse(input, out planeId) &&
                logic.IsValidPlane(planeId, planes))
            {
                break;
            }

            Console.WriteLine("Invalid plane. Please try again.");
        }

        // Ask the admin to enter the departure date and time.
        DateTime departureDate;

        while (true)
        {
            Console.Write(
                "Enter departure date and time (dd-MM-yyyy HH:mm): "
            );

            string input = Console.ReadLine() ?? "";

            if (logic.IsValidDepartureDate(input))
            {
                departureDate = DateTime.ParseExact(
                    input,
                    "dd-MM-yyyy HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture
                );

                break;
            }

            Console.WriteLine(
                "Invalid departure date. Please try again."
            );
        }

        // Ask the admin to enter the arrival date and time.
        DateTime arrivalDate;

        while (true)
        {
            Console.Write(
                "Enter arrival date and time (dd-MM-yyyy HH:mm): "
            );

            string input = Console.ReadLine() ?? "";

            if (logic.IsValidDepartureDate(input))
            {
                arrivalDate = DateTime.ParseExact(
                    input,
                    "dd-MM-yyyy HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture
                );

                if (arrivalDate > departureDate)
                {
                    break;
                }
            }

            Console.WriteLine(
                "Invalid arrival date. Arrival must be after departure."
            );
        }

        // Use the Netherlands time zone.
        TimeZoneInfo dutchTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");

        // Convert departure date to Unix timestamp.
        long departureTimestamp = new DateTimeOffset(
            departureDate,
            dutchTimeZone.GetUtcOffset(departureDate)
        ).ToUnixTimeSeconds();

        // Convert arrival date to Unix timestamp.
        long arrivalTimestamp = new DateTimeOffset(
            arrivalDate,
            dutchTimeZone.GetUtcOffset(arrivalDate)
        ).ToUnixTimeSeconds();

        // Ask the admin to enter a valid ticket price.
        double price = 0;

        while (true)
        {
            Console.Write("Enter ticket price (€): ");

            string input = Console.ReadLine() ?? "";

            if (double.TryParse(input, out price) &&
                logic.IsValidPrice(price))
            {
                break;
            }

            Console.WriteLine(
                "Invalid price. Please enter a price greater than 0."
            );
        }

        // Create a flight object with the entered information.
        FlightModel flight = new FlightModel
        {
            PlaneId = planeId,
            StartAirportId = departureAirportId,
            DestinationId = destinationAirportId,
            Price = price,
            DepartureDatetime = departureDate.ToString("dd-MM-yyyy HH:mm"),
            ArrivalDatetime = arrivalDate.ToString("dd-MM-yyyy HH:mm")
        };

        // Show the flight information before saving.
        Console.WriteLine();
        Console.WriteLine("=== Flight Overview ===");
        Console.WriteLine($"Departure airport ID: {departureAirportId}");
        Console.WriteLine($"Destination airport ID: {destinationAirportId}");
        Console.WriteLine($"Plane ID: {planeId}");
        Console.WriteLine($"Departure: {departureDate:dd-MM-yyyy HH:mm}");
        Console.WriteLine($"Arrival: {arrivalDate:dd-MM-yyyy HH:mm}");
        Console.WriteLine($"Ticket price: €{price:F2}");

        // Ask the admin to confirm the new flight.
        Console.WriteLine();
        Console.Write("Do you want to save this flight? (Y/N): ");

        string confirmation = Console.ReadLine() ?? "";

        if (confirmation.ToUpper() == "Y")
        {
            // Validate and save the flight through the Logic layer.
            bool success = logic.AddFlight(flight);

            if (success)
            {
                Console.WriteLine("Flight added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add flight. Invalid information.");
            }
        }
        else
        {
            Console.WriteLine("Flight was not saved.");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }
}