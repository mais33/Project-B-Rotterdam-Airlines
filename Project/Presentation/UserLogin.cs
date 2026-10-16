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
            if(acc.Role == UserRole.Admin)
            {
                Console.WriteLine($"Welcome back admin");
                AccountManager.Start();
            }
            else if (acc.Role == UserRole.Customer)
            {
                Console.WriteLine($"Welcome back " + acc.FirstName + acc.LastName);
                Console.WriteLine("Your email number is " + acc.Email);


                //Write some code to go back to the menu
                //Menu.Start();
            }
            else
            {
                Console.WriteLine("No account found with that email and password");
            }   
        }
    }
}