public class SeatModel
{
    public Int64 Id { get; set; }
    public Int64 PlaneId { get; set; }
    public string SeatNumber { get; set; } = "";
    public int RowNumber { get; set; }
    public double Price { get; set; }
    public int Tier { get; set; }

    public SeatModel()
    {
    }

    public SeatModel(
        Int64 id,
        Int64 planeId,
        string seatNumber,
        int rowNumber,
        double price,
        int tier)
    {
        Id = id;
        PlaneId = planeId;
        SeatNumber = seatNumber;
        RowNumber = rowNumber;
        Price = price;
        Tier = tier;
    }
}
