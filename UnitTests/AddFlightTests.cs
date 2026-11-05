namespace UnitTests;

[TestClass]
public class AddFlightTests
{
    [DataTestMethod]
    [DataRow(150, true)]
    [DataRow(50, true)]
    [DataRow(0, false)]
    [DataRow(-20, false)]
    public void CheckFlightPrice(double price, bool expected)
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        // Act
        bool result = logic.IsValidPrice(price);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(1000L, 2000L, true)]
    [DataRow(2000L, 1000L, false)]
    [DataRow(1000L, 1000L, false)]
    public void CheckFlightDateTime(
    long departure,
    long arrival,
    bool expected)
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        // Act
        bool result = logic.IsValidDateTime(departure, arrival);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow("15-11-2026 10:30", true)]
    [DataRow("31-02-2026 10:30", false)]
    [DataRow("abc", false)]
    [DataRow("", false)]
    public void CheckDepartureDate(string date, bool expected)
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        // Act
        bool result = logic.IsValidDepartureDate(date);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(1L, true)]
    [DataRow(2L, true)]
    [DataRow(5L, false)]
    [DataRow(0L, false)]
    public void CheckAirportExists(long airportId, bool expected)
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        List<AirportModel> airports = new List<AirportModel>
    {
        new AirportModel { Id = 1, City = "Rotterdam", Name = "Rotterdam Airport" },
        new AirportModel { Id = 2, City = "Amsterdam", Name = "Schiphol" }
    };

        // Act
        bool result = logic.IsValidAirport(airportId, airports);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(1L, true)]
    [DataRow(2L, true)]
    [DataRow(5L, false)]
    [DataRow(0L, false)]
    public void CheckPlaneExists(long planeId, bool expected)
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        List<PlaneModel> planes = new List<PlaneModel>
    {
        new PlaneModel { Id = 1, Name = "Plane A", Model = "Boeing 737", Capacity = 180 },
        new PlaneModel { Id = 2, Name = "Plane B", Model = "Airbus A320", Capacity = 150 }
    };

        // Act
        bool result = logic.IsValidPlane(planeId, planes);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void AddFlightWithValidData()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        FlightModel flight = new FlightModel
        {
            PlaneId = 1,
            StartAirportId = 1,
            DestinationId = 2,
            Price = 150,
            DepartureDatetime = 1800000000,
            ArrivalDatetime = 1800007200
        };

        List<AirportModel> airports = new List<AirportModel>
    {
        new AirportModel { Id = 1, City = "Rotterdam", Name = "Rotterdam Airport" },
        new AirportModel { Id = 2, City = "Amsterdam", Name = "Schiphol" }
    };

        List<PlaneModel> planes = new List<PlaneModel>
    {
        new PlaneModel { Id = 1, Name = "Plane A", Model = "Boeing 737", Capacity = 180 }
    };

        // Act
        bool result = logic.IsValidFlight(flight, airports, planes);

        // Assert
        Assert.IsTrue(result);
    }
}