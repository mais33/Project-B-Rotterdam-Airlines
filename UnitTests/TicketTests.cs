using Dapper;
using Microsoft.Data.Sqlite;

namespace UnitTests;

[TestClass]
public class TicketTests
{
    private string previousDirectory = "";
    private string testDirectory = "";

    [TestInitialize]
    public void SetUp()
    {
        SqliteConnection.ClearAllPools();
        previousDirectory = Directory.GetCurrentDirectory();
        testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(testDirectory, "DataSources"));
        File.Copy(Path.Combine(AppContext.BaseDirectory, "DataSources", "ticket-template.txt"),
            Path.Combine(testDirectory, "DataSources", "ticket-template.txt"));
        Directory.SetCurrentDirectory(testDirectory);
        using SqliteConnection connection = new SqliteConnection("Data Source=DataSources/database.db;Pooling=False");
        connection.Execute(@"
            CREATE TABLE Flight (id INTEGER PRIMARY KEY, plane_id INTEGER, start_airport_id INTEGER, destination_id INTEGER, price REAL, arrival_datetime TEXT, departure_datetime TEXT);
            CREATE TABLE Airport (id INTEGER PRIMARY KEY, city TEXT, name TEXT);
            CREATE TABLE Reservation (id INTEGER PRIMARY KEY, account_id INTEGER, flight_id INTEGER, ticket_price REAL, status TEXT);
            CREATE TABLE Passenger (id INTEGER PRIMARY KEY, account_id INTEGER, first_name TEXT, last_name TEXT, dob TEXT);
            INSERT INTO Airport VALUES (1, 'Rotterdam', 'RTM'), (2, 'London', 'LHR');
            INSERT INTO Flight VALUES (1, 1, 1, 2, 100, '2026-12-01 12:00', '2026-12-01 10:00');");
    }

    [TestCleanup]
    public void CleanUp()
    {
        Directory.SetCurrentDirectory(previousDirectory);
        SqliteConnection.ClearAllPools();
        Directory.Delete(testDirectory, true);
    }

    [TestMethod]
    public void MultipleBookingsUseSavedTicketsAndCurrentFlightData()
    {
        TicketLogic logic = new TicketLogic();
        logic.SaveBooking(1, 1, 100, new() { "Emre" }, new() { "Test" }, new() { "1998-01-01" });
        logic.SaveBooking(1, 1, 100, new() { "Second" }, new() { "Person" }, new() { "1990-01-01" });
        List<TicketModel> tickets = logic.GetTicketsForAccount(1);
        Assert.AreEqual(2, tickets.Count);
        Assert.AreNotEqual(tickets[0].ReservationId, tickets[1].ReservationId);
        Assert.AreNotEqual(tickets[0].Seat, tickets[1].Seat);
        Assert.AreEqual(0, logic.GetTicketsForAccount(2).Count);
        using SqliteConnection connection = new SqliteConnection("Data Source=DataSources/database.db;Pooling=False");
        connection.Execute("UPDATE Flight SET departure_datetime = '2026-12-02 10:00'");
        string text = new TicketTemplate().Create(tickets);
        StringAssert.Contains(text, "2026-12-02 10:00");
        StringAssert.Contains(text, "Rotterdam -> London");
        StringAssert.Contains(text, "Emre Test");
        StringAssert.Contains(text, "Second Person");
    }

    [TestMethod]
    public void FailedBookingRollsBackReservationPassengersAndTickets()
    {
        TicketLogic logic = new TicketLogic();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => logic.SaveBooking(1, 1, 200,
            new() { "First", "Second" }, new() { "Person", "Person" }, new() { "1990-01-01" }));
        using SqliteConnection connection = new SqliteConnection("Data Source=DataSources/database.db;Pooling=False");
        Assert.AreEqual(0, connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Reservation"));
        Assert.AreEqual(0, connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Passenger"));
        Assert.AreEqual(0, connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Ticket"));
    }
}
