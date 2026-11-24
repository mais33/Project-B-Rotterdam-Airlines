
using Microsoft.Data.Sqlite;
using Dapper;

public class ReservationAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public void CreatePassengerReservationTable()
    {
        string sql = @"
            CREATE TABLE IF NOT EXISTS PassengerReservation (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                passenger_id INTEGER,
                reservation_id INTEGER,
                FOREIGN KEY (passenger_id) REFERENCES Passenger(id),
                FOREIGN KEY (reservation_id) REFERENCES Reservation(id)
            )";

        _connection.Execute(sql);
    }

    public List<FlightModel> GetFlights()
    {
        const string sql = @"
            SELECT
                id AS Id,
                plane_id AS PlaneId,
                destination_id AS DestinationId,
                start_airport_id AS StartAirportId,
                price AS Price,
                arrival_datetime AS ArrivalDatetime,
                departure_datetime AS DepartureDatetime
            FROM Flight";

        return _connection.Query<FlightModel>(sql).ToList();
    }

    public Int64 AddReservation(ReservationModel reservation)
    {
        const string sql = @"
            INSERT INTO Reservation
                (account_id, flight_id, ticket_price, status, booking_group_id)
            VALUES
                (@AccountId, @FlightId, @TicketPrice, @Status, @BookingGroupId);

            SELECT last_insert_rowid();";

        reservation.Id =
            _connection.ExecuteScalar<Int64>(sql, reservation);

        return reservation.Id;
    }

    public Int64 AddPassenger(PassengerModel passenger)
    {
        const string sql = @"
            INSERT INTO Passenger
                (first_name, last_name, date_of_birth, passport_number)
            VALUES
                (@FirstName, @LastName, @Dob, NULL);

            SELECT last_insert_rowid();";

        passenger.Id =
            _connection.ExecuteScalar<Int64>(sql, passenger);

        return passenger.Id;
    }

    public Int64 AddPassengerReservation(
        PassengerReservationModel passengerReservation)
    {
        CreatePassengerReservationTable();

        const string sql = @"
            INSERT INTO PassengerReservation
                (passenger_id, reservation_id)
            VALUES
                (@PassengerId, @ReservationId);

            SELECT last_insert_rowid();";

        passengerReservation.Id =
            _connection.ExecuteScalar<Int64>(
                sql,
                passengerReservation
            );

        return passengerReservation.Id;
    }

    public List<PassengerReservationModel> GetByReservationId(
        Int64 reservationId)
    {
        CreatePassengerReservationTable();

        const string sql = @"
            SELECT
                id AS Id,
                passenger_id AS PassengerId,
                reservation_id AS ReservationId
            FROM PassengerReservation
            WHERE reservation_id = @ReservationId";

        return _connection.Query<PassengerReservationModel>(
            sql,
            new { ReservationId = reservationId }
        ).ToList();
    }

    public List<ReservationModel> GetReservationsByAccountId(
        Int64 accountId)
    {
        const string sql = @"
            SELECT
                id AS Id,
                account_id AS AccountId,
                flight_id AS FlightId,
                ticket_price AS TicketPrice,
                status AS Status,
                booking_group_id AS BookingGroupId
            FROM Reservation
            WHERE account_id = @AccountId
            ORDER BY id DESC";

        return _connection.Query<ReservationModel>(
            sql,
            new { AccountId = accountId }
        ).ToList();
    }

    public List<ReservationModel> GetAllReservations()
    {
        const string sql = @"
            SELECT
                id AS Id,
                account_id AS AccountId,
                flight_id AS FlightId,
                ticket_price AS TicketPrice,
                status AS Status,
                booking_group_id AS BookingGroupId
            FROM Reservation
            ORDER BY id DESC";

        return _connection.Query<ReservationModel>(sql).ToList();
    }

    public List<Int64> SaveCompleteReservation(
        Int64 accountId,
        FlightModel outboundFlight,
        FlightModel? returnFlight,
        List<PassengerModel> passengers,
        List<Int64> outboundSeatIds,
        List<Int64>? returnSeatIds)
    {
        using var connection = new SqliteConnection(
            "Data Source=DataSources/database.db");

        connection.Open();

        // Enable foreign key validation.
        connection.Execute("PRAGMA foreign_keys = ON;");

        using var transaction = connection.BeginTransaction();

        try
        {
            if (accountId <= 0 ||
                outboundFlight == null ||
                passengers == null ||
                passengers.Count == 0)
            {
                throw new InvalidOperationException(
                    "Invalid reservation information.");
            }

            if (outboundSeatIds == null ||
                outboundSeatIds.Count != passengers.Count)
            {
                throw new InvalidOperationException(
                    "Each passenger must have an outbound seat.");
            }

            if (returnFlight != null &&
                (returnSeatIds == null ||
                 returnSeatIds.Count != passengers.Count))
            {
                throw new InvalidOperationException(
                    "Each passenger must have a return seat.");
            }

            if (returnFlight == null &&
                returnSeatIds != null &&
                returnSeatIds.Count > 0)
            {
                throw new InvalidOperationException(
                    "Return seats require a return flight.");
            }

            // Validate seats for the specific outbound flight.
            ValidateSelectedSeats(
                outboundFlight.Id,
                outboundSeatIds);

            // Validate seats for the specific return flight.
            if (returnFlight != null && returnSeatIds != null)
            {
                ValidateSelectedSeats(
                    returnFlight.Id,
                    returnSeatIds);
            }

            // Create outbound reservation.
            Int64 outboundReservationId = InsertReservation(
                outboundFlight.Id,
                outboundSeatIds,
                null);

            List<Int64> reservationIds = new()
            {
                outboundReservationId
            };

            Int64? returnReservationId = null;

            if (returnFlight != null && returnSeatIds != null)
            {
                returnReservationId = InsertReservation(
                    returnFlight.Id,
                    returnSeatIds,
                    outboundReservationId);

                reservationIds.Add(returnReservationId.Value);

                // Link outbound and return reservations.
                connection.Execute(
                    @"UPDATE Reservation
                      SET booking_group_id = @BookingGroupId
                      WHERE id = @ReservationId",
                    new
                    {
                        BookingGroupId = outboundReservationId,
                        ReservationId = outboundReservationId
                    },
                    transaction);
            }

            // Save passengers and link them to tickets.
            for (int i = 0; i < passengers.Count; i++)
            {
                PassengerModel passenger = passengers[i];

                Int64 passengerId =
                    connection.ExecuteScalar<Int64>(
                        @"INSERT INTO Passenger
                            (first_name,
                             last_name,
                             date_of_birth,
                             passport_number)
                          VALUES
                            (@FirstName,
                             @LastName,
                             @DateOfBirth,
                             NULL);

                          SELECT last_insert_rowid();",
                        new
                        {
                            passenger.FirstName,
                            passenger.LastName,
                            DateOfBirth = passenger.Dob
                        },
                        transaction);

                InsertTicket(
                    passengerId,
                    outboundReservationId,
                    outboundSeatIds[i]);

                if (returnReservationId.HasValue &&
                    returnSeatIds != null)
                {
                    InsertTicket(
                        passengerId,
                        returnReservationId.Value,
                        returnSeatIds[i]);
                }
            }

            transaction.Commit();

            return reservationIds;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

        // Check seat availability for a specific flight.
        // Do not update Seat.is_reserved globally.
        void ValidateSelectedSeats(
            Int64 flightId,
            List<Int64> seatIds)
        {
            if (flightId <= 0)
            {
                throw new InvalidOperationException(
                    "Invalid flight ID.");
            }

            if (seatIds.Distinct().Count() != seatIds.Count)
            {
                throw new InvalidOperationException(
                    "Duplicate seats are not allowed.");
            }

            foreach (Int64 seatId in seatIds)
            {
                int availableSeats =
                    connection.ExecuteScalar<int>(
                        @"SELECT COUNT(*)
                          FROM Seat s
                          INNER JOIN Flight f
                              ON f.plane_id = s.plane_id
                          WHERE f.id = @FlightId
                            AND s.id = @SeatId
                            AND NOT EXISTS (
                                SELECT 1
                                FROM Ticket t
                                INNER JOIN Reservation r
                                    ON r.id = t.reservation_id
                                WHERE t.seat_id = s.id
                                  AND r.flight_id = @FlightId
                                  AND r.status = 'Confirmed'
                            )",
                        new
                        {
                            FlightId = flightId,
                            SeatId = seatId
                        },
                        transaction);

                if (availableSeats != 1)
                {
                    throw new InvalidOperationException(
                        $"Seat {seatId} is unavailable for flight {flightId}.");
                }
            }
        }

        // Create reservation and calculate its total price.
        Int64 InsertReservation(
            Int64 flightId,
            List<Int64> seatIds,
            Int64? bookingGroupId)
        {
            double? flightPrice =
                connection.QuerySingleOrDefault<double?>(
                    @"SELECT price
                      FROM Flight
                      WHERE id = @FlightId",
                    new { FlightId = flightId },
                    transaction);

            if (!flightPrice.HasValue)
            {
                throw new InvalidOperationException(
                    $"Flight {flightId} does not exist.");
            }

            double seatTotal = 0;

            foreach (Int64 seatId in seatIds)
            {
                double? seatPrice =
                    connection.QuerySingleOrDefault<double?>(
                        @"SELECT price
                          FROM Seat
                          WHERE id = @SeatId",
                        new { SeatId = seatId },
                        transaction);

                if (!seatPrice.HasValue)
                {
                    throw new InvalidOperationException(
                        $"Seat {seatId} does not exist.");
                }

                seatTotal += seatPrice.Value;
            }

            double totalPrice =
                flightPrice.Value * passengers.Count + seatTotal;

            return connection.ExecuteScalar<Int64>(
                @"INSERT INTO Reservation
                    (account_id,
                     flight_id,
                     ticket_price,
                     status,
                     booking_group_id)
                  VALUES
                    (@AccountId,
                     @FlightId,
                     @TicketPrice,
                     @Status,
                     @BookingGroupId);

                  SELECT last_insert_rowid();",
                new
                {
                    AccountId = accountId,
                    FlightId = flightId,
                    TicketPrice = totalPrice,
                    Status = "Confirmed",
                    BookingGroupId = bookingGroupId
                },
                transaction);
        }

        // Link passenger, reservation and seat.
        void InsertTicket(
            Int64 passengerId,
            Int64 reservationId,
            Int64 seatId)
        {
            connection.Execute(
                @"INSERT INTO Ticket
                    (passenger_id,
                     reservation_id,
                     seat_id)
                  VALUES
                    (@PassengerId,
                     @ReservationId,
                     @SeatId)",
                new
                {
                    PassengerId = passengerId,
                    ReservationId = reservationId,
                    SeatId = seatId
                },
                transaction);
        }
    }

    public List<ReservationDetailsModel> GetReservationDetails(long? accountId = null)
    {
        const string sql = @"
            SELECT
                r.id AS ReservationId,
                r.account_id AS AccountId,
                a.first_name || ' ' || a.last_name AS CustomerName,
                r.flight_id AS FlightId,
                departure.city AS DepartureCity,
                destination.city AS DestinationCity,
                f.departure_datetime AS DepartureDateTime,
                r.ticket_price AS TotalPrice,
                r.status AS Status,
                r.booking_group_id AS BookingGroupId,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                s.seat_number AS SeatNumber
            FROM Reservation r
            LEFT JOIN Account a ON a.id = r.account_id
            LEFT JOIN Flight f ON f.id = r.flight_id
            LEFT JOIN Airport departure ON departure.id = f.start_airport_id
            LEFT JOIN Airport destination ON destination.id = f.destination_id
            LEFT JOIN Ticket t ON t.reservation_id = r.id
            LEFT JOIN Passenger p ON p.id = t.passenger_id
            LEFT JOIN Seat s ON s.id = t.seat_id
            WHERE (@AccountId IS NULL OR r.account_id = @AccountId)
            ORDER BY r.id DESC, t.id";

        var rows = _connection.Query(sql, new { AccountId = accountId });
        var reservations = new Dictionary<long, ReservationDetailsModel>();

        foreach (var row in rows)
        {
            long reservationId = (long)row.ReservationId;

            if (!reservations.TryGetValue(reservationId, out var reservation))
            {
                reservation = new ReservationDetailsModel
                {
                    ReservationId = reservationId,
                    AccountId = (long)row.AccountId,
                    CustomerName = (string?)row.CustomerName ?? "",
                    FlightId = (long)row.FlightId,
                    DepartureCity = (string?)row.DepartureCity ?? "",
                    DestinationCity = (string?)row.DestinationCity ?? "",
                    DepartureDateTime = (string?)row.DepartureDateTime ?? "",
                    TotalPrice = Convert.ToDouble(row.TotalPrice),
                    Status = (string?)row.Status ?? "",
                    BookingGroupId = row.BookingGroupId == null
                        ? null
                        : (long?)row.BookingGroupId
                };

                reservations.Add(reservationId, reservation);
            }

            if (row.FirstName != null || row.LastName != null)
            {
                reservation.Passengers.Add(
                    new ReservationPassengerDetailsModel
                    {
                        FirstName = (string?)row.FirstName ?? "",
                        LastName = (string?)row.LastName ?? "",
                        SeatNumber = (string?)row.SeatNumber ?? ""
                    });
            }
        }

        return reservations.Values.ToList();
    }

}
