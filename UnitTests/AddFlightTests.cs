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

    [TestMethod]
    public void ValidFlightDateTimeIsAccepted()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        DateTime departure = DateTime.Now.AddDays(10);
        DateTime arrival = departure.AddHours(2);

        // Act
        bool result = logic.IsValidDateTime(
            departure.ToString("dd-MM-yyyy HH:mm"),
            arrival.ToString("dd-MM-yyyy HH:mm")
        );

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ArrivalBeforeDepartureIsInvalid()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        DateTime departure = DateTime.Now.AddDays(10);
        DateTime arrival = departure.AddHours(-2);

        // Act
        bool result = logic.IsValidDateTime(
            departure.ToString("dd-MM-yyyy HH:mm"),
            arrival.ToString("dd-MM-yyyy HH:mm")
        );

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ArrivalEqualToDepartureIsInvalid()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        DateTime departure = DateTime.Now.AddDays(10);

        // Act
        bool result = logic.IsValidDateTime(
            departure.ToString("dd-MM-yyyy HH:mm"),
            departure.ToString("dd-MM-yyyy HH:mm")
        );

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidDepartureDateIsAccepted()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        DateTime departure = DateTime.Now.AddDays(10);

        // Act
        bool result = logic.IsValidDepartureDate(
            departure.ToString("dd-MM-yyyy HH:mm")
        );

        // Assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("31-02-2026 10:30")]
    [DataRow("abc")]
    [DataRow("")]
    public void InvalidDepartureDateIsRejected(string date)
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        // Act
        bool result = logic.IsValidDepartureDate(date);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void DepartureDateInThePastIsInvalid()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        // Act
        bool result = logic.IsValidDepartureDate(
            "01-01-2000 10:00"
        );

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void FlightDateTimeInThePastIsInvalid()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        // Act
        bool result = logic.IsValidDateTime(
            "01-01-2000 10:00",
            "01-01-2000 12:00"
        );

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void FlightDateTimeWithDatabaseFormatIsValid()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        DateTime departure = DateTime.Now.AddDays(10);
        DateTime arrival = departure.AddHours(2);

        // Act
        bool result = logic.IsValidDateTime(
            departure.ToString("yyyy-MM-dd HH:mm"),
            arrival.ToString("yyyy-MM-dd HH:mm")
        );

        // Assert
        Assert.IsTrue(result);
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
            new AirportModel
            {
                Id = 1,
                City = "Rotterdam",
                Name = "Rotterdam Airport"
            },
            new AirportModel
            {
                Id = 2,
                City = "Amsterdam",
                Name = "Schiphol"
            }
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
            new PlaneModel
            {
                Id = 1,
                Name = "Plane A",
                Model = "Boeing 737",
                Capacity = 180
            },
            new PlaneModel
            {
                Id = 2,
                Name = "Plane B",
                Model = "Airbus A320",
                Capacity = 150
            }
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

        DateTime departure = DateTime.Now.AddDays(10);
        DateTime arrival = departure.AddHours(2);

        FlightModel flight = new FlightModel
        {
            PlaneId = 1,
            StartAirportId = 1,
            DestinationId = 2,
            Price = 150,
            DepartureDatetime = departure.ToString("yyyy-MM-dd HH:mm"),
            ArrivalDatetime = arrival.ToString("yyyy-MM-dd HH:mm")
        };

        List<AirportModel> airports = new List<AirportModel>
        {
            new AirportModel
            {
                Id = 1,
                City = "Rotterdam",
                Name = "Rotterdam Airport"
            },
            new AirportModel
            {
                Id = 2,
                City = "Amsterdam",
                Name = "Schiphol"
            }
        };

        List<PlaneModel> planes = new List<PlaneModel>
        {
            new PlaneModel
            {
                Id = 1,
                Name = "Plane A",
                Model = "Boeing 737", 
                Capacity = 180
            }
        };

        // Act
        bool result = logic.IsValidFlight(
            flight,
            airports,
            planes
        );

        // Assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("20-11-2026 08:00", "20-11-2026 09:00", true)]
    [DataRow("20-11-2026 12:00", "20-11-2026 14:00", true)]
    [DataRow("20-11-2026 11:00", "20-11-2026 13:00", false)]
    [DataRow("20-11-2026 09:00", "20-11-2026 11:00", false)]
    [DataRow("20-11-2026 10:00", "20-11-2026 12:00", false)]
    [DataRow("20-11-2026 09:00", "20-11-2026 13:00", false)]
    public void CheckPlaneAvailability(
        string newDeparture,
        string newArrival,
        bool expected)
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        List<FlightModel> existingFlights = new List<FlightModel>
        {
            new FlightModel
            {
                PlaneId = 1,
                DepartureDatetime = "20-11-2026 10:00",
                ArrivalDatetime = "20-11-2026 12:00"
            }
        };

        // Act
        bool result = logic.IsPlaneAvailable(
            1,
            newDeparture,
            newArrival,
            existingFlights
        );

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void DifferentPlaneIsAvailable()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        List<FlightModel> existingFlights = new List<FlightModel>
        {
            new FlightModel
            {
                PlaneId = 1,
                DepartureDatetime = "20-11-2026 10:00",
                ArrivalDatetime = "20-11-2026 12:00"
            }
        };

        // Act
        bool result = logic.IsPlaneAvailable(
            2,
            "20-11-2026 11:00",
            "20-11-2026 13:00",
            existingFlights
        );

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void PlaneWithoutFlightsIsAvailable()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        List<FlightModel> existingFlights = new List<FlightModel>();

        // Act
        bool result = logic.IsPlaneAvailable(
            1,
            "20-11-2026 10:00",
            "20-11-2026 12:00",
            existingFlights
        );

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void PlaneWithOldDateFormatIsUnavailable()
    {
        // Arrange
        AdminFlightLogic logic = new AdminFlightLogic();

        List<FlightModel> existingFlights = new List<FlightModel>
        {
            new FlightModel
            {
                PlaneId = 1,
                DepartureDatetime = "2026-11-10 10:00",
                ArrivalDatetime = "2026-11-10 12:00"
            }
        };

        // Act
        bool result = logic.IsPlaneAvailable(
            1,
            "10-11-2026 11:00",
            "10-11-2026 13:00",
            existingFlights
        );

        // Assert
        Assert.IsFalse(result);
    }
}