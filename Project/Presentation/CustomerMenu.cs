public static class CustomerMenu
{
    public static void Start(AccountModel account)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Customer Menu ===");
            Console.WriteLine("1. Make a reservation");
            Console.WriteLine("2. View tickets");
            Console.WriteLine("3. Log out");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    ReservationPresentation.Start(account);
                    break;

                case "2":
                    Console.WriteLine("View tickets is not available yet.");
                    break;

                case "3":
                    Console.WriteLine("You have been logged out.");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}