public class AirportLogic
{
    private AirportAccess _airportAccess = new();


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
}