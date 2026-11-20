public class PasswordHider
{
    public string PassWordHider()
    {
        string input = "";

        while (true)
        {
            ConsoleKeyInfo userInput = Console.ReadKey(true);

            if (userInput.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (userInput.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(userInput.KeyChar))
            {
                input += userInput.KeyChar;
                Console.Write("*");
            }
        }

        return input;
    }
}