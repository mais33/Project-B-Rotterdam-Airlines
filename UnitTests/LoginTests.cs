namespace UnitTests;

[TestClass]
public sealed class Test1
{
    [DataTestMethod]
    [DataRow("john.smith@gmail.com", "Test123!")]
    public void LoginValidCredentials(string m, string p)
    {
        AccountsLogic l = new();

        AccountModel result = l.CheckLogin(m, p);

        Assert.IsNotNull(result);
        Assert.AreEqual(m, result.Email);
    }

    [DataTestMethod]
    [DataRow("john.smith@gmail.com", "wrong")]
    [DataRow("wrong1", "Test123!")]
    [DataRow("wrong2", "wrong")]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void LoginInvalidCredentials(string m, string p)
    {
        AccountsLogic l = new();

        AccountModel result = l.CheckLogin(m, p);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void ExistingAccountBookingsLoadFromNormalizedTables()
    {
        TicketLogic ticketLogic = new();

        List<TicketModel> tickets = ticketLogic.GetTicketsForAccount(9);

        Assert.AreEqual(7, tickets.Count);
        Assert.AreEqual("1A", tickets[0].Seat);
        Assert.AreEqual("1B", tickets[1].Seat);
        Assert.AreEqual(tickets[0].BookingReference, tickets[1].BookingReference);
        Assert.AreNotEqual(tickets[0].BookingReference, tickets[2].BookingReference);
    }

    [TestMethod]
    public void MakeReservationAndAssignSeatsWorksWithErdTables()
    {
        ReservationLogic reservationLogic = new();
        TicketLogic ticketLogic = new();
        FlightLogic flightLogic = new();

        FlightModel? flight = flightLogic.GetFlightById(4);
        Assert.IsNotNull(flight);

        double totalPrice = reservationLogic.CalculateTotalPrice(flight.Price, 2);
        Int64 reservationId = reservationLogic.MakeReservation(11, flight.Id, totalPrice);
        Assert.IsTrue(reservationId > 0);

        string reference = ticketLogic.MakeBookingReference(reservationId);

        Int64 p1 = reservationLogic.AddPassenger("Alice", "Jansen", "1999-04-12");
        Int64 pr1 = reservationLogic.AddPassengerReservation(p1, reservationId);
        TicketModel t1 = ticketLogic.CreateTicket(reservationId, pr1, 11, flight, reference, "Alice", "Jansen");

        Int64 p2 = reservationLogic.AddPassenger("Bob", "DeVries", "1998-09-21");
        Int64 pr2 = reservationLogic.AddPassengerReservation(p2, reservationId);
        TicketModel t2 = ticketLogic.CreateTicket(reservationId, pr2, 11, flight, reference, "Bob", "DeVries");

        Assert.AreEqual("1A", t1.Seat);
        Assert.AreEqual("1B", t2.Seat);

        List<TicketModel> savedTickets = ticketLogic.GetTicketsForAccount(11);
        Assert.AreEqual(2, savedTickets.Count);
        Assert.AreEqual(reference, savedTickets[0].BookingReference);
        Assert.AreEqual("1A", savedTickets[0].Seat);
        Assert.AreEqual("1B", savedTickets[1].Seat);
    }
}
