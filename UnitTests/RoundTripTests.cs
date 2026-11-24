namespace UnitTests;

[TestClass]
public sealed class RoundTripTests
{
    private const Int64 Rotterdam = 1;
    private const Int64 Paris = 2;
    private const Int64 London = 3;
    private const Int64 Berlin = 4;

    private static FlightModel MakeFlight(Int64 id, Int64 from, Int64 to, string departure, string arrival)
    {
        return new FlightModel(id, 1, to, from, 100.0, arrival, departure);
    }

    private static FlightModel Outbound()
    {
        return MakeFlight(1, Rotterdam, Paris, "20-12-2026 08:00", "20-12-2026 10:00");
    }

    [DataTestMethod]
    [DataRow("27-12-2026 09:00", true)]
    [DataRow("20-12-2026 12:00", true)]
    [DataRow("15-12-2026 09:00", false)]
    [DataRow("20-12-2026 09:00", false)]
    [DataRow("20-12-2026 10:00", false)]
    public void CheckReturnFlightTime(string returnDeparture, bool expected)
    {
        ReservationLogic logic = new ReservationLogic();
        FlightModel returnFlight = MakeFlight(2, Paris, Rotterdam, returnDeparture, "27-12-2026 11:00");

        bool result = logic.IsValidReturnFlight(Outbound(), returnFlight);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ReturnFlightsOnlyShowReversedRoute()
    {
        ReservationLogic logic = new ReservationLogic();
        List<FlightModel> allFlights = new List<FlightModel>
        {
            MakeFlight(2, Paris, Rotterdam, "27-12-2026 09:00", "27-12-2026 11:00"),
            MakeFlight(3, Rotterdam, London, "27-12-2026 09:00", "27-12-2026 10:00"),
            MakeFlight(4, Paris, Berlin, "27-12-2026 09:00", "27-12-2026 11:00"),
        };

        List<FlightModel> returnFlights = logic.GetValidReturnFlights(Outbound(), allFlights);

        Assert.AreEqual(1, returnFlights.Count);
        Assert.AreEqual(2, returnFlights[0].Id);
    }

    [TestMethod]
    public void NoReturnFlightOnRoute()
    {
        ReservationLogic logic = new ReservationLogic();
        List<FlightModel> allFlights = new List<FlightModel>
        {
            MakeFlight(3, Rotterdam, London, "27-12-2026 09:00", "27-12-2026 10:00"),
        };

        List<FlightModel> returnFlights = logic.GetValidReturnFlights(Outbound(), allFlights);

        Assert.AreEqual(0, returnFlights.Count);
    }

    [TestMethod]
    public void NoValidReturnFlightWhenAllDepartTooEarly()
    {
        ReservationLogic logic = new ReservationLogic();
        List<FlightModel> allFlights = new List<FlightModel>
        {
            MakeFlight(2, Paris, Rotterdam, "18-12-2026 09:00", "18-12-2026 11:00"),
            MakeFlight(5, Paris, Rotterdam, "20-12-2026 09:30", "20-12-2026 11:30"),
        };

        List<FlightModel> returnFlights = logic.GetValidReturnFlights(Outbound(), allFlights);

        Assert.AreEqual(0, returnFlights.Count);
    }
}
