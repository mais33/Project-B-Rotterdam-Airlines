namespace UnitTests;

[TestClass]
public sealed class SeatTests
{
    private SeatModel[,] MakeTestSeatMap()
    {
        SeatModel[,] seatMap = new SeatModel[16, 6];

        seatMap[0, 0] = new SeatModel(1, "1A", 1, 10.00, false, SeatTier.Standard);
        seatMap[0, 1] = new SeatModel(2, "1B", 1, 10.00, true, SeatTier.Standard);
        seatMap[8, 2] = new SeatModel(3, "9C", 9, 20.00, false, SeatTier.Premium);
        seatMap[13, 5] = new SeatModel(4, "14F", 14, 35.00, false, SeatTier.VIP);

        return seatMap;
    }

    [DataTestMethod]
    [DataRow("1A")]
    [DataRow("1B")]
    [DataRow("9C")]
    [DataRow("14F")]
    public void FindSeatReturnsExistingSeat(string seatNumber)
    {
        SeatLogic logic = new SeatLogic();
        SeatModel[,] seatMap = MakeTestSeatMap();

        SeatModel result = logic.FindSeat(seatMap, seatNumber);

        Assert.IsNotNull(result);
        Assert.AreEqual(seatNumber, result.SeatNumber);
    }

    [DataTestMethod]
    [DataRow("99Z")]
    [DataRow("2A")]
    [DataRow("")]
    [DataRow("banana")]
    [DataRow("A1")]
    public void FindSeatReturnsNullForUnknownSeat(string seatNumber)
    {
        SeatLogic logic = new SeatLogic();
        SeatModel[,] seatMap = MakeTestSeatMap();

        SeatModel result = logic.FindSeat(seatMap, seatNumber);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindSeatInEmptySeatMapReturnsNull()
    {
        SeatLogic logic = new SeatLogic();
        SeatModel[,] seatMap = new SeatModel[16, 6];

        SeatModel result = logic.FindSeat(seatMap, "1A");

        Assert.IsNull(result);
    }

    [DataTestMethod]
    [DataRow(10.00, 10.00)]
    [DataRow(35.00, 35.00)]
    [DataRow(0.0, 0.0)]
    [DataRow(-5.0, 0.0)]
    public void CheckSeatPrice(double price, double expected)
    {
        SeatModel seat = new SeatModel(1, "1A", 1, price, false, SeatTier.Standard);

        double result = seat.Price;

        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(SeatTier.Standard, false)]
    [DataRow(SeatTier.Premium, true)]
    [DataRow(SeatTier.VIP, false)]
    public void SeatKeepsTierAndReservedStatus(SeatTier tier, bool reserved)
    {
        SeatModel seat = new SeatModel(1, "1A", 1, 10.00, reserved, tier);

        Assert.AreEqual(tier, seat.Tier);
        Assert.AreEqual(reserved, seat.IsReserved);
    }

    [DataTestMethod]
    [DataRow(SeatTier.Standard, 10.00)]
    [DataRow(SeatTier.Premium, 20.00)]
    [DataRow(SeatTier.VIP, 35.00)]
    public void CheckTierPrice(SeatTier tier, double expected)
    {
        SeatPriceLogic logic = new SeatPriceLogic();
        SeatModel[,] seatMap = MakeTestSeatMap();

        double result = logic.GetTierPrice(seatMap, tier);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TierPriceOfEmptySeatMapIsZero()
    {
        SeatPriceLogic logic = new SeatPriceLogic();
        SeatModel[,] seatMap = new SeatModel[16, 6];

        double result = logic.GetTierPrice(seatMap, SeatTier.VIP);

        Assert.AreEqual(0.0, result);
    }
}
