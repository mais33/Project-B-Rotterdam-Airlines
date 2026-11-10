public class UserRegistration
{
    private static AccountsLogic accountsLogic = new AccountsLogic();
    private static AccountsAccess accountsAccess = new AccountsAccess();

    public string AskFirstName()
    {
        while (true)
        {
            Console.Write("  First name (or press Enter to return): ");
            string firstName = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(firstName))
            {
                return "";
            }

            if (accountsLogic.NameCheck(firstName))
            {
                return firstName;
            }

            Console.WriteLine();
            Console.WriteLine("  > A name can only contain letters. Please try again.");
            Console.WriteLine();
        }
    }

    public string AskLastName()
    {
        while (true)
        {
            Console.Write("  Last name:      ");
            string lastName = (Console.ReadLine() ?? "").Trim();

            if (accountsLogic.NameCheck(lastName))
            {
                return lastName;
            }

            Console.WriteLine();
            Console.WriteLine("  > A name can only contain letters. Please try again.");
            Console.WriteLine();
        }
    }

    public string AskEmail()
    {
        while (true)
        {
            Console.Write("  Email address:  ");
            string email = (Console.ReadLine() ?? "").Trim();

            if (accountsLogic.CheckEmail(email))
            {
                return email;
            }

            Console.WriteLine();
            Console.WriteLine("  > Please enter a valid email address. Please try again.");
            Console.WriteLine();
        }
    }

    public string AskPhoneNumber()
    {
        while (true)
        {
            Console.Write("  Phone number:   ");
            string phoneNumber = (Console.ReadLine() ?? "").Trim();

            if (accountsLogic.PhoneNumberCheck(phoneNumber))
            {
                return phoneNumber;
            }

            Console.WriteLine();
            Console.WriteLine("  > Invalid phone number (e.g. 0612345678 or +353 87 123 4567).");
            Console.WriteLine();
        }
    }

    public string GetValidPassword()
    {
        while (true)
        {
            Console.Write("  Password:         ");
            string password = accountsLogic.PassWordHider() ?? "";

            if (accountsLogic.PasswordCheck(password))
            {
                return password;
            }

            Console.WriteLine();
            Console.WriteLine("  > Password is not strong enough. Please try again.");
            Console.WriteLine();
        }
    }

    public string AskPassword()
    {
        string password = "";
        bool isValid = false;

        Console.WriteLine();
        Console.WriteLine("  ------------------------------------------------");
        Console.WriteLine("  Password requirements:");
        Console.WriteLine("  - At least 8 characters");
        Console.WriteLine("  - At least 1 uppercase letter, 1 digit & 1 symbol");
        Console.WriteLine("  ------------------------------------------------");
        Console.WriteLine();

        do
        {
            password = GetValidPassword();

            Console.Write("  Confirm password: ");
            string confirmPassword = accountsLogic.PassWordHider() ?? "";

            if (confirmPassword == password)
            {
                isValid = true;
            }
            else if (confirmPassword == "B" || confirmPassword == "b")
            {
                Console.WriteLine();
                Console.WriteLine("  > Choose a new password:");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("  > Passwords do not match. (or type 'B' to change)");
                Console.WriteLine();
            }

        } while (!isValid);

        return password;
    }

    public static void Start()
    {
        UserRegistration userRegistration = new();

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                    REGISTER                      ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine("  Please enter your personal details:");
        Console.WriteLine();

        string firstName = userRegistration.AskFirstName();
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return;
        }

        string lastName = userRegistration.AskLastName();
        string email = userRegistration.AskEmail();
        string phoneNumber = userRegistration.AskPhoneNumber();
        string password = userRegistration.AskPassword();

        AccountModel registeredAccount = new(0, firstName, lastName, email, phoneNumber, UserRole.Customer, false, password);
        accountsLogic.CreateAccount(registeredAccount, password);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("  > Account successfully created! You can now log in.");
        Console.WriteLine();
        Console.WriteLine("  Press any key to continue...");
        Console.ReadKey();
    }
}