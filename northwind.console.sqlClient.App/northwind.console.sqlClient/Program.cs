using Microsoft.Data.SqlClient;

SqlConnectionStringBuilder builder = new();

builder.InitialCatalog = "northWind";
builder.MultipleActiveResultSets = true;
builder.Encrypt = true;
builder.TrustServerCertificate = true;
builder.ConnectTimeout = 10;




