static class Menu
{
    private static AccountsAccess accountsAccess = new();
    private static AccountsLogic accountsLogic = new();
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        // accountsAccess.ClearAccounts(); //deletes all values from Account table
        // accountsLogic.CreateAdminAccount("admin"); // creates the admin. can use this line if admin account is deleted from database. (the password sould always be "admin")
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to register");

        string input = Console.ReadLine() ?? "";
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            UserRegistration.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}