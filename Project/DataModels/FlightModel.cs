public class FlightModel
{
    public Int64 Id { get; set; }
    public Int64 PlaneId { get; set; }
    public Int64 DestinationId { get; set; }
    public Int64 StartAirportId { get; set; }
    public double Price { get; set; }
    public string ArrivalDatetime { get; set; }
    public string DepartureDatetime { get; set; }

    public FlightModel()
    {
    }

    public FlightModel(
        Int64 id,
        Int64 planeId,
        Int64 destinationId,
        Int64 startAirportId,
        double price,
        string arrivalDatetime,
        string departureDatetime)
    {
        Id = id;
        PlaneId = planeId;
        DestinationId = destinationId;
        StartAirportId = startAirportId;
        Price = price;
        ArrivalDatetime = arrivalDatetime;
        DepartureDatetime = departureDatetime;
    }
}