static class Menu
{
    private static AccountsAccess accountsAccess = new();
    private static AccountsLogic accountsLogic = new();

    // This method shows the start menu
    public static void Start()
    {
        while (true)
        {
            Console.Clear();
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
                    Console.WriteLine();
                    Console.WriteLine("  > View flights is coming soon.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
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

    // Menu for a customer after logging in
    public static void CustomerMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                  CUSTOMER MENU                   ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine("  [1] View flights");
            Console.WriteLine("  [2] Log out");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-2): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.WriteLine();
                    Console.WriteLine("  > View flights is coming soon.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;

                case "2":
                    Console.WriteLine();
                    Console.WriteLine("  > You have been logged out.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose 1 or 2.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Menu for an admin after logging in
    public static void AdminMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("               ADMINISTRATOR MENU                 ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine("  [1] Manage flights");
            Console.WriteLine("  [2] Manage accounts");
            Console.WriteLine("  [3] Log out");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-3): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.WriteLine();
                    Console.WriteLine("  > Manage flights is coming soon.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;

                case "2":
                    AccountManager.Start();
                    break;

                case "3":
                    Console.WriteLine();
                    Console.WriteLine("  > You have been logged out.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose 1, 2 or 3.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}