using System.Security.Cryptography.X509Certificates;

public class UserRegistration
{
    private static AccountsLogic accountsLogic = new AccountsLogic();
    private static AccountsAccess accountsAccess = new AccountsAccess();

    

    public string AskFirstName()
    {
        Console.WriteLine("What is your first name?");
        string firstName = Console.ReadLine() ?? "";

        while(!accountsLogic.NameCheck(firstName))
        {
            Console.WriteLine("Please enter a valid first name (a name can't contain anyhting else than letters)");
            firstName = Console.ReadLine()!;
        }

        return firstName;
    }

    public string AskLastName()
    {
        Console.WriteLine("What is your last name?");
        string lastName = Console.ReadLine() ?? "";

        while(!accountsLogic.NameCheck(lastName))
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

        while(!accountsLogic.CheckEmail(Email))
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

        while(accountsLogic.PhoneNumberCheck(phoneNumber) == false) //make a method an can make a do-while loop somewhere in this current method
        {
            Console.WriteLine("Please enter a valid phonenumber (no longer than 11 digits)");
            phoneNumber = Console.ReadLine()!;
        }

        return phoneNumber;
    }

    public string GetValidPassword()
    {
        while(true)
            {
                Console.WriteLine("register your password (atleast 8 characters, 1 uppercase letter, 1 digit and a symbol)");
                string password = accountsLogic.PassWordHider() ?? "";

                if(accountsLogic.PasswordCheck(password))
                {
                    return password;
                }

                Console.WriteLine("Please enter a valid password atleast 8 characters, 1 uppercase letter, 1 digit and a symbol");
            }
    }

    public string AskPassword()
    {
        string Password = "";
        bool isValid = false;


        do
        {
            Password = GetValidPassword();

            Console.WriteLine("Confirm password (or press 'B' to re-enter a new password)");
            string identicalPassword = accountsLogic.PassWordHider() ?? "";

            if (identicalPassword == Password)
            {
                isValid = true;
            }

            else if(identicalPassword == "B" || identicalPassword == "b")
            {
                Console.WriteLine("Going back to enter a new password\n");
            }
            else
            {
                Console.WriteLine("The passwords didn't match, starting over");
            }

        }while(!isValid);

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

        AccountModel registeredAccount = new(0, firstName, lastName, email, phoneNumber, UserRole.Customer, false, password);
        accountsLogic.CreateAccount(registeredAccount, password);
    }
        
}