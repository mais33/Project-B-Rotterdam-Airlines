using Microsoft.AspNetCore.Identity;

public class UserLoginLogic
{
    private readonly UserLoginAccess _userLoginAccess = new UserLoginAccess();

    private readonly PasswordHasher<AccountModel> _passwordHasher =
        new PasswordHasher<AccountModel>();

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            !email.Contains("@") ||
            !email.Contains("."))
        {
            return false;
        }

        return true;
    }

    public AccountModel? Login(string email, string password)
    {
        AccountModel? account = _userLoginAccess.GetAccountByEmail(email);

        if (account == null)
        {
            return null;
        }

        PasswordVerificationResult result =
            _passwordHasher.VerifyHashedPassword(
                account,
                account.Password,
                password
            );

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return account;
    }
}