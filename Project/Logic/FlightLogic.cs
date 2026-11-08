public class FlightLogic
{
    private FlightAccess _access = new();
    private AirportAccess _airportAccess = new();

    public List<FlightModel> GetFlights()
    {
        return _access.GetAll();
    }

    public FlightModel? GetFlightById(Int64 id)
    {
        return _access.GetById(id);
    }

    public string GetStartCity(FlightModel flight)
    {
        if (flight.StartAirportId <= 0)
        {
            return "Rotterdam";
        }

        AirportModel? airport = _airportAccess.GetById(flight.StartAirportId);
        return airport != null ? airport.City : "Rotterdam";
    }

    public string GetDestinationCity(FlightModel flight)
    {
        AirportModel? airport = _airportAccess.GetById(flight.DestinationId);
        return airport != null ? airport.City : "Unknown";
    }

    // Departure looks like "2026-11-02 08:30", so the part before the space is the date
    public string GetDate(string departure)
    {
        if (string.IsNullOrWhiteSpace(departure))
        {
            return "Unknown";
        }

        string[] parts = departure.Split(' ');
        return parts[0];
    }

    // The part after the space is the time
    public string GetTime(string departure)
    {
        if (string.IsNullOrWhiteSpace(departure))
        {
            return "";
        }

        string[] parts = departure.Split(' ');
        if (parts.Length > 1)
        {
            return parts[1];
        }
        return "";
    }
}
