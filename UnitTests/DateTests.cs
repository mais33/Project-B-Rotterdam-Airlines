namespace UnitTests;

[TestClass]
public sealed class DateTests
{
    [DataTestMethod]
    [DataRow("25-12-2026", true)]
    [DataRow("01-01-2000", true)]
    [DataRow("29-02-2028", true)]
    [DataRow("29-02-2026", false)]
    [DataRow("31-04-2026", false)]
    [DataRow("25-13-2026", false)]
    [DataRow("00-12-2026", false)]
    [DataRow("banana", false)]
    [DataRow("", false)]
    public void CheckReadDate(string text, bool expected)
    {
        DateTime date;

        bool result = DateLogic.TryReadDate(text, out date);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ReadDateGivesTheRightDate()
    {
        DateTime date;

        DateLogic.TryReadDate("06-12-2026", out date);

        Assert.AreEqual(6, date.Day);
        Assert.AreEqual(12, date.Month);
        Assert.AreEqual(2026, date.Year);
    }

    [DataTestMethod]
    [DataRow("25-12-2026 14:30", true)]
    [DataRow("25-12-2026 00:00", true)]
    [DataRow("25-12-2026 23:59", true)]
    [DataRow("25-12-2026 24:00", false)]
    [DataRow("25-12-2026 14:60", false)]
    [DataRow("25-12-2026", false)]
    [DataRow("banana", false)]
    [DataRow("", false)]
    public void CheckReadDateTime(string text, bool expected)
    {
        DateTime dateTime;

        bool result = DateLogic.TryReadDateTime(text, out dateTime);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ReadDateTimeGivesTheRightDateAndTime()
    {
        DateTime dateTime;

        DateLogic.TryReadDateTime("06-12-2026 20:05", out dateTime);

        Assert.AreEqual(6, dateTime.Day);
        Assert.AreEqual(12, dateTime.Month);
        Assert.AreEqual(2026, dateTime.Year);
        Assert.AreEqual(20, dateTime.Hour);
        Assert.AreEqual(5, dateTime.Minute);
    }

    [TestMethod]
    public void DateToTextUsesTwoDigits()
    {
        DateTime date = new DateTime(2026, 3, 5);

        string result = DateLogic.DateToText(date);

        Assert.AreEqual("05-03-2026", result);
    }

    [TestMethod]
    public void DateTimeToTextUsesTwoDigits()
    {
        DateTime dateTime = new DateTime(2026, 12, 6, 8, 5, 0);

        string result = DateLogic.DateTimeToText(dateTime);

        Assert.AreEqual("06-12-2026 08:05", result);
    }
}
