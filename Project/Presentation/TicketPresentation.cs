static class TicketPresentation
{
    static private TicketLogic ticketLogic = new TicketLogic();


    public static void ShowConfirmation(List<TicketModel> tickets)
    {
        if (tickets.Count == 0)
        {
            Console.WriteLine("No tickets were made.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== Booking Confirmed ===");
        Console.WriteLine("Booking reference: " + tickets[0].BookingReference);
        Console.WriteLine("Flight: " + tickets[0].FlightId);
        Console.WriteLine();
        Console.WriteLine("Tickets:");

        for (int i = 0; i < tickets.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tickets[i].FirstName} {tickets[i].LastName} - Seat {tickets[i].Seat}");
        }

        Console.WriteLine();
        Console.WriteLine("Thank you for flying with Rotterdam Airlines!");
    }

    public static void ShowMyTickets(AccountModel account)
    {
        List<TicketModel> tickets = ticketLogic.GetTicketsForAccount(account.Id);

        Console.WriteLine();
        Console.WriteLine("=== My Tickets ===");

        if (tickets.Count == 0)
        {
            Console.WriteLine("You don't have any tickets yet.");
            return;
        }

        for (int i = 0; i < tickets.Count; i++)
        {
            Console.WriteLine();
            Console.WriteLine($"Ticket {i + 1}");
            Console.WriteLine("Booking reference: " + tickets[i].BookingReference);
            Console.WriteLine("Passenger: " + tickets[i].FirstName + " " + tickets[i].LastName);
            Console.WriteLine("Flight: " + tickets[i].FlightId);
            Console.WriteLine("Seat: " + tickets[i].Seat);
        }
    }
}
