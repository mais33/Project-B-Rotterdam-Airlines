
using Microsoft.AspNetCore.Identity;

//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{

    public static AccountModel? CurrentAccount { get; private set; }
    private AccountsAccess _access = new();
    private readonly PasswordHasher<AccountModel> _hasher = new();

    public AccountsLogic()
    {

    }

    public AccountModel CheckLogin(string email, string password)
    {

        AccountModel acc = _access.GetByEmail(email);
        if (acc != null)
        {
            var verification = _hasher.VerifyHashedPassword(acc, acc.Password, password);
            if(verification == PasswordVerificationResult.Success)
            {
                CurrentAccount = acc;
                return acc;     
            }
        }
        return null!;
    }

    public bool IsAdmin(AccountModel account)
    {
        return account != null && account.Role == UserRole.Admin;
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

    public bool EditPhoneNumber(AccountModel account, string newPhoneNumber)
    {
        if(!PhoneNumberCheck(newPhoneNumber))
        {
            return false;
        }

        account.PhoneNumber = newPhoneNumber;

        _access.UpdatePhoneNumber(account); 
        return true;
    }

    public void CreateAccount(AccountModel account, string password)
    {
        if(account is null)
        {
            return;
        }

        account.Password = _hasher.HashPassword(account, password);

        _access.Write(account!);   
    }

    public bool EditAccount(AccountModel account)
    {
        if(account is null)
        {
            return false;
        }

        if (CurrentAccount is null)
        {
            Console.WriteLine("No user is currently logged in.");
            return false;
        }
        
        if(CurrentAccount.Role != UserRole.Admin)
        {
            Console.WriteLine("You are not allowed to edit an Account");
            return false;
        }


        AccountModel existingAccount = _access.GetAccountById(account.Id); 

        if(existingAccount is null)
        {
            Console.WriteLine("Account not found.");
            return false;
        }
    
        if (existingAccount.Role == UserRole.Admin && existingAccount.Id != CurrentAccount.Id) 
        {
            Console.WriteLine("You cannot change the role of an Admin account");
            return false;
        }

        if (existingAccount.Id == CurrentAccount.Id && account.Role == UserRole.Customer)
        {
            Console.WriteLine("As an admin, you cannot change your own role to Customer.");
            return false;
        }

        _access.Update(account);
        return true;
    }

    public bool BlockAccount(AccountModel account)
    {

        if(account is null)
        {
            return false;
        }

        if(CurrentAccount.Role != UserRole.Admin) //if acc is not an admin, that acc cannot block an acc
        {
            Console.WriteLine("You are not allowed to block an Account");
            return false;
        }

        AccountModel existingAccount = _access.GetAccountById(account.Id);

        if(existingAccount is null)
        {
            Console.WriteLine("Account not found");
            return false;
        }

        if(existingAccount.IsBlocked) //Admin cannot block an account that is already blocked
        {
            Console.WriteLine("This account is already blocked");
            return false;
        }

        if(existingAccount.Role == UserRole.Admin) //Admin acc cannot block another admin acc
        {
            Console.WriteLine("You cannot change the status of another Admin account");
            return false;
        }

        if(existingAccount.Id == CurrentAccount.Id) //Admin cannot block his own acc
        {
            Console.WriteLine("You cannot chnage the status of your own account");
            return false;
        }

        _access.BlockAccount(existingAccount);
        return true;
    }

    public bool UnBlockAccount(AccountModel account)
    {
        if(account is null)
        {
            return false;
        }

        if(CurrentAccount.Role != UserRole.Admin) //if acc is not an admin, that acc cannot unblock an acc
        {
            Console.WriteLine("You are not allowed to unblock an Account");
            return false;
        }


        AccountModel existingAccount = _access.GetAccountById(account.Id);

        if(existingAccount is null)
        {
            Console.WriteLine("Account not found");
            return false;
        }

        if(!existingAccount.IsBlocked) //Admin cannot unblock an account that is not blocked
        {
            Console.WriteLine("You cannot unblock an account that is not blocked");
            return false;
        }

        if(existingAccount.Role == UserRole.Admin) //Admin acc cannot block another admin acc
        {
            Console.WriteLine("You cannot change the status of another Admin account");
            return false;
        }

        if(existingAccount.Id == CurrentAccount.Id) //Admin cannot change the status of  his own acc
        {
            Console.WriteLine("You cannot chnage the status of your own account");
            return false;
        }

        _access.UnblockAccount(existingAccount);
        return true;
    }

    public List<AccountModel> GetAllAccounts()
    {
        return _access.GetAllAccounts();
    }

    public AccountModel GetAccountById(Int64 id)
    {
        return _access.GetAccountById(id);
    }

    public void CreateAdminAccount(string password)
    {
        AccountModel adminAccount = new(0, "admin", "admin", "admin", "1234", UserRole.Admin, false, "");
        adminAccount.Password = _hasher.HashPassword(adminAccount, password);
        _access.Write(adminAccount); 
    }


}




