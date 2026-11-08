using System.Security.Cryptography.X509Certificates;
using SQLitePCL;

public class SeatManager
{
    public static bool UserExited = false;

    public void DrawSeatMap(SeatModel[,] seatMap)
    {
        Console.WriteLine("      A   B   C     D   E   F  ");

        for(int i = 0; i < seatMap.GetLength(0); i++)
        {
            Console.Write($"{i + 1,2} | ");

            for (int k = 0; k < seatMap.GetLength(1); k++)
            {
                SeatModel seat = seatMap[i, k];

                if (seat != null && seat.IsReserved)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[X]"); // X means reserved
                }
                else if (seat != null && seat.Tier == SeatTier.Standard)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[S]"); // S means  standard seat
                }
                else if (seat != null && seat.Tier == SeatTier.Premium)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("[P]"); // P means  Premium seat
                }
                else if (seat != null && seat.Tier == SeatTier.VIP)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("[V]"); // V means  Vip seat
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[ ]"); // [] for if there is no seat in 2d array cell
                }
                Console.ResetColor();
                
                if (k == 2)
                {
                    Console.Write("   "); // gap between seats
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine(); // Move to next line for next row
        }
    }
    public static void Start()
    {
        UserExited = false;
        bool selecting = true;
        SeatManager seatManager = new();
        SeatLogic seatLogic = new();
        
        SeatModel[,] currentMap = seatLogic.LoadSeatMapFromDatabase();

        int AmountOfSeatsReservedCount = 0;

        while(selecting)
        {
            Console.Clear();
            seatManager.DrawSeatMap(currentMap);

            Console.WriteLine("\nEnter a seat to select (for example: 1A)");
            Console.WriteLine("type 'confirm' to confirm seat reservation or 'exit' to return to customer menu");
            string choice = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if(choice == "EXIT" )
            {
                UserExited = true;
                return;
            }
            if(choice == "CONFIRM")
            {
                if(AmountOfSeatsReservedCount == 0)
                {
                    Console.WriteLine("You must atleast reserve 1 seat to confirm reservation");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                    continue;
                }
                selecting = false;
                break;
            }
            
            

            SeatModel selectedSeat = seatLogic.FindSeat(currentMap, choice);

            if (selectedSeat != null)
            {
                if (!selectedSeat.IsReserved)
                {
                    Console.WriteLine($"\nYou selected a {selectedSeat.Tier} seat {selectedSeat.SeatNumber}. Price: ${selectedSeat.Price}");
                    seatLogic.ReserveSeat(selectedSeat);
                    currentMap = seatLogic.LoadSeatMapFromDatabase();

                    Console.WriteLine("\nPress Escape to choose a different seat or press Enter to confirm this seat");
                    ConsoleKeyInfo action = Console.ReadKey(true);

                    if(action.Key == ConsoleKey.Escape)
                    {
                        seatLogic.UnReserveSeat(selectedSeat);
                        currentMap = seatLogic.LoadSeatMapFromDatabase();
                        continue;
                    }

                    if(action.Key == ConsoleKey.Enter)
                    {
                        AmountOfSeatsReservedCount++;
                        Console.WriteLine("\nYou confirmed your seat reservation.");
                        if(AmountOfSeatsReservedCount >= 6)
                        {
                            Console.WriteLine("You have reserved the maximum amount of 6 seats. Returning..");
                            selecting = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"\nSeat {selectedSeat.SeatNumber} is already reserved.");
                    Console.WriteLine("\nPress any key to try another seat...");
                    Console.ReadKey(true);
                }
            }
            else
            {
                Console.WriteLine("\nInvalid seat format or seat does not exist. Try something like 1A or 12F."); 
                Console.WriteLine("\nPress any key to try again...");
                Console.ReadKey(true);
            }
        }
    }
    
}