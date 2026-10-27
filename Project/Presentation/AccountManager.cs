public class AccountManager
{
    private static AccountsLogic accountsLogic = new();

    public static void Start()
    {
        AccountManager accountManager = new();
        accountManager.ManageAccounts();
    }

    public void ManageAccounts()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("======================================================================");
            Console.WriteLine("                           MANAGE ACCOUNTS                            ");
            Console.WriteLine("======================================================================");
            Console.WriteLine();

            List<AccountModel> allAccounts = accountsLogic.GetAllAccounts();

            if (allAccounts.Count == 0)
            {
                Console.WriteLine("  > No accounts found.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("  Select an account to edit:\n");

            for (int i = 0; i < allAccounts.Count; i++)
            {
                AccountModel acc = allAccounts[i];
                string status = acc.IsBlocked ? "Blocked" : "Active";

                Console.WriteLine($"  [{i + 1}] {acc.FirstName} {acc.LastName}");
                Console.WriteLine($"      Email:  {acc.Email}");
                Console.WriteLine($"      Role:   {acc.Role}   |   Status: {status}");
                Console.WriteLine();
            }

            int backOption = allAccounts.Count + 1;
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine($"  [{backOption}] Back to Administrator Menu");
            Console.WriteLine("----------------------------------------------------------------------");
            Console.Write($"  Choose an option (1-{backOption}): ");

            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int choice))
            {
                if (choice == backOption)
                {
                    return;
                }
                else if (choice >= 1 && choice <= allAccounts.Count)
                {
                    AccountModel selectedAccount = allAccounts[choice - 1];
                    EditAccountMenu(selectedAccount);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose a number from the list.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("  > Please enter a valid number.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    public void EditAccountMenu(AccountModel account)
    {
        while (true)
        {
            // Refresh account from database so changes are visible right away
            account = accountsLogic.GetAccountById(account.Id);

            if (account == null)
            {
                Console.WriteLine();
                Console.WriteLine("  > Account not found.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                   EDIT ACCOUNT                   ");
            Console.WriteLine("==================================================");
            Console.WriteLine();

            string status = account.IsBlocked ? "Blocked" : "Active";
            Console.WriteLine("  Account Information:");
            Console.WriteLine("  ------------------------------------------------");
            Console.WriteLine($"  Name:          {account.FirstName} {account.LastName}");
            Console.WriteLine($"  Email:         {account.Email}");
            Console.WriteLine($"  Phone number:  {account.PhoneNumber}");
            Console.WriteLine($"  Role:          {account.Role}");
            Console.WriteLine($"  Status:        {status}");
            Console.WriteLine("  ------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("  Options:");
            Console.WriteLine("  [1] Edit email");
            Console.WriteLine("  [2] Edit role");
            Console.WriteLine("  [3] Edit phone number");
            Console.WriteLine("  [4] Edit account status");
            Console.WriteLine("  [5] Back to accounts list");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-5): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    EditEmail(account);
                    break;

                case "2":
                    EditRole(account);
                    break;

                case "3":
                    EditPhoneNumber(account);
                    break;

                case "4":
                    EditAccountStatus(account);
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose 1, 2, 3, 4 or 5.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public void EditEmail(AccountModel account)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                    EDIT EMAIL                    ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine($"  Account:        {account.FirstName} {account.LastName}");
        Console.WriteLine($"  Current email:  {account.Email}");
        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        Console.Write("  Enter new email: ");
        string newEmail = Console.ReadLine() ?? "";

        while (!accountsLogic.CheckEmail(newEmail))
        {
            Console.WriteLine();
            Console.WriteLine("  > Please enter a valid email address.");
            Console.WriteLine();
            Console.Write("  Enter new email: ");
            newEmail = Console.ReadLine() ?? "";
        }

        account.Email = newEmail;
        bool success = accountsLogic.EditAccount(account);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        if (success)
        {
            Console.WriteLine($"  > Email successfully updated to: {account.Email}");
        }

        Console.WriteLine();
        Console.WriteLine("  Press any key to continue...");
        Console.ReadKey();
    }

    public void EditRole(AccountModel account)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                    EDIT ROLE                     ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine($"  Account:       {account.FirstName} {account.LastName}");
        Console.WriteLine($"  Current role:  {account.Role}");
        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("  [1] Set role to Customer");
        Console.WriteLine("  [2] Set role to Admin");
        Console.WriteLine("  [3] Back");
        Console.WriteLine("--------------------------------------------------");
        Console.Write("  Choose an option (1-3): ");

        string choice = Console.ReadLine() ?? "";
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                if (account.Role == UserRole.Customer)
                {
                    Console.WriteLine("  > This account already has the Customer role.");
                }
                else
                {
                    account.Role = UserRole.Customer;
                    bool success = accountsLogic.EditAccount(account);
                    if (success)
                    {
                        Console.WriteLine("  > Role successfully updated to Customer.");
                    }
                }
                break;

            case "2":
                if (account.Role == UserRole.Admin)
                {
                    Console.WriteLine("  > This account already has the Admin role.");
                }
                else
                {
                    account.Role = UserRole.Admin;
                    bool success = accountsLogic.EditAccount(account);
                    if (success)
                    {
                        Console.WriteLine("  > Role successfully updated to Admin.");
                    }
                }
                break;

            case "3":
                return;

            default:
                Console.WriteLine("  > Invalid choice. Please choose 1, 2 or 3.");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("  Press any key to continue...");
        Console.ReadKey();
    }

    public void EditPhoneNumber(AccountModel account)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                EDIT PHONE NUMBER                 ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        Console.WriteLine($"  Account:               {account.FirstName} {account.LastName}");
        Console.WriteLine($"  Current phone number:  {account.PhoneNumber}");
        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        Console.Write("  Enter new phone number: ");
        string newPhoneNumber = Console.ReadLine() ?? "";

        bool success = accountsLogic.EditPhoneNumber(account, newPhoneNumber);

        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        if (success)
        {
            Console.WriteLine($"  > Phone number successfully updated to: {newPhoneNumber}");
        }
        else
        {
            Console.WriteLine("  > Invalid phone number. Please enter digits only (maximum 11 digits).");
        }

        Console.WriteLine();
        Console.WriteLine("  Press any key to continue...");
        Console.ReadKey();
    }

    public void EditAccountStatus(AccountModel account)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("               EDIT ACCOUNT STATUS                ");
        Console.WriteLine("==================================================");
        Console.WriteLine();
        string currentStatus = account.IsBlocked ? "Blocked" : "Active";
        Console.WriteLine($"  Account:         {account.FirstName} {account.LastName}");
        Console.WriteLine($"  Current status:  {currentStatus}");
        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("  [1] Block account");
        Console.WriteLine("  [2] Unblock account");
        Console.WriteLine("  [3] Back");
        Console.WriteLine("--------------------------------------------------");
        Console.Write("  Choose an option (1-3): ");

        string choice = Console.ReadLine() ?? "";
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                bool blockSuccess = accountsLogic.BlockAccount(account);
                if (blockSuccess)
                {
                    Console.WriteLine("  > Account has been successfully blocked.");
                }
                break;

            case "2":
                bool unblockSuccess = accountsLogic.UnBlockAccount(account);
                if (unblockSuccess)
                {
                    Console.WriteLine("  > Account has been successfully unblocked.");
                }
                break;

            case "3":
                return;

            default:
                Console.WriteLine("  > Invalid choice. Please choose 1, 2 or 3.");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("  Press any key to continue...");
        Console.ReadKey();
    }
}