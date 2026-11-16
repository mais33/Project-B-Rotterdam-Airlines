
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
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        string trimmed = phoneNumber.Trim();

        if (trimmed.StartsWith("-") || trimmed.EndsWith("-"))
        {
            return false;
        }

        string cleaned = trimmed.Replace(" ", "").Replace("-", "");

        if (cleaned.StartsWith("+"))
        {
            cleaned = cleaned.Substring(1);
        }

        if (!cleaned.All(char.IsDigit))
        {
            return false;
        }

        if (cleaned.Length < 8 || cleaned.Length > 15)
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

        account.PhoneNumber = newPhoneNumber.Trim();

        _access.UpdatePhoneNumber(account); 
        return true;
    }

    public bool IsEmailTaken(string email)
    {
        return _access.GetByEmail(email) != null;
    }
    
    public OperationResult CreateAccount(AccountModel account, string password, string email)
    {
        if(account is null)
        {
            return OperationResult.FailureResult("Account cannot be null");
        }

        account.Password = _hasher.HashPassword(account, password);

        _access.Write(account);
        return OperationResult.SuccessResult();
    }

    public OperationResult EditAccount(AccountModel account)
    {
        if(account is null)
        {
            return OperationResult.FailureResult("Account data cannot be null");
        }

        if (CurrentAccount is null)
        {
            return OperationResult.FailureResult("No user is currently logged in.");
        }
        
        if(CurrentAccount.Role != UserRole.Admin)
        {
            return OperationResult.FailureResult("You are not allowed to edit an account.");
        }


        AccountModel existingAccount = _access.GetAccountById(account.Id); 

        if(existingAccount is null)
        {
            return OperationResult.FailureResult("Account not found.");
        }
    
        if (existingAccount.Role == UserRole.Admin && existingAccount.Id != CurrentAccount.Id) 
        {
            return OperationResult.FailureResult("You cannot edit another Admin account.");
        }

        if (existingAccount.Id == CurrentAccount.Id && account.Role == UserRole.Customer)
        {
            return OperationResult.FailureResult("As an admin, you cannot change your own role to Customer.");
        }

        _access.Update(account);
        return OperationResult.SuccessResult();
    }

    public OperationResult BlockAccount(AccountModel account)
    {

        if(account is null)
        {
            return OperationResult.FailureResult("Account data cannot be null");
        }

        if(CurrentAccount.Role != UserRole.Admin) //if acc is not an admin, that acc cannot block an acc
        {
            return OperationResult.FailureResult("You are not allowed to block an Account");
        }

        AccountModel existingAccount = _access.GetAccountById(account.Id);

        if(existingAccount is null)
        {
            return OperationResult.FailureResult("Account not found");
        }

        if(existingAccount.IsBlocked) //Admin cannot block an account that is already blocked
        {
            return OperationResult.FailureResult("This account is already blocked");
        }

        if(existingAccount.Role == UserRole.Admin) //Admin acc cannot block another admin acc
        {
            return OperationResult.FailureResult("You cannot change the status of another Admin account");
        }

        if(existingAccount.Id == CurrentAccount.Id) //Admin cannot block his own acc
        {
            return OperationResult.FailureResult("You cannot change the status of your own account");
        }

        _access.BlockAccount(existingAccount);
        return OperationResult.SuccessResult();
    }

    public OperationResult UnBlockAccount(AccountModel account)
    {
        if(account is null)
        {
            return OperationResult.FailureResult("Account data cannot be null");
        }

        if(CurrentAccount.Role != UserRole.Admin) //if acc is not an admin, that acc cannot unblock an acc
        {
            return OperationResult.FailureResult("You are not allowed to unblock an Account");
        }


        AccountModel existingAccount = _access.GetAccountById(account.Id);

        if(existingAccount is null)
        {
            return OperationResult.FailureResult("Account not found");
        }

        if(!existingAccount.IsBlocked) //Admin cannot unblock an account that is not blocked
        {
            return OperationResult.FailureResult("You cannot unblock an account that is not blocked");
        }

        if(existingAccount.Role == UserRole.Admin) //Admin acc cannot block another admin acc
        {
            return OperationResult.FailureResult("You cannot change the status of another Admin account");
        }

        if(existingAccount.Id == CurrentAccount.Id) //Admin cannot change the status of  his own acc
        {
            return OperationResult.FailureResult("You cannot change the status of your own account");
        }

        _access.UnblockAccount(existingAccount);
        return OperationResult.SuccessResult();
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




