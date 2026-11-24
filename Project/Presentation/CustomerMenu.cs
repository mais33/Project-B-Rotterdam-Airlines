public static class CustomerMenu
{

    public static void Start(AccountModel account)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                  CUSTOMER MENU                   ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine($"  Welcome, {account.FirstName} {account.LastName}!");
            Console.WriteLine();
            Console.WriteLine("  [1] Make a reservation");
            Console.WriteLine("  [2] View flights");
            Console.WriteLine("  [3] Booking Confiramtions");
            Console.WriteLine("  [4] View my reservations");
            Console.WriteLine("  [5] View tickets");
            Console.WriteLine("  [6] Log out");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-5): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    ReservationPresentation.Start(account);
                    break;

                case "2":
                    FlightOverview.Start();
                    break;

                case "3":
                    break;

                case "4":
                    ViewReservationPresentation.ShowMyReservations(account);
                    break;
                case "5":
                    TicketPresentation.Start(account);
                    break;
                case "6":
                    Console.WriteLine();
                    Console.WriteLine("  > You have been logged out.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid option. Please choose a number from 1 to 5.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}