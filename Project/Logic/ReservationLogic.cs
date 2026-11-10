public class ReservationLogic
{
    private readonly ReservationAccess _reservationAccess =
        new ReservationAccess();
    private readonly SeatAccess _seatAccess =
        new SeatAccess();
    private readonly TicketAccess _ticketAccess =
        new TicketAccess();

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

        string trimmed = name.Trim();

        if (trimmed.Length < 3)
        {
            return false;
        }

        for (int i = 0; i < trimmed.Length; i++)
        {
            char c = trimmed[i];
            if (!char.IsLetter(c) && c != ' ' && c != '-')
            {
                return false;
            }
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
        string firstName,
        string lastName,
        string dateOfBirth,
        string passportNumber = "")
    {
        PassengerModel passenger = new PassengerModel(
            0,
            firstName,
            lastName,
            dateOfBirth,
            passportNumber
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

    public Int64 LinkPassengerToReservation(
        Int64 passengerId,
        Int64 reservationId,
        Int64 seatId = 0)
    {
        if (seatId <= 0)
        {
            SeatModel? seat = _seatAccess.GetNextAvailableSeatForFlight(1, 1);
            if (seat != null)
            {
                seatId = seat.Id;
            }
        }

        TicketModel ticket = new TicketModel(
            0,
            passengerId,
            reservationId,
            seatId
        );

        return _ticketAccess.AddTicket(ticket);
    }

    public List<TicketModel> GetTicketsForReservation(Int64 reservationId)
    {
        return _ticketAccess.GetByReservationId(reservationId);
    }
}
