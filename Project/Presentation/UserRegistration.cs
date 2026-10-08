using System.Security.Cryptography.X509Certificates;

public class UserRegistration
{
    private static AccountsLogic accountsLogic = new AccountsLogic();
    private static AccountsAccess accountsAccess = new AccountsAccess();

    public static string PassWordHider()
    {
        string input = "";

        while (true)
        {
            ConsoleKeyInfo userInput = Console.ReadKey(true);

            if (userInput.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (userInput.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(userInput.KeyChar))
            {
                input += userInput.KeyChar;
                Console.Write("*");
            }
        }

        return input;
    }

    public static void Start()
    {
        Console.WriteLine("What is your first name?");
        string firstName = Console.ReadLine()!;

        Console.WriteLine("What is your last name?");
        string lastName = Console.ReadLine()!;

        while(accountsLogic.NameCheck(firstName, lastName) == false)
        {
            Console.WriteLine("Please enter a valid name (a name can't contain anyhting else than letters)");
            Console.WriteLine("Enter first name: ");
            firstName = Console.ReadLine()!;
            Console.WriteLine("Enter last name: ");
            lastName = Console.ReadLine()!;
        }

        Console.WriteLine("Please enter your email");
        string Email = Console.ReadLine()!;

        while(accountsLogic.CheckEmail(Email) == false)
        {
            Console.WriteLine("Please enter a valid email");
            Email = Console.ReadLine()!;
        }

        Console.WriteLine("Please enter your phonenumber");
        string phoneNumber = Console.ReadLine()!;

        while(accountsLogic.PhoneNumberCheck(phoneNumber) == false)
        {
            Console.WriteLine("Please enter a valid phonenumber (no longer than 11 digits)");
            phoneNumber = Console.ReadLine()!;
        }

        string Password = "";
        string identicalPassword = "";

        while (true)
        {
            while(true)
            {
                Console.WriteLine("register your password (password must contain atleast 8 characters)");
                Password = PassWordHider();

                if(accountsLogic.PasswordCheck(Password!))
                {
                    break;
                }

                Console.WriteLine("Please enter a valid password (at least 8 characters).");
                
            }

            bool startOver = false;

            while(true)
            {
                Console.WriteLine("Confirm password");
                identicalPassword = PassWordHider();

                if (identicalPassword == Password)
                {
                    break;
                }

                if(identicalPassword == "B" || identicalPassword == "b")
                {
                    Console.WriteLine("Going back to enter a new password");
                    startOver = true;
                    break;
                }

                Console.WriteLine("The passwords didn't match, please try again. or press (B) to enter a new password");
            }
            if (!startOver)
            {
                break;
            }
        }
        

        accountsLogic.CreateAccount(firstName, lastName, Email, phoneNumber, Password);
        accountsAccess.ClearAccounts(); //deletes all values from Account table

        
    }
}