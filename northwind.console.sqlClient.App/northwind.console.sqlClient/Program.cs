using Microsoft.Data.SqlClient;
using System.Data;

SqlConnectionStringBuilder builder = new()
{
    InitialCatalog = "northWind",
    MultipleActiveResultSets = true,
    Encrypt = true,
    TrustServerCertificate = true,
    ConnectTimeout = 10
};

Console.WriteLine("Connect to: ");
Console.WriteLine("\t1. - SQL Server on local machine");
Console.WriteLine("\t2. - Azure SQL Database");
Console.WriteLine("\t3. - Azure SQL Edge");
Console.WriteLine();
Console.Write("\tPress a key: ");

ConsoleKey key = Console.ReadKey().Key;
Console.WriteLine();
Console.WriteLine();

if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    builder.DataSource = ".";
}
else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    builder.DataSource = "tcp:apps-services-net7.database.windows.net,1433";
} 
else if (key is ConsoleKey.D3 or ConsoleKey.NumPad3)
{
    builder.DataSource = "tcp:127.0.0.1,1433"; // Azure SQL Edge
}
else
{
    Console.WriteLine("No data source selected.");
    return;
}

Console.WriteLine("Authenticating using: ");
Console.WriteLine("\t1. - Windows Integrated Security");
Console.WriteLine("\t2. - SQL Login, eg sa");
Console.WriteLine();
Console.Write("\tPress a key: ");

key = Console.ReadKey().Key;
Console.WriteLine();
Console.WriteLine();

if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    builder.IntegratedSecurity = true;
}
else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    builder.UserID = "sa";

    Console.Write("Enter your password: ");
    string? password = Console.ReadLine();

    if(string.IsNullOrEmpty(password))
    {
        Console.WriteLine("Password cannot be empy.");
        return;
    }

    builder.Password = password;
    builder.PersistSecurityInfo = true;
}
else
{
    Console.WriteLine("No authentication is selected.");
    return;
}

SqlConnection connection = new(builder.ConnectionString);

Console.WriteLine(connection.ConnectionString);
Console.WriteLine();

connection.StateChange += Connection_StateChange;
connection.InfoMessage += Connection_InfoMessage;

try
{
    WriteLine("Opening connection. Please wait up to {0} seconds...", builder.ConnectTimeout);
    WriteLine();
    await connection.OpenAsync();

    WriteLine($"SQL Server version: {connection.ServerVersion}");

    connection.StatisticsEnabled = true;
}
catch (SqlException ex)
{
    WriteLine($"SQL Exception: {ex.Message}");
    return;
}

//Executing queries and working with data readers using ADO.NET

//filter by unit price
Write("Enter a unit price ");
string? priceText = ReadLine();

if (!decimal.TryParse(priceText, out decimal price))
{
    WriteLine("You must enter a valid price...");
    return;
}

SqlCommand cmd = connection.CreateCommand();

WriteLine("\tExecute command using:");
WriteLine("\t\t1 - Text");
WriteLine("\t\t2 - Stored Procedure");
WriteLine();
Write("\t\tPress a key: ");

key = ReadKey().Key;
WriteLine(); 
WriteLine();

SqlParameter p1, p2 = new(), p3 = new();

if(key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    cmd.CommandType = CommandType.Text;
    cmd.CommandText = "SELECT ProductId, ProductName, UnitPrice FROM Products" + " WHERE UnitPrice > @price";
    cmd.Parameters.AddWithValue("price", price);
} else if(key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.CommandText = "GetExpensiveProducts";

    p1 = new()
    {
        ParameterName = "price",
        SqlDbType = SqlDbType.Money,
        SqlValue = price
    };

    p2 = new()
    {
        Direction = ParameterDirection.Output,
        ParameterName = "count",
        SqlDbType = SqlDbType.Int
    };

    p3 = new()
    {
        Direction = ParameterDirection.ReturnValue,
        ParameterName = "rv",
        SqlDbType = SqlDbType.Int
    };

    cmd.Parameters.Add(p1);
    cmd.Parameters.Add(p2);
    cmd.Parameters.Add(p3);
}



SqlDataReader reader = await cmd.ExecuteReaderAsync();

WriteLine("----------------------------------------------------------");
WriteLine("| {0,5} | {1,-35} | {2,8} |", "Id", "Name", "Price");
WriteLine("----------------------------------------------------------");

while (await reader.ReadAsync())
{
    WriteLine("| {0,5} | {1,-35} | {2,8:C} |",
        reader.GetFieldValueAsync<int>("ProductId"),
        reader.GetFieldValueAsync<string>("ProductName"),
        reader.GetFieldValueAsync<decimal>("UnitPrice"));
}

WriteLine("----------------------------------------------------------");

await reader.CloseAsync();

WriteLine($"Output count: {p2.Value}");
WriteLine($"Return value: {p3.Value}");

await connection.CloseAsync();