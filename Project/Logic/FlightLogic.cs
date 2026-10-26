public class FlightLogic
{
    private FlightAccess _access = new();

    public List<FlightModel> GetFlights()
    {
        return _access.GetAll();	
    }
}
