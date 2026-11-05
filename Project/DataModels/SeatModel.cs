public class SeatModel
{
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

    public SeatModel(string seatNumber, int rowNumber, double price, bool status)
    {
        SeatNumber = seatNumber;
        RowNumber = rowNumber;
        Price = price;
        IsReserved = status;
    }

}

public enum SeatTier
{
    Standard,
    Premium,
    VIP
}