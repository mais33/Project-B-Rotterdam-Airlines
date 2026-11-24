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
        if (!DateLogic.TryReadDateTime(departure, out DateTime departureDate))
        {
            return false;
        }

        if (!DateLogic.TryReadDateTime(arrival, out DateTime arrivalDate))
        {
            return false;
        }

        // Departure must be in the future and arrival must be later.
        return departureDate > DateTime.Now &&
               arrivalDate > departureDate;
    }

    // Validate that the departure date is in the future.
    public bool IsValidDepartureDate(string date)
    {
        if (!DateLogic.TryReadDateTime(date, out DateTime departureDate))
        {
            return false;
        }

        return departureDate > DateTime.Now;
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
        DateTime newDeparture;
        DateTime newArrival;

        // Do not mark a plane as available if the new dates cannot be read.
        if (!DateLogic.TryReadDateTime(departure, out newDeparture) ||
            !DateLogic.TryReadDateTime(arrival, out newArrival))
        {
            return false;
        }

        foreach (FlightModel flight in existingFlights)
        {
            if (flight.PlaneId == planeId)
            {
                DateTime existingDeparture;
                DateTime existingArrival;

                // Read the dates of the existing flight.
                bool validDeparture = DateLogic.TryReadDateTime(
                    flight.DepartureDatetime,
                    out existingDeparture
                );

                bool validArrival = DateLogic.TryReadDateTime(
                    flight.ArrivalDatetime,
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