public class SeatLogic
{
    private SeatAccess seatAccess = new();   
    public void ReserveSeat(SeatModel seat)
    {
        seatAccess.SetSeatAsReserved(seat);
    }

    public void UnReserveSeat(SeatModel seat)
    {
        seatAccess.SetSeatAsUnReserved(seat);
    }

    public SeatModel[,] LoadSeatMapFromDatabase()
    {
        var dbSeats = seatAccess.GetAllSeats().ToList();

        if (dbSeats.Count == 0)
        {
            SeedDatabaseSeats();
            dbSeats = seatAccess.GetAllSeats();
        }   
        
        SeatModel[,] freshSeatMap = new SeatModel[16, 6];

        foreach (var seat in dbSeats)
        {
            int rowIndex = seat.RowNumber - 1;

            char seatLetter = char.ToUpper(seat.SeatNumber.Last());
            
            int colIndex = -1;
            if (seatLetter == 'A') colIndex = 0;
            else if (seatLetter == 'B') colIndex = 1;
            else if (seatLetter == 'C') colIndex = 2;
            else if (seatLetter == 'D') colIndex = 3;
            else if (seatLetter == 'E') colIndex = 4;
            else if (seatLetter == 'F') colIndex = 5;

            if (rowIndex >= 0 && rowIndex < 16 && colIndex >= 0 && colIndex < 6)
            {
                freshSeatMap[rowIndex, colIndex] = seat;
            }
        }

        return freshSeatMap;
    }

    public void SeedDatabaseSeats()
    {
        char[] seatLetters = { 'A', 'B', 'C', 'D', 'E', 'F' };
        
        for(int i = 0; i < 16; i++)
        {
            int rowNumber = i + 1;
            for(int k = 0; k < 6; k++)
            {
                string seatNum = $"{rowNumber}{seatLetters[k]}";
                SeatModel savedSeat;

                if(rowNumber <= 8)
                {
                    savedSeat = new SeatModel(0, seatNum, i + 1, 10.00, false, SeatTier.Standard); //Standard seats
                }
                else if(rowNumber <= 13)
                {
                    savedSeat = new SeatModel(0, seatNum, i + 1, 20.00, false, SeatTier.Premium); // Premium seats
                }
                else
                {
                    savedSeat = new SeatModel(0, seatNum, i + 1, 35.00, false, SeatTier.VIP); // VIP seats
                }

                seatAccess.Write(savedSeat);
            }
        }
    }

    public SeatModel FindSeat(SeatModel[,] seatMap, string seatNumber)
    {

        foreach (var seat in seatMap)
        {
            if (seat != null && seat.SeatNumber?.ToUpper() == seatNumber)
            {
                return seat;
            }
        }
        return null;
    }

}