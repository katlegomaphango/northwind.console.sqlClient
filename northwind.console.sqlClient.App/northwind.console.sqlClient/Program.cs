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


