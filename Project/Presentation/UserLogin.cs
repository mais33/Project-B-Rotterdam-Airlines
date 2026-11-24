
static class UserLogin
{
    private static AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                     LOG IN                       ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine("  Please enter your account details:");
            Console.WriteLine();
            Console.Write("  Email address (or press Enter to return): ");
            string email = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            email = email.Trim();

            if (!accountsLogic.CheckEmail(email))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  > Please enter a valid email address. Please try again.");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("  Press any key to try again...");
                Console.ReadKey();
                continue;
            }

            Console.Write("  Password (or press Enter to return): ");
            string password = accountsLogic.PassWordHider();

            if (password == "")
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");

            AccountModel acc = accountsLogic.CheckLogin(email, password);

            if (acc != null)
            {
                if (acc.Role == UserRole.Admin)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  > Welcome back, Admin!");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    Menu.AdminMenu(acc);
                    return;
                }
                else if (acc.Role == UserRole.Customer)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  > Welcome back, {acc.FirstName} {acc.LastName}!");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    CustomerMenu.Start(acc);
                    return;
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  > Incorrect email address or password.");
                Console.WriteLine("  > Please try again.");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("  Press any key to try again...");
                Console.ReadKey();
            }
        }
    }
}
