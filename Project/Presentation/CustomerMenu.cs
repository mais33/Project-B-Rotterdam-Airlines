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
            Console.WriteLine("  [3] View tickets");
            Console.WriteLine("  [4] Log out");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-4): ");

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
                    TicketPresentation.ShowMyTickets(account);
                    break;

                case "4":
                    Console.WriteLine();
                    Console.WriteLine("  > You have been logged out.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid option. Please choose 1, 2, 3 or 4.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}