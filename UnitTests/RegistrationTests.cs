namespace UnitTests;

[TestClass]
public sealed class RegistrationTests
{
    [DataTestMethod]
    [DataRow("+353871234567")]
    [DataRow("+353 87 123 4567")]
    [DataRow("+353-87-123-4567")]
    [DataRow("00353871234567")]
    [DataRow("0871234567")]
    [DataRow("087 123 4567")]
    [DataRow("+353 1 234 5678")]
    [DataRow("0612345678")]
    [DataRow("+31612345678")]
    [DataRow("+31 6 12345678")]
    public void PhoneNumberValid(string phoneNumber)
    {
        AccountsLogic logic = new();

        bool result = logic.PhoneNumberCheck(phoneNumber);

        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("06")]
    [DataRow("1234567")]
    [DataRow("1234567890123456")]
    [DataRow("++353871234567")]
    [DataRow("353+871234567")]
    [DataRow("+35387abc4567")]
    [DataRow("-0612345678")]
    [DataRow("0612345678-")]
    public void PhoneNumberInvalid(string phoneNumber)
    {
        AccountsLogic logic = new();

        bool result = logic.PhoneNumberCheck(phoneNumber);

        Assert.IsFalse(result);
    }
}
