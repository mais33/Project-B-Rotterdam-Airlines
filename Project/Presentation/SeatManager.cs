
public class SeatManager
{
    public static bool UserExited = false;

    public static List<Int64> SelectedSeatIds { get; private set; }
        = new List<Int64>();

    public void DrawSeatMap(SeatModel[,] seatMap)
    {
        Console.WriteLine("      A   B   C     D   E   F  ");

        for (int i = 0; i < seatMap.GetLength(0); i++)
        {
            Console.Write($"{i + 1,2} | ");

            for (int k = 0; k < seatMap.GetLength(1); k++)
            {
                SeatModel? seat = seatMap[i, k];

                if (seat != null &&
                    SelectedSeatIds.Contains(seat.Id))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[✓]");
                }
                else if (seat != null && seat.IsReserved)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[X]");
                }
                else if (seat != null &&
                         seat.Tier == SeatTier.Standard)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[S]");
                }
                else if (seat != null &&
                         seat.Tier == SeatTier.Premium)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("[P]");
                }
                else if (seat != null &&
                         seat.Tier == SeatTier.VIP)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("[V]");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[ ]");
                }

                Console.ResetColor();
                Console.Write(k == 2 ? "   " : " ");
            }

            Console.WriteLine();
        }
    }

    public static void Start(Int64 flightId)
    {
        UserExited = false;
        SelectedSeatIds.Clear();

        SeatManager seatManager = new SeatManager();
        SeatLogic seatLogic = new SeatLogic();

        int requiredSeats = ReservationPresentation.passengerCount;

        if (flightId <= 0 || requiredSeats <= 0)
        {
            Console.WriteLine("Invalid flight or passenger count.");
            UserExited = true;
            return;
        }

        while (SelectedSeatIds.Count < requiredSeats)
        {
            SeatModel[,] currentMap =
                seatLogic.LoadSeatMapByFlightId(flightId);

            Console.Clear();
            seatManager.DrawSeatMap(currentMap);

            Console.WriteLine();
            Console.WriteLine(
                $"Select seat {SelectedSeatIds.Count + 1} of {requiredSeats}");

            Console.WriteLine(
                $"Selected: {SelectedSeatIds.Count}/{requiredSeats}");

            Console.WriteLine();
            Console.WriteLine(
                "Standard [S]  Premium [P]  VIP [V]");

            Console.WriteLine(
                "Selected [✓]  Reserved [X]");

            Console.WriteLine();
            Console.WriteLine(
                "Enter a seat number (example: 1A)");

            Console.WriteLine(
                "Type 'exit' to cancel seat selection.");

            string choice =
                Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (choice == "EXIT")
            {
                SelectedSeatIds.Clear();
                UserExited = true;
                return;
            }

            SeatModel? selectedSeat =
                seatLogic.FindSeat(currentMap, choice);

            if (selectedSeat == null)
            {
                Console.WriteLine("Seat not found.");
                Console.ReadKey(true);
                continue;
            }

            if (selectedSeat.IsReserved)
            {
                Console.WriteLine(
                    "This seat is already reserved.");

                Console.ReadKey(true);
                continue;
            }

            if (SelectedSeatIds.Contains(selectedSeat.Id))
            {
                Console.WriteLine(
                    "You already selected this seat.");

                Console.ReadKey(true);
                continue;
            }

            Console.WriteLine();
            Console.WriteLine(
                $"Seat: {selectedSeat.SeatNumber}");

            Console.WriteLine(
                $"Tier: {selectedSeat.Tier}");

            Console.WriteLine();
            Console.WriteLine(
                $"Seat price: EUR {selectedSeat.Price:F2}");

            Console.WriteLine();
            Console.WriteLine(
                "Press Enter to select this seat.");

            Console.WriteLine(
                "Press Escape to choose another seat.");

            ConsoleKeyInfo action = Console.ReadKey(true);

            if (action.Key == ConsoleKey.Enter)
            {
                SelectedSeatIds.Add(selectedSeat.Id);
            }
        }

        Console.Clear();
        Console.WriteLine("=== SELECTED SEATS ===");
        Console.WriteLine();

        SeatModel[,] finalMap =
            seatLogic.LoadSeatMapByFlightId(flightId);

        double seatTotal = 0;

        foreach (Int64 seatId in SelectedSeatIds)
        {
            foreach (SeatModel? seat in finalMap)
            {
                if (seat != null && seat.Id == seatId)
                {
                    Console.WriteLine(
                        $"{seat.SeatNumber} - EUR {seat.Price:F2}");

                    seatTotal += seat.Price;
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine(
            $"Total seat price: EUR {seatTotal:F2}");

        Console.WriteLine();
        Console.WriteLine(
            "Seats selected. Continue to reservation confirmation.");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }
}
