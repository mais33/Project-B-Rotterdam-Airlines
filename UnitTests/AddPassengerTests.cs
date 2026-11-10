namespace UnitTests;

[TestClass]
public sealed class AddPassengerTests
{
    [DataTestMethod]
    [DataRow(1, true)]
    [DataRow(3, true)]
    [DataRow(0, false)]
    [DataRow(-2, false)]
    public void CheckPassengerCount(int count, bool expected)
    {
        ReservationLogic logic = new ReservationLogic();

        bool result = logic.IsValidPassengerCount(count);

        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow("Omid", true)]
    [DataRow("Jan", true)]
    [DataRow("Al", false)]
    [DataRow("", false)]
    [DataRow("   ", false)]
    [DataRow("1234", false)]
    [DataRow("Omid1", false)]
    public void CheckPassengerName(string name, bool expected)
    {
        ReservationLogic logic = new ReservationLogic();

        bool result = logic.IsValidName(name);

        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow("1999-05-20", true)]
    [DataRow("20-05-1999", true)]
    [DataRow("", false)]
    [DataRow("invalid", false)]
    [DataRow("2099-01-01", false)]
    public void CheckDateOfBirth(string dob, bool expected)
    {
        ReservationLogic logic = new ReservationLogic();

        bool result = logic.IsValidDateOfBirth(dob);

        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow("2099-01-01", true)]
    [DataRow("1999-05-20", false)]
    [DataRow("20-05-1999", false)]
    public void CheckIsFutureDate(string dob, bool expected)
    {
        ReservationLogic logic = new ReservationLogic();

        bool result = logic.IsFutureDate(dob);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void CalculateRoundTripTotalPrice()
    {
        ReservationLogic logic = new ReservationLogic();

        double total = logic.CalculateTotalPrice(100.0, 150.0, 2);

        Assert.AreEqual(500.0, total);
    }

    [TestMethod]
    public void LinkPassengersToReservationAndRoundTrip()
    {
        ReservationLogic logic = new ReservationLogic();

        Int64 outboundResId = logic.MakeReservation(9, 1, 259.98);
        Int64 returnResId = logic.MakeReservation(9, 2, 300.0);

        Int64 p1 = logic.AddPassenger(9, "Omid", "Fayzi", "29-01-2000");
        Int64 p2 = logic.AddPassenger(9, "Jan", "Hagel", "05-05-2000");

        logic.LinkPassengerToReservation(p1, outboundResId);
        logic.LinkPassengerToReservation(p2, outboundResId);

        logic.LinkPassengerToReservation(p1, returnResId);
        logic.LinkPassengerToReservation(p2, returnResId);

        List<PassengerReservationModel> outboundPassengers = logic.GetPassengersForReservation(outboundResId);
        List<PassengerReservationModel> returnPassengers = logic.GetPassengersForReservation(returnResId);

        Assert.AreEqual(2, outboundPassengers.Count);
        Assert.AreEqual(2, returnPassengers.Count);
        Assert.AreEqual(outboundPassengers[0].PassengerId, returnPassengers[0].PassengerId);
        Assert.AreEqual(outboundPassengers[1].PassengerId, returnPassengers[1].PassengerId);
    }
}
