public class AdminFlightLogic
{
    private readonly AdminFlightAccess _access =
        new AdminFlightAccess();

    // Check if the ticket price is greater than zero.
    public bool IsValidPrice(double price)
    {
        return price > 0;
    }

    // Check if arrival time is after departure time.
    public bool IsValidDateTime(long departure, long arrival)
    {
        return arrival > departure;
    }

    // Check if the date has the correct format.
    public bool IsValidDepartureDate(string date)
    {
        // Try to convert the text to a date.
        // Return false if the format is incorrect.
        return DateTime.TryParseExact(
            date,
            "dd-MM-yyyy HH:mm",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out _
        );
    }

    // Check if the selected airport exists.
    public bool IsValidAirport(
        long airportId,
        List<AirportModel> airports)
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
    public bool IsValidPlane(
        long planeId,
        List<PlaneModel> planes)
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

    // Check if all flight information is valid.
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

    // Get available airports through the DataAccess layer.
    public List<AirportModel> GetAirports()
    {
        return _access.GetAirports();
    }

    // Get available planes through the DataAccess layer.
    public List<PlaneModel> GetPlanes()
    {
        return _access.GetPlanes();
    }

    // Validate the flight before saving it.
    public bool AddFlight(FlightModel flight)
    {
        List<AirportModel> airports = GetAirports();
        List<PlaneModel> planes = GetPlanes();

        if (!IsValidFlight(flight, airports, planes))
        {
            return false;
        }

        _access.AddFlight(flight);

        return true;
    }
}