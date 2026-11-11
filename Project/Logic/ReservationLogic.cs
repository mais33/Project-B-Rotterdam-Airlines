public class ReservationLogic
{
    private readonly ReservationAccess _reservationAccess =
        new ReservationAccess();

    public List<FlightModel> GetFlights()
    {
        return _reservationAccess.GetFlights();
    }

    public List<FlightModel> GetReturnFlights(FlightModel outboundFlight)
    {
        List<FlightModel> allFlights = _reservationAccess.GetFlights();
        List<FlightModel> returnFlights = new List<FlightModel>();

        for (int i = 0; i < allFlights.Count; i++)
        {
            if (allFlights[i].StartAirportId == outboundFlight.DestinationId &&
                allFlights[i].DestinationId == outboundFlight.StartAirportId)
            {
                returnFlights.Add(allFlights[i]);
            }
        }

        return returnFlights;
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

        string trimmed = name.Trim();

        if (trimmed.Length < 3)
        {
            return false;
        }

        if (!trimmed.All(char.IsLetter))
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

        string[] formats = { "dd-MM-yyyy", "yyyy-MM-dd", "d-M-yyyy", "dd/MM/yyyy", "yyyy/MM/dd" };
        DateTime birthDate;

        bool parsed = DateTime.TryParseExact(
            dateOfBirth.Trim(),
            formats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out birthDate
        );

        if (!parsed && !DateTime.TryParse(dateOfBirth, out birthDate))
        {
            return false;
        }

        if (birthDate > DateTime.Today || birthDate.Year < 1900)
        {
            return false;
        }

        return true;
    }

    public bool IsFutureDate(string dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(dateOfBirth))
        {
            return false;
        }

        string[] formats = { "dd-MM-yyyy", "yyyy-MM-dd", "d-M-yyyy", "dd/MM/yyyy", "yyyy/MM/dd" };
        DateTime birthDate;

        bool parsed = DateTime.TryParseExact(
            dateOfBirth.Trim(),
            formats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out birthDate
        );

        if (!parsed && !DateTime.TryParse(dateOfBirth, out birthDate))
        {
            return false;
        }

        return birthDate > DateTime.Today;
    }

    public double CalculateTotalPrice(
        double flightPrice,
        int passengerCount)
    {
        return flightPrice * passengerCount;
    }

    public double CalculateTotalPrice(
        double outboundPrice,
        double returnPrice,
        int passengerCount)
    {
        return (outboundPrice + returnPrice) * passengerCount;
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
        Int64 accountId,
        string firstName,
        string lastName,
        string dateOfBirth)
    {
        PassengerModel passenger = new PassengerModel(
            0,
            accountId,
            firstName.Trim(),
            lastName.Trim(),
            dateOfBirth.Trim()
        );

        return _reservationAccess.AddPassenger(passenger);
    }

    public Int64 LinkPassengerToReservation(
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

    public List<PassengerReservationModel> GetPassengersForReservation(Int64 reservationId)
    {
        return _reservationAccess.GetByReservationId(reservationId);
    }
}
