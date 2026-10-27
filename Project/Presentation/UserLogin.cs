static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();
    

    public static void Start()
    {

        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("Please enter your password");
        string password = accountsLogic.PassWordHider();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        if(acc != null)
        {
            // Go to the right menu based on the role
            if(acc.Role == UserRole.Admin)
            {
                Console.WriteLine($"Welcome back admin");
                Menu.AdminMenu();
            }
            else if (acc.Role == UserRole.Customer)
            {
                Console.WriteLine($"Welcome back " + acc.FirstName + " " + acc.LastName);
                Console.WriteLine("Your email is " + acc.Email);
                Menu.CustomerMenu();
            }
        }
        else
        {
            Console.WriteLine("No account found with that email and password");
        }
    }
}