//https://medium.com/@cummingsi1993/the-operation-result-pattern-a-simple-guide-fe10ff959080
public class OperationResult
{
    public bool Success {get;set;}
    public string FailureMessage{get;set;}

    protected OperationResult()
    {
        Success = true;
    }
    protected OperationResult(string message)
    {
        Success = false;
        FailureMessage = message;
    }

    public static OperationResult FailureResult(string message)
    {
        return new OperationResult(message);
    }

    public static OperationResult SuccessResult()
    {
        return new OperationResult();
    }
}