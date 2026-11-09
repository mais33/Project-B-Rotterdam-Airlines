static class TicketPresentation
{
    private static TicketLogic ticketLogic = new TicketLogic();
    private static FlightLogic flightLogic = new FlightLogic();

    // Shown right after the customer confirms a reservation
    public static void ShowConfirmation(List<TicketModel> tickets, FlightModel? flight = null)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                BOOKING CONFIRMED                 ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        if (tickets.Count == 0)
        {
            Console.WriteLine("  > No tickets were created.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine(new TicketTemplate().Create(tickets));

        Console.WriteLine("  You can see this again under 'Booking confirmations'");
        Console.WriteLine("  in the Customer Menu.");
        Console.WriteLine();
        Console.WriteLine("  Press any key to return to the menu...");
        Console.ReadKey();
    }

    // Shown from the Customer Menu: all bookings of the logged in customer
    public static void ShowMyTickets(AccountModel? account = null)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("               BOOKING CONFIRMATIONS              ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        if (account == null)
        {
            account = AccountsLogic.CurrentAccount;
        }

        if (account == null)
        {
            Console.WriteLine("  > Please log in first to view your bookings.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        List<TicketModel> tickets = ticketLogic.GetTicketsForAccount(account.Id);

        if (tickets.Count == 0)
        {
            Console.WriteLine("  > You do not have any bookings yet.");
            Console.WriteLine("  > Choose 'Make a reservation' in the menu to book a flight.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"  Bookings of {account.FirstName} {account.LastName}");
        Console.WriteLine();
        string ticketText = new TicketTemplate().Create(tickets);
        Console.WriteLine(ticketText);

        Console.Write("  Preview confirmation email? (yes/no): ");
        string answer = (Console.ReadLine() ?? "").Trim().ToLower();
        while (answer != "yes" && answer != "no")
        {
            Console.Write("  Please enter yes or no: ");
            answer = (Console.ReadLine() ?? "").Trim().ToLower();
        }
        if (answer == "yes")
        {
            Console.WriteLine();
            Console.WriteLine("  CONFIRMATION EMAIL PREVIEW");
            Console.WriteLine($"  To: {account.Email}");
            Console.WriteLine("  Subject: Your flight ticket confirmation");
            Console.WriteLine();
            Console.WriteLine(ticketText);
        }
        Console.WriteLine("  Press any key to return...");
        Console.ReadKey();
    }
}
