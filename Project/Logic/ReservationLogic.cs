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

    public List<FlightModel> GetValidReturnFlights(FlightModel outboundFlight, List<FlightModel> flights)
    {
        List<FlightModel> validReturnFlights = new List<FlightModel>();

        for (int i = 0; i < flights.Count; i++)
        {
            if (IsValidReturnFlight(outboundFlight, flights[i]))
            {
                validReturnFlights.Add(flights[i]);
            }
        }

        return validReturnFlights;
    }

    public bool IsValidReturnFlight(FlightModel outboundFlight, FlightModel returnFlight)
    {
        if (returnFlight.StartAirportId != outboundFlight.DestinationId ||
            returnFlight.DestinationId != outboundFlight.StartAirportId)
        {
            return false;
        }

        if (!TryParseFlightDateTime(outboundFlight.ArrivalDatetime, out DateTime outboundArrival) ||
            !TryParseFlightDateTime(returnFlight.DepartureDatetime, out DateTime returnDeparture))
        {
            return false;
        }

        return returnDeparture > outboundArrival;
    }

    private bool TryParseFlightDateTime(string value, out DateTime dateTime)
    {
        return DateLogic.TryReadDateTime(value, out dateTime);
    }

    public bool IsValidPassengerCount(int passengerCount)
    {
        if (passengerCount <= 0)
        {
            return false;
        }

        return true;
    }

    public List<Int64> SaveCompleteReservation(
    Int64 accountId,
    FlightModel outboundFlight,
    FlightModel? returnFlight,
    List<PassengerModel> passengers,
    List<Int64> outboundSeatIds,
    List<Int64>? returnSeatIds)
{
    return _reservationAccess.SaveCompleteReservation(
        accountId,
        outboundFlight,
        returnFlight,
        passengers,
        outboundSeatIds,
        returnSeatIds
    );
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

        DateTime birthDate;

        bool parsed = DateLogic.TryReadDate(dateOfBirth.Trim(), out birthDate);

        if (!parsed)
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

        DateTime birthDate;

        bool parsed = DateLogic.TryReadDate(dateOfBirth.Trim(), out birthDate);

        if (!parsed)
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


    public List<ReservationModel> GetReservationsForAccount(Int64 accountId)
    {
        if (accountId <= 0)
        {
            throw new ArgumentException("Invalid account ID.");
        }

        return _reservationAccess.GetReservationsByAccountId(accountId);
    }

    public List<ReservationModel> GetAllReservations()
    {
        return _reservationAccess.GetAllReservations();
    }



    public List<ReservationDetailsModel> GetReservationDetailsForAccount(
        long accountId)
    {
        if (accountId <= 0)
        {
            throw new ArgumentException("Invalid account ID.");
        }

        return _reservationAccess.GetReservationDetails(accountId);
    }

    public List<ReservationDetailsModel> GetAllReservationDetails()
    {
        return _reservationAccess.GetReservationDetails();
    }

    public List<Int64> ConfirmReservation(
        Int64 accountId,
        FlightModel outboundFlight,
        FlightModel? returnFlight,
        List<PassengerModel> passengers,
        List<Int64> outboundSeatIds,
        List<Int64>? returnSeatIds = null)
    {
        if (accountId <= 0)
        {
            throw new ArgumentException("Invalid account ID.");
        }

        if (outboundFlight == null)
        {
            throw new ArgumentException("An outbound flight is required.");
        }

        if (passengers == null || passengers.Count == 0)
        {
            throw new ArgumentException("At least one passenger is required.");
        }

        if (outboundSeatIds == null ||
            outboundSeatIds.Count != passengers.Count)
        {
            throw new ArgumentException(
                "Each passenger must have an outbound seat.");
        }

        if (returnFlight != null &&
            (returnSeatIds == null ||
             returnSeatIds.Count != passengers.Count))
        {
            throw new ArgumentException(
                "Each passenger must have a return seat.");
        }

        if (returnFlight == null &&
            returnSeatIds != null &&
            returnSeatIds.Count > 0)
        {
            throw new ArgumentException(
                "Return seats require a return flight.");
        }

        foreach (PassengerModel passenger in passengers)
        {
            if (!IsValidName(passenger.FirstName) ||
                !IsValidName(passenger.LastName) ||
                !IsValidDateOfBirth(passenger.Dob))
            {
                throw new ArgumentException(
                    "Passenger information is invalid.");
            }
        }

        if (outboundSeatIds.Distinct().Count() != outboundSeatIds.Count)
        {
            throw new ArgumentException(
                "Outbound seats must be unique.");
        }

        if (returnSeatIds != null &&
            returnSeatIds.Distinct().Count() != returnSeatIds.Count)
        {
            throw new ArgumentException(
                "Return seats must be unique.");
        }

        // The DataAccess method will save reservations,
        // passengers and tickets in one database transaction.
        return _reservationAccess.SaveCompleteReservation(
            accountId,
            outboundFlight,
            returnFlight,
            passengers,
            outboundSeatIds,
            returnSeatIds
        );
    }


}
