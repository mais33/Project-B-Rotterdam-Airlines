public class SeatModel
{
    public Int64 Id {get;set;}
    public string SeatNumber {get;set;}
    public int RowNumber {get;set;}
    private double _price;
    public double Price 
    {
        get => _price;
        set
        {
            _price = value < 0 ? 0 : value;
        }
    }
    public bool IsReserved {get;set;}
    public SeatTier Tier {get;set;}

    public SeatModel() {}
    public SeatModel(Int64 id, string seatNumber, int rowNumber, double price, bool status, SeatTier tier)
    {
        Id = id;
        SeatNumber = seatNumber;
        RowNumber = rowNumber;
        Price = price;
        IsReserved = status;
        Tier = tier;
    }

}

public enum SeatTier
{
    Standard = 1,
    Premium = 2,
    VIP = 3
}