static class UserLogin
{
    private static UserLoginLogic userLoginLogic = new UserLoginLogic();

    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("=== Rotterdam Airlines Login ===");

        while (true)
        {
            string email = "";

            while (!userLoginLogic.IsValidEmail(email))
            {
                Console.Write("Email address: ");
                email = Console.ReadLine() ?? "";

                if (!userLoginLogic.IsValidEmail(email))
                {
                    Console.WriteLine("Invalid email address. Please try again.");
                }
            }

            string password = "";

            while (string.IsNullOrWhiteSpace(password))
            {
                Console.Write("Password: ");
                password = ReadPassword();

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password cannot be empty. Please try again.");
                }
            }

            AccountModel? account = userLoginLogic.Login(email, password);

            if (account == null)
            {
                Console.WriteLine();
                Console.WriteLine("Incorrect email address or password. Please try again.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"Welcome {account.FirstName}!");
                Console.WriteLine("Login successful.");

                CustomerMenu.Start(account);
                return;
            }
        }
    }

    private static string ReadPassword()
    {
        string password = "";

        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter &&
                     key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }

        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();

        return password;
    }
}