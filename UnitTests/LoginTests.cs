namespace UnitTests;



[TestClass]
public sealed class Test1
{


    [DataTestMethod]
    [DataRow("kevin@kevin.nl", "kevin")]
    public void LoginValidCredentials(string m, string p)
    {
        AccountsLogic l = new();
        AccountsAccess access = new();

        AccountModel result = l.CheckLogin(m, p);

        Assert.IsNotNull(result);
        Assert.AreEqual(m, result.Email);
        Assert.AreEqual(p, result.Password);
    }

    [DataTestMethod]
    [DataRow("kevin@kevin.nl", "wrong")]
    [DataRow("wrong1", "kevin")]
    [DataRow("wrong2", "wrong")]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void LoginInvalidCredentials(string m, string p)
    {
        AccountsLogic l = new();

        AccountModel result = l.CheckLogin(m, p);

        Assert.IsNull(result);
    }

    [DataTestMethod]
    [DataRow("customer@test.nl", true)]
    [DataRow("john.doe@mail.com", true)]
    [DataRow("customer", false)]
    [DataRow("customer@test", false)]
    [DataRow("@test.nl", false)]
    [DataRow("customer.test@nl", false)]
    public void CheckLoginEmailFormat(string email, bool expected)
    {
        AccountsLogic l = new();

        bool result = l.CheckEmail(email);

        Assert.AreEqual(expected, result);
    }
}
