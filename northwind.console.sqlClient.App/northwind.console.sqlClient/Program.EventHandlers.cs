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
}
