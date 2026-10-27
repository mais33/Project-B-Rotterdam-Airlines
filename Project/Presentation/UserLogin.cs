static class UserLogin
{
    private static AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                     LOG IN                       ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine("  Please enter your account details:");
        Console.WriteLine();
        Console.Write("  Email address: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("  Password:      ");
        string password = accountsLogic.PassWordHider();

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");

        AccountModel acc = accountsLogic.CheckLogin(email, password);

        if (acc != null)
        {
            if (acc.Role == UserRole.Admin)
            {
                Console.WriteLine();
                Console.WriteLine("  > Welcome back, Admin!");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
                Menu.AdminMenu();
            }
            else if (acc.Role == UserRole.Customer)
            {
                Console.WriteLine();
                Console.WriteLine($"  > Welcome back, {acc.FirstName} {acc.LastName}!");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
                Menu.CustomerMenu();
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("  > No account found with that email and password.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to continue...");
            Console.ReadKey();
        }
    }
}