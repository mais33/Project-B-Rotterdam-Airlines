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

        List<Int64> planeIds = seatAccess.GetPlaneIds();

        foreach (Int64 planeId in planeIds)
        {
            // Get existing seats for this plane.
            List<SeatModel> existingSeats =
                seatAccess.GetSeatsByPlaneId(planeId);

            for (int i = 0; i < 16; i++)
            {
                int rowNumber = i + 1;

                for (int k = 0; k < 6; k++)
                {
                    string seatNum = $"{rowNumber}{seatLetters[k]}";

                    // Skip seats that already exist for this plane.
                    if (existingSeats.Any(s =>
                        s.SeatNumber == seatNum))
                    {
                        continue;
                    }

                    double price;
                    SeatTier tier;

                    if (rowNumber <= 8)
                    {
                        price = 10.00;
                        tier = SeatTier.Standard;
                    }
                    else if (rowNumber <= 13)
                    {
                        price = 20.00;
                        tier = SeatTier.Premium;
                    }
                    else
                    {
                        price = 35.00;
                        tier = SeatTier.VIP;
                    }

                    SeatModel seat = new SeatModel(
                        0,
                        seatNum,
                        rowNumber,
                        price,
                        false,
                        tier
                    );

                    seatAccess.Write(seat, planeId);
                }
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


    public SeatModel[,] LoadSeatMapByPlaneId(Int64 planeId)
    {
        if (planeId <= 0)
        {
            throw new ArgumentException("Invalid plane ID.");
        }

        List<SeatModel> seats = seatAccess.GetSeatsByPlaneId(planeId);

        SeatModel[,] seatMap = new SeatModel[16, 6];

        foreach (SeatModel seat in seats)
        {
            int rowIndex = seat.RowNumber - 1;

            if (string.IsNullOrWhiteSpace(seat.SeatNumber))
            {
                continue;
            }

            char seatLetter = char.ToUpper(
                seat.SeatNumber[seat.SeatNumber.Length - 1]);

            int columnIndex = seatLetter - 'A';

            if (rowIndex >= 0 && rowIndex < 16 &&
                columnIndex >= 0 && columnIndex < 6)
            {
                seatMap[rowIndex, columnIndex] = seat;
            }
        }

        return seatMap;
    }


    public SeatModel[,] LoadSeatMapByFlightId(Int64 flightId)
    {
        if (flightId <= 0)
        {
            throw new ArgumentException("Invalid flight ID.");
        }

        List<SeatModel> seats =
            seatAccess.GetSeatsByFlightId(flightId);

        SeatModel[,] seatMap = new SeatModel[16, 6];

        foreach (SeatModel seat in seats)
        {
            int rowIndex = seat.RowNumber - 1;

            if (string.IsNullOrWhiteSpace(seat.SeatNumber))
            {
                continue;
            }

            char seatLetter = char.ToUpper(
                seat.SeatNumber[seat.SeatNumber.Length - 1]);

            int columnIndex = seatLetter - 'A';

            if (rowIndex >= 0 && rowIndex < 16 &&
                columnIndex >= 0 && columnIndex < 6)
            {
                seatMap[rowIndex, columnIndex] = seat;
            }
        }

        return seatMap;
    }


}