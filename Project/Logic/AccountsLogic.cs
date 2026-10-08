
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


        AccountModel acc = _access.GetByEmail(email);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
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

    public bool NameCheck(string firstName, string LastName)
    {
        if(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(LastName) || !firstName.All(char.IsLetter) || !LastName.All(char.IsLetter))
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

}




