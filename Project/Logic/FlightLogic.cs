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

    // Get the actual departure city of the flight.
    public string GetDepartureCity(FlightModel flight)
    {
        AirportModel? airport = _airportAccess.GetById(flight.StartAirportId);
        return airport != null ? airport.City : "Unknown";
    }

    public string GetDestinationCity(FlightModel flight)
    {
        AirportModel airport = _airportAccess.GetById(flight.DestinationId);
        return airport != null ? airport.City : "Unknown";
    }

    public List<AirportModel> GetAirports()
    {
        return _airportAccess.GetAll();
    }

    public AirportModel? FindAirportByCity(string city)
    {
        foreach (AirportModel airport in GetAirports())
        {
            if (airport.City == city)
            {
                return airport;
            }
        }

        return null;
    }

    public bool TryParseDate(string input, out DateTime date)
    {
        return DateTime.TryParse(input, out date);
    }

    public bool IsDateInPast(DateTime date)
    {
        return date.Date < DateTime.Today;
    }

    public bool IsValidReturnDate(DateTime departureDate, DateTime returnDate)
    {
        return returnDate.Date >= departureDate.Date;
    }

    public List<FlightModel> SearchFlights(AirportModel start, AirportModel destination, DateTime date)
    {
        string searchDate = date.ToString("yyyy-MM-dd");
        return _access.Search(start.Id, destination.Id, searchDate);
    }

    // Departure looks like "2026-11-02 08:30", so the part before the space is the date
    public string GetDate(string departure)
    {
        string[] parts = departure.Split(' ');
        return parts[0];
    }

    // The part after the space is the time
    public string GetTime(string departure)
    {
        string[] parts = departure.Split(' ');

        if (parts.Length > 1)
        {
            return parts[1];
        }

        return "";
    }
}