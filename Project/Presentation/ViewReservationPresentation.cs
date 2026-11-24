public static class ViewReservationPresentation
{
    private static readonly ReservationLogic _logic = new ReservationLogic();

    public static void ShowMyReservations(AccountModel account)
    {
        if (account == null)
        {
            Console.WriteLine("You must log in first.");
            Pause();
            return;
        }

        try
        {
            List<ReservationDetailsModel> reservations =
                _logic.GetReservationDetailsForAccount(account.Id);

            Console.Clear();
            Console.WriteLine("=== MY RESERVATIONS ===");
            Console.WriteLine();

            DisplayReservations(reservations);
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to load reservations.");
        }

        Pause();
    }

    public static void ShowAllReservations(AccountModel account)
    {
        if (account == null ||
            (!string.Equals(account.Role.ToString(), "Admin",
                StringComparison.OrdinalIgnoreCase) &&
             !string.Equals(account.Role.ToString(), "Administrator",
                StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Access denied. Admin only.");
            Pause();
            return;
        }

        try
        {
            List<ReservationDetailsModel> reservations =
                _logic.GetAllReservationDetails();

            Console.Clear();
            Console.WriteLine("=== ALL CUSTOMER RESERVATIONS ===");
            Console.WriteLine();

            DisplayReservations(reservations);
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to load reservations.");
        }

        Pause();
    }

    private static void DisplayReservations(
        List<ReservationDetailsModel> reservations)
    {
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
            return;
        }

        foreach (ReservationDetailsModel reservation in reservations)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Reservation ID : {reservation.ReservationId}");
            Console.WriteLine($"Customer       : {reservation.CustomerName}");
            Console.WriteLine($"Route          : {reservation.DepartureCity} -> {reservation.DestinationCity}");
            Console.WriteLine($"Departure      : {reservation.DepartureDateTime}");

            Console.WriteLine("Passengers:");

            if (reservation.Passengers.Count == 0)
            {
                Console.WriteLine("  No passenger details found.");
            }
            else
            {
                foreach (ReservationPassengerDetailsModel passenger
                    in reservation.Passengers)
                {
                    Console.WriteLine(
                        $"  - {passenger.FirstName} {passenger.LastName} | Seat: {passenger.SeatNumber}");
                }
            }

            Console.WriteLine($"Total price    : EUR {reservation.TotalPrice:F2}");
            Console.WriteLine($"Status         : {reservation.Status}");

            if (reservation.BookingGroupId.HasValue)
            {
                Console.WriteLine(
                    $"Booking Group  : {reservation.BookingGroupId.Value}");
            }
        }

        Console.WriteLine("--------------------------------");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }
}
