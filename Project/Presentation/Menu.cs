static class Menu
{
    // This shows the menu
    static public void Start()
    {
        Console.WriteLine();
        Console.WriteLine("=== Rotterdam Airlines ===");
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to make a reservation");

        string input = Console.ReadLine() ?? "";

        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Console.WriteLine();
            Console.WriteLine(
                "You need to log in first before making a reservation."
            );

            Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }
}