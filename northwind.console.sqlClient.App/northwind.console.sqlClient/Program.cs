using Microsoft.Data.SqlClient;

SqlConnectionStringBuilder builder = new();

builder.InitialCatalog = "northWind";
builder.MultipleActiveResultSets = true;
builder.Encrypt = true;
builder.TrustServerCertificate = true;
builder.ConnectTimeout = 10;

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
} else if (key is ConsoleKey.D3 or ConsoleKey.NumPad3)
{
    builder.DataSource = "tcp:127.0.0.1,1433"; // Azure SQL Edge
}
else
{
    Console.WriteLine("No data source selected.");
    return;
}