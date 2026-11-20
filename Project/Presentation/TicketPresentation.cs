public class TicketPresentation
{
    private static TicketLogic _ticketLogic = new();
    public static List<TicketDisplayModel> AllAccTickets = new();

    public static void Start(AccountModel account)
    {
        Console.Clear();
        AllAccTickets = _ticketLogic.GetTicketsByAccountId(account.Id);

        if(AllAccTickets.Count > 0)
        {   
            foreach(var ticket in AllAccTickets)
            {
                Console.WriteLine(ticket);
            }
        }
        else
        {
            Console.WriteLine("  > You don't have any tickets yet");
            Console.WriteLine("  Press any key to continue...");
            Console.ReadKey(true);
            return;
        }

        Console.WriteLine("  \n[1] return to customer menu");
        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                Console.WriteLine("  > Returning to Customer menu");
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey(true);
                return;

            // case "2":
            //     RemoveTicketForCustomer(account);
            //     break;

            default:
                Console.WriteLine("  > Invalid option. Please choose 1, 2, 3 or 4.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey(true);     
            break;
        }

        

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }

    public static void RemoveTicketForCustomer(AccountModel account) //ticket will still exist in database, but user does not see it anymore
    {
        Console.Clear();

        if(AllAccTickets.Count == 0)
        {
            Console.WriteLine("You have no tickets to remove");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey(true);
            return;
        }
        
        bool canceling = true;

        while(canceling)
        {
            Console.Clear();
            Console.WriteLine("What ticket do you want to cancel (press ESC to exit)");
            string input = Console.ReadLine() ?? "";

            ConsoleKeyInfo action = Console.ReadKey(true);
            
            if(action.Key == ConsoleKey.Escape)
            {
                canceling = false;
            }

            if (int.TryParse(input, out int ticketId))
            {
                for (int i = 0; i < AllAccTickets.Count; i++)
                {
                    if (AllAccTickets[i].Id == ticketId)
                    {

                        AllAccTickets.RemoveAt(i);
                        Console.ReadKey(true);
                        break;
                        
                    }
                }
                
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
         
    }
}