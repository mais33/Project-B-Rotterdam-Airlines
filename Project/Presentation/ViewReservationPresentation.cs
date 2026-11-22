
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
            List<ReservationModel> reservations =
                _logic.GetReservationsForAccount(account.Id);

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
            !string.Equals(account.Role.ToString(), "Admin",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(account.Role.ToString(), "Administrator",
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Access denied. Admin only.");
            Pause();
            return;
        }

        try
        {
            List<ReservationModel> reservations =
                _logic.GetAllReservations();

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
        List<ReservationModel> reservations)
    {
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
            return;
        }

        foreach (ReservationModel reservation in reservations)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Reservation ID : {reservation.Id}");
            Console.WriteLine($"Account ID     : {reservation.AccountId}");
            Console.WriteLine($"Flight ID      : {reservation.FlightId}");
            Console.WriteLine($"Total price    : EUR {reservation.TicketPrice:F2}");
            Console.WriteLine($"Status         : {reservation.Status}");
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
