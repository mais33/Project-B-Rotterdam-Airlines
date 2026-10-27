// See https://aka.ms/new-console-template for more information
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true; // added to let dapper ignore underscores in database

Console.WriteLine("=== Welcome to the Rotterdam Airlines ===");
Menu.Start();
