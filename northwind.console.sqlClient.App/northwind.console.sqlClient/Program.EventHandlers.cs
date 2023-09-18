using System.Data;
using Microsoft.Data.SqlClient;

namespace northwind.console.sqlClient;

internal class Program
{
    static void Connection_StateChange(Object sender, StateChangeEventArgs e)
    {
        ConsoleColor prevColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;

        Console.WriteLine($"State change from {e.OriginalState} to {e.CurrentState}");

        Console.ForegroundColor = prevColor;
    }

    static void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
    {
        ConsoleColor prevColor = Console.ForegroundColor;
        Console.ForegroundColor= ConsoleColor.DarkBlue;

        Console.WriteLine($"Info: {e.Message}");

        foreach (var error in e.Errors)
        {
            Console.WriteLine($"    Error: {error}");
        }

        Console.ForegroundColor = prevColor;
    }
}
