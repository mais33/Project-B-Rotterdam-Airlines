public class ReservationLogic
{
    private readonly ReservationAccess _reservationAccess =
        new ReservationAccess();

    public List<FlightModel> GetFlights()
    {
        return _reservationAccess.GetFlights();
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

    public void MakeReservation(
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

        _reservationAccess.AddReservation(reservation);
    }

    public void AddPassenger(
        Int64 accountId,
        string firstName,
        string lastName,
        string dateOfBirth)
    {
        PassengerModel passenger = new PassengerModel(
            0,
            accountId,
            firstName,
            lastName,
            dateOfBirth
        );

        _reservationAccess.AddPassenger(passenger);
    }
}