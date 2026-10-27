static class Menu
{

    //This method shows the menu
    static public void Start()
    {
        while (true)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Log in");
            Console.WriteLine("3. View flights");
            Console.WriteLine("Choose an option:");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Register menu");
                        break;

                    case 2:
                        UserLogin.Start();
                        break;

                    case 3:
                        Console.WriteLine("View flights");
                        break;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Please enter a number.");
            }

            Console.WriteLine();
        }
    }

    // Menu for a customer after logging in
    static public void CustomerMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Customer menu ===");
            Console.WriteLine("1. View flights");
            Console.WriteLine("2. Log out");
            Console.WriteLine("Choose an option:");

            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.WriteLine("View flights");
            }
            else if (input == "2")
            {
                Console.WriteLine("You are logged out.");
                return;
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }
    }

    // Menu for an admin after logging in
    static public void AdminMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Administrator menu ===");
            Console.WriteLine("1. Manage flights");
            Console.WriteLine("2. Manage accounts");
            Console.WriteLine("3. Log out");
            Console.WriteLine("Choose an option:");

            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.WriteLine("Manage flights");
            }
            else if (input == "2")
            {
                Console.WriteLine("Manage accounts");
            }
            else if (input == "3")
            {
                Console.WriteLine("You are logged out.");
                return;
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }
    }
}