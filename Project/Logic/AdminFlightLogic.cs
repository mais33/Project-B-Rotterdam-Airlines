public class AdminFlightLogic
{
    private readonly AdminFlightAccess _access = new AdminFlightAccess();

    // Validate ticket price.
    public bool IsValidPrice(double price)
    {
        return price > 0;
    }

    // Validate departure and arrival times.
    public bool IsValidDateTime(string departure, string arrival)
    {
        if (!DateTime.TryParseExact(
            departure,
            "dd-MM-yyyy HH:mm",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out DateTime departureDate))
        {
            return false;
        }

        if (!DateTime.TryParseExact(
            arrival,
            "dd-MM-yyyy HH:mm",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out DateTime arrivalDate))
        {
            return false;
        }

        return arrivalDate > departureDate;
    }

    public bool IsValidDepartureDate(string date)
    {
        return DateTime.TryParseExact(
            date,
            "dd-MM-yyyy HH:mm",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out _
        );
    }

    // Check if the selected airport exists.
    public bool IsValidAirport(long airportId, List<AirportModel> airports)
    {
        foreach (AirportModel airport in airports)
        {
            if (airport.Id == airportId)
            {
                return true;
            }
        }

        return false;
    }

    // Check if the selected plane exists.
    public bool IsValidPlane(long planeId, List<PlaneModel> planes)
    {
        foreach (PlaneModel plane in planes)
        {
            if (plane.Id == planeId)
            {
                return true;
            }
        }

        return false;
    }

    // Validate all flight information.
    public bool IsValidFlight(
        FlightModel flight,
        List<AirportModel> airports,
        List<PlaneModel> planes)
    {
        if (!IsValidPrice(flight.Price))
        {
            return false;
        }

        if (!IsValidDateTime(
            flight.DepartureDatetime,
            flight.ArrivalDatetime))
        {
            return false;
        }

        if (!IsValidAirport(flight.StartAirportId, airports))
        {
            return false;
        }

        if (!IsValidAirport(flight.DestinationId, airports))
        {
            return false;
        }

        if (flight.StartAirportId == flight.DestinationId)
        {
            return false;
        }

        if (!IsValidPlane(flight.PlaneId, planes))
        {
            return false;
        }

        return true;
    }

    // Get data through the DataAccess layer.
    public List<AirportModel> GetAirports()
    {
        return _access.GetAirports();
    }

    public List<PlaneModel> GetPlanes()
    {
        return _access.GetPlanes();
    }

    public List<FlightModel> GetFlights()
    {
        return _access.GetFlights();
    }

    // Check if the plane is available during the selected time.
    public bool IsPlaneAvailable(
        long planeId,
        string departure,
        string arrival,
        List<FlightModel> existingFlights)
    {
        string[] dateFormats =
        {
        "dd-MM-yyyy HH:mm",
        "yyyy-MM-dd HH:mm"
    };

        DateTime newDeparture = DateTime.ParseExact(
            departure,
            dateFormats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None
        );

        DateTime newArrival = DateTime.ParseExact(
            arrival,
            dateFormats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None
        );

        foreach (FlightModel flight in existingFlights)
        {
            if (flight.PlaneId == planeId)
            {
                DateTime existingDeparture;
                DateTime existingArrival;

                // Read existing flight dates in either supported format.
                bool validDeparture = DateTime.TryParseExact(
                    flight.DepartureDatetime,
                    dateFormats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out existingDeparture
                );

                bool validArrival = DateTime.TryParseExact(
                    flight.ArrivalDatetime,
                    dateFormats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out existingArrival
                );

                // Do not mark a plane as available if its schedule cannot be checked.
                if (!validDeparture || !validArrival)
                {
                    return false;
                }

                // Prevent overlapping flights for the same plane.
                if (newDeparture < existingArrival &&
                    newArrival > existingDeparture)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Validate flight and plane availability before saving.
    public bool AddFlight(FlightModel flight)
    {
        List<AirportModel> airports = GetAirports();
        List<PlaneModel> planes = GetPlanes();

        if (!IsValidFlight(flight, airports, planes))
        {
            return false;
        }

        List<FlightModel> existingFlights = GetFlights();

        if (!IsPlaneAvailable(
            flight.PlaneId,
            flight.DepartureDatetime,
            flight.ArrivalDatetime,
            existingFlights))
        {
            return false;
        }

        _access.AddFlight(flight);

        return true;
    }
}