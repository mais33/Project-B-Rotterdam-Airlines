public class FlightLogic
{
    private FlightAccess _access = new();
    private AirportAccess _airportAccess = new();

    public List<FlightModel> GetFlights()
    {
        return _access.GetAll();
    }

    public string GetDestinationCity(FlightModel flight)
    {
        AirportModel airport = _airportAccess.GetById(flight.DestinationId);
        return airport.City;
    }
}
