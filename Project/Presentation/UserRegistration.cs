using System.Security.Cryptography.X509Certificates;

public class UserRegistration
{
    private static AccountsLogic accountsLogic = new AccountsLogic();
    private static AccountsAccess accountsAccess = new AccountsAccess();



    public string AskFirstName()
    {
        Console.WriteLine("What is your first name?");
        string firstName = Console.ReadLine()!;

        while(accountsLogic.NameCheck(firstName) == false)
        {
            Console.WriteLine("Please enter a valid first name (a name can't contain anyhting else than letters)");
            firstName = Console.ReadLine()!;
        }

        return firstName;
    }

    public string AskLastName()
    {
        Console.WriteLine("What is your last name?");
        string lastName = Console.ReadLine()!;

        while(accountsLogic.NameCheck(lastName) == false)
        {
            Console.WriteLine("Please enter a valid last name (a name can't contain anyhting else than letters)");
            lastName = Console.ReadLine()!;
        }

        return lastName;
    }

    public string AskEmail()
    {
        Console.WriteLine("Please enter your email");
        string Email = Console.ReadLine()!;

        while(accountsLogic.CheckEmail(Email) == false)
        {
            Console.WriteLine("Please enter a valid email");
            Email = Console.ReadLine()!;
        }

        return Email;
    }

    public string AskPhoneNumber()
    {
        Console.WriteLine("Please enter your phonenumber");
        string phoneNumber = Console.ReadLine()!;

        while(accountsLogic.PhoneNumberCheck(phoneNumber) == false)
        {
            Console.WriteLine("Please enter a valid phonenumber (no longer than 11 digits)");
            phoneNumber = Console.ReadLine()!;
        }

        return phoneNumber;
    }

    public string AskPassword()
    {
        string Password = "";
        string identicalPassword = "";

        while (true)
        {
            while(true)
            {
                Console.WriteLine("register your password (password must contain atleast 8 characters)");
                Password = accountsLogic.PassWordHider();

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
                identicalPassword = accountsLogic.PassWordHider();

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
            return Password;
    }

    public static void Start()
    {
        UserRegistration userRegistration = new();

        string firstName = userRegistration.AskFirstName();
        string lastName = userRegistration.AskLastName();
        string email = userRegistration.AskEmail();
        string phoneNumber = userRegistration.AskPhoneNumber();
        string password = userRegistration.AskPassword();

        AccountModel registeredAccount = new(0, firstName, lastName, email, phoneNumber, "Customer", password);
        accountsLogic.CreateAccount(registeredAccount);
        // accountsAccess.ClearAccounts(); //deletes all values from Account table
    }
        
}