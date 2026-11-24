
static class Menu
{
    // This method shows the start menu
    private static AccountsLogic accountsLogic = new();
    // private static SeatAccess seatAccess = new();
    // private static AccountsAccess accountsAccess = new();
    private static TicketLogic ticketLogic = new();
    public static void Start()
    {
        while (true)
        {
            // Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("               ROTTERDAM AIRLINES                 ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine("  [1] Register");
            Console.WriteLine("  [2] Log in");
            Console.WriteLine("  [3] View flights");
            Console.WriteLine("  [4] Exit program");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-4): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    UserRegistration.Start();
                    break;

                case "2":
                    UserLogin.Start();
                    break;

                case "3":
                    FlightOverview.Start();
                    break;

                case "4":
                    Console.WriteLine();
                    Console.WriteLine("  > Thank you for visiting Rotterdam Airlines. Goodbye!");
                    Console.WriteLine();
                    return;
                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose 1, 2, 3 or 4.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Show the administrator menu after login
    public static void AdminMenu(AccountModel account)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("               ADMINISTRATOR MENU                 ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine($"  Welcome, {account.FirstName} {account.LastName}!");
            Console.WriteLine();
            Console.WriteLine("  [1] Make a reservation");
            Console.WriteLine("  [2] View flights");
            Console.WriteLine("  [3] Booking confirmations");
            Console.WriteLine("  [4] Manage accounts");
            Console.WriteLine("  [5] Add Flight");
            Console.WriteLine("  [6] View all reservations");
            Console.WriteLine("  [7] Log out");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-7): ");

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
                    AccountManager.Start();
                    break;

                case "5":
                    AdminFlightPresentation.Start();
                    break;

                case "6":
                    ViewReservationPresentation.ShowAllReservations(account);
                    break;

                case "7":
                    Console.WriteLine();
                    Console.WriteLine("  > You have been logged out.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose a number from 1 to 7.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
