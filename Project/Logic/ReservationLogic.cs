public class ReservationLogic
{
    private readonly ReservationAccess _reservationAccess =
        new ReservationAccess();
    private readonly SeatAccess _seatAccess =
        new SeatAccess();

    public List<FlightModel> GetFlights()
    {
        return _reservationAccess.GetFlights();
    }

    public int GetAvailableSeatCount(FlightModel flight)
    {
        return _seatAccess.CountAvailableSeatsForFlight(flight.Id, flight.PlaneId);
    }

    public bool IsValidPassengerCount(int passengerCount)
    {
        if (passengerCount <= 0)
        {
            return false;
        }

        return true;
    }

    public bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (name.Length < 3)
        {
            return false;
        }

        return true;
    }

    public bool IsValidDateOfBirth(string dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(dateOfBirth))
        {
            return false;
        }

        DateTime birthDate;

        if (!DateTime.TryParse(dateOfBirth, out birthDate))
        {
            return false;
        }

        return true;
    }

    public double CalculateTotalPrice(
        double flightPrice,
        int passengerCount)
    {
        return flightPrice * passengerCount;
    }

    public Int64 MakeReservation(
        Int64 accountId,
        Int64 flightId,
        double totalPrice)
    {
        ReservationModel reservation = new ReservationModel(
            0,
            accountId,
            flightId,
            totalPrice,
            "Confirmed"
        );

        return _reservationAccess.AddReservation(reservation);
    }

    public Int64 AddPassenger(
        string firstName,
        string lastName,
        string dateOfBirth)
    {
        PassengerModel passenger = new PassengerModel(
            0,
            firstName,
            lastName,
            dateOfBirth
        );

        return _reservationAccess.AddPassenger(passenger);
    }

    public Int64 AddPassenger(
        Int64 accountId,
        string firstName,
        string lastName,
        string dateOfBirth)
    {
        return AddPassenger(firstName, lastName, dateOfBirth);
    }

    public Int64 AddPassengerReservation(
        Int64 passengerId,
        Int64 reservationId)
    {
        PassengerReservationModel passengerReservation = new PassengerReservationModel(
            0,
            passengerId,
            reservationId
        );

        return _reservationAccess.AddPassengerReservation(passengerReservation);
    }
}
