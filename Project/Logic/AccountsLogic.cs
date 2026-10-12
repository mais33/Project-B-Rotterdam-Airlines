
using Microsoft.AspNetCore.Identity;
//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }
    private AccountsAccess _access = new();
    private readonly PasswordHasher<AccountModel> _hasher = new();

    public AccountsLogic()
    {
        // Could do something here

    }

    public AccountModel CheckLogin(string email, string password)
    {
        password = PassWordHider();

        AccountModel acc = _access.GetByEmail(email);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null!;
    }

    public bool IsAdmin(AccountModel account)
    {
        return account != null && account.Role == "Admin";
    }

    public bool CreateAccount(string firstName, string lastName, string email, string phoneNumber, string password)
    {
        AccountModel newAccount = new AccountModel(0, firstName, lastName, email, phoneNumber, "User", password);

        newAccount.Password = _hasher.HashPassword(newAccount, password);

        _access.Write(newAccount);

        return true;
    }

    public bool PasswordCheck(string password)
    {
        if(string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            return false;
        }

        if(!password.Any(char.IsUpper) || !password.Any(char.IsLetter) || !password.Any(char.IsDigit) || !password.Any(c => !char.IsLetterOrDigit(c)))
        {
            return false;
        }

        return true;
    }

    public string PassWordHider()
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

    public bool NameCheck(string Name)
    {
        if(string.IsNullOrWhiteSpace(Name) || !Name.All(char.IsLetter))
        {
            return false;
        }

        return true;
    }

    public bool CheckEmail(string email)
    {
        int atIndex = email.IndexOf('@');
        int dotIndex = email.IndexOf('.');

        bool IsAtBeforeDot = dotIndex > atIndex;

        if(string.IsNullOrWhiteSpace(email) ||!email.Contains("@") || !email.Contains(".") || email.StartsWith("@") || 
            email.StartsWith(".")|| email.EndsWith("@") || email.EndsWith(".") || !IsAtBeforeDot)
        {
            return false;
        }

        return true;
    }

    public bool PhoneNumberCheck(string phoneNumber)
    {
        if(string.IsNullOrWhiteSpace(phoneNumber) || !phoneNumber.All(char.IsDigit) || phoneNumber.Length > 11)
        {
            return false;
        }

        return true;
    }

    public void CreateAccount(AccountModel account)
    {
        if(account == null)
        {
            return;
        }
        
        _access.Write(account!);   
    }

    public bool EditAccount(AccountModel account)
    {
        if(account != null)
        {
            return false;
        }

        if(account.Role == "Admin")
        {
            _access.Update(account!);
            return true;
        }

        Console.WriteLine("You are not allowed to edit an account");
        return false;
    }

    public bool BlockAccount(AccountModel account)
    {
        if(account != null)
        {
            return false;
        }

        if(account.Role == "Admin")
        {
            _access.BlockAccount(account!);
            return true;
        }

        Console.WriteLine("You are not allowed ro block an Account");
        return false;
    }

}




