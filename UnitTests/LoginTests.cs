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
}
