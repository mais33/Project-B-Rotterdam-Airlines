public  class AccountManager
{
    private static AccountsLogic accountsLogic = new();

    public void AdminMenu()
    {
        Console.WriteLine("\n--- Admin Menu ---");
        Console.WriteLine("1: manage accounts\n");

        bool isRunning = true;
        AccountManager accountManager = new();
        
        while(isRunning)
        {
            string choice = Console.ReadLine()!;
            
            switch(choice)
            {
                case "1":
                    accountManager.AskAccToManage();
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again");
                    break;
            }

        }
    }
    public void AccountInfo() // shows a list of all accounts that an admin can change
    {
        List<AccountModel> allAccounts = accountsLogic.GetAllAccounts();
        foreach(var acc in allAccounts)
        {
            Console.WriteLine(acc.ToAdminString());
        }
    }

    public long GetAccId(int searchId)
    {
        List<AccountModel> allAccounts = accountsLogic.GetAllAccounts();
        foreach(var acc in allAccounts)
        {
            if(searchId == acc.Id)
            {
                return acc.Id;
            }
        }

        return 0;
    }

    public int GetIdFromUser()
    {
        Console.WriteLine("Please enter the number of the account you want to edit");
        int numberInput = Convert.ToInt32(Console.ReadLine());

        return numberInput;

    }

    public void AskAccToManage()
    {
        AccountInfo();
        
        int numberInput = GetIdFromUser();

        AccountModel accToEdit = accountsLogic.GetAccountById(numberInput);

        while (true)
        {
            Console.WriteLine($"\nYou are now editing the account of {accToEdit.FirstName} {accToEdit.LastName}\n");
            Console.WriteLine("1. Change Email");
            Console.WriteLine("2. Change Role");
            Console.WriteLine("3. Change Phonenumber");
            Console.WriteLine("4. Change Blocked status");
            Console.WriteLine("5. Return to Admin menu");
            

            int actionChoice = Convert.ToInt32(Console.ReadLine()!);

            while(true)
            {
                switch(actionChoice) //action 
                {
                    case 1:
                        AccEmailEdit(accToEdit);
                        AskAccToManage();
                        break;
                    case 2:
                        AccRoleEdit(accToEdit);
                        AskAccToManage();
                        break;
                    case 3:
                        EditPhoneNumber(accToEdit);
                        AskAccToManage();
                        break;
                    case 4:
                        BlockorUnblockAcc(accToEdit);
                        AskAccToManage();
                        break;
                    case 5:
                        AdminMenu();
                        AccountInfo();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;

                }
            }
        }

    }

    public void AccEmailEdit(AccountModel account)
    {
        Console.WriteLine("Enter new Email");
        string newEmail= Console.ReadLine()!;

        if (!accountsLogic.IsAdmin(AccountsLogic.CurrentAccount!))
        {
            Console.WriteLine("You are not authorised to edit an Email");
            return;
        }

        while (!accountsLogic.CheckEmail(newEmail))
        {
            Console.WriteLine("Please enter a valid email");
            newEmail = Console.ReadLine() ?? "";
        }
    
        account.Email = newEmail;
        
        accountsLogic.EditAccount(account);
        Console.WriteLine($"Updated Email to: {account.Email}");

    }

    public void AccRoleEdit(AccountModel account)
    {
        Console.WriteLine("1. change role to Customer");
        Console.WriteLine("2. change role to Admin");
        Console.WriteLine("3. Return\n");

        int newRole = Convert.ToInt32(Console.ReadLine()!);
        
        if(accountsLogic.IsAdmin(account) && newRole == 1)
        {    
            Console.WriteLine($"You are not allowed to change the role of an admin account\n");
        }
        else if(!accountsLogic.IsAdmin(account) && newRole == 1)
        {
            account.Role = UserRole.Customer;
            accountsLogic.EditAccount(account);
        }
        else if(newRole == 2)
        {
            account.Role = UserRole.Admin;
            accountsLogic.EditAccount(account);
            Console.WriteLine("You succesfully changed the role of this account to Admin");
        }
        else if(newRole == 3)
        {
            AskAccToManage();
        }
        else
        {
            Console.WriteLine("You entered an invalid number");
        }

    }

    public void EditPhoneNumber(AccountModel account)
    {
        Console.WriteLine("Enter a new phonenumber: ");
        string newPhoneNumber = Console.ReadLine() ?? "";

        bool EditSucces = accountsLogic.EditPhoneNumber(account, newPhoneNumber);
        
        if(EditSucces)
        {
            Console.WriteLine($"You changed the phonenumber of this account to {newPhoneNumber}");
        }
        else
        {
            Console.WriteLine("Please enter a valid phonenumber");
        }
    }

    public void BlockorUnblockAcc(AccountModel account)
    {
        Console.WriteLine("1. Block account");
        Console.WriteLine("2. Unblock account");
        Console.WriteLine("3. Return\n");

        int choice = Convert.ToInt32(Console.ReadLine()!);
        AccountManager accountManager = new();

        switch(choice)
        {
            case 1:
                bool BlockSuccess = accountsLogic.BlockAccount(account);

                if(BlockSuccess)
                {
                    Console.WriteLine("Succesfully blocked this account");
                }
                break;
            case 2:
                bool unblockSuccess = accountsLogic.UnBlockAccount(account);
                
                if(unblockSuccess)
                {
                    Console.WriteLine("Succesfully unblocked this account");
                }
                break;
            case 3:
                AskAccToManage();
                break;

            default:
                Console.WriteLine("You entered an invalid number");
                break;
        }


    }



    public static void Start()
    {

        AccountManager accountManager = new();

        accountManager.AdminMenu();
    }   

}