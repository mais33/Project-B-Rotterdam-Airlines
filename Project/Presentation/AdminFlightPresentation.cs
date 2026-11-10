public static class AdminFlightPresentation
{
    private static AdminFlightLogic logic = new AdminFlightLogic();

    public static void Start()
    {
        Console.Clear();

        Console.WriteLine();
        Console.WriteLine("=== Add New Flight ===");
        Console.WriteLine();

        // Get airports from the Logic layer.
        List<AirportModel> airports = logic.GetAirports();

        if (airports.Count == 0)
        {
            Console.WriteLine("No airports available.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show available airports.
        Console.WriteLine("Available airports:");

        foreach (AirportModel airport in airports)
        {
            Console.WriteLine(
                $"{airport.Id}. {airport.City} - {airport.Name}"
            );
        }

        // Select departure airport.
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

        // Select a different destination airport.
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

        // Enter departure date and time.
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

        // Enter arrival date and time.
        DateTime arrivalDate;

        while (true)
        {
            Console.Write(
                "Enter arrival date and time (dd-MM-yyyy HH:mm): "
            );

            string input = Console.ReadLine() ?? "";

            // Validate arrival time against the selected departure time.
            if (logic.IsValidDateTime(
                departureDate.ToString("dd-MM-yyyy HH:mm"),
                input))
            {
                arrivalDate = DateTime.ParseExact(
                    input,
                    "dd-MM-yyyy HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture
                );

                break;
            }

            Console.WriteLine(
                "Invalid arrival date. Arrival must be after departure."
            );
        }

        // Get planes and existing flights through the Logic layer.
        List<PlaneModel> planes = logic.GetPlanes();
        List<FlightModel> existingFlights = logic.GetFlights();

        // Find available planes for the selected time.
        List<PlaneModel> availablePlanes = new List<PlaneModel>();

        foreach (PlaneModel plane in planes)
        {
            if (logic.IsPlaneAvailable(
                plane.Id,
                departureDate.ToString("dd-MM-yyyy HH:mm"),
                arrivalDate.ToString("dd-MM-yyyy HH:mm"),
                existingFlights))
            {
                availablePlanes.Add(plane);
            }
        }

        // Return if no planes are available.
        if (availablePlanes.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No planes available for the selected time.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show available planes.
        Console.WriteLine();
        Console.WriteLine("Available planes:");

        foreach (PlaneModel plane in availablePlanes)
        {
            Console.WriteLine(
                $"{plane.Id}. {plane.Name} - {plane.Model} - Capacity: {plane.Capacity}"
            );
        }

        // Select an available plane.
        long planeId = 0;

        while (true)
        {
            Console.Write("Choose plane ID: ");

            string input = Console.ReadLine() ?? "";

            if (long.TryParse(input, out planeId) &&
                logic.IsValidPlane(planeId, availablePlanes))
            {
                break;
            }

            Console.WriteLine(
                "Invalid or unavailable plane. Please try again."
            );
        }

        // Enter a valid ticket price.
        double price = 0;

        while (true)
        {
            Console.Write("Enter ticket price (€): ");

            string input = Console.ReadLine() ?? "";

            // Read the price using a dot as the decimal separator.
            if (double.TryParse(
                    input,
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out price) &&
                logic.IsValidPrice(price))
            {
                break;
            }

            Console.WriteLine(
                "Invalid price. Please enter a price greater than 0."
            );
        }

        // Create the new flight.
        FlightModel flight = new FlightModel
        {
            PlaneId = planeId,
            StartAirportId = departureAirportId,
            DestinationId = destinationAirportId,
            Price = price,

            // Store dates in the same format as existing flights.
            DepartureDatetime = departureDate.ToString("yyyy-MM-dd HH:mm"),
            ArrivalDatetime = arrivalDate.ToString("yyyy-MM-dd HH:mm")
        };

        // Show flight overview before saving.
        Console.WriteLine();
        Console.WriteLine("=== Flight Overview ===");
        Console.WriteLine($"Departure airport ID: {departureAirportId}");
        Console.WriteLine($"Destination airport ID: {destinationAirportId}");
        Console.WriteLine($"Plane ID: {planeId}");
        Console.WriteLine($"Departure: {departureDate:dd-MM-yyyy HH:mm}");
        Console.WriteLine($"Arrival: {arrivalDate:dd-MM-yyyy HH:mm}");

        Console.WriteLine(
            $"Ticket price: €{price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}"
        );

        // Confirm the new flight.
        Console.WriteLine();
        Console.Write("Do you want to save this flight? (Y/N): ");

        string confirmation = Console.ReadLine() ?? "";

        if (confirmation.ToUpper() == "Y")
        {
            // Validate and save through the Logic layer.
            bool success = logic.AddFlight(flight);

            if (success)
            {
                Console.WriteLine("Flight added successfully!");
            }
            else
            {
                Console.WriteLine(
                    "Failed to add flight. Invalid information or plane unavailable."
                );
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