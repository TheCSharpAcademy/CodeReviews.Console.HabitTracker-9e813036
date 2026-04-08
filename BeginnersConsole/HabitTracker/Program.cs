using HabitTracker.Application;
using HabitTracker.Data;
using HabitTracker.Infrastructure;
using HabitTracker.UI;
using Microsoft.Extensions.Configuration;

string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .Build();

string connectionString = @"Data Source=habit-Tracker.db";
using var speechService = new SpeechService(configuration);
var inputService = new InputService(speechService);
var databaseManager = new DatabaseManager(connectionString);
var habitController = new HabitController(databaseManager, inputService);
databaseManager.CreateTable();

databaseManager.SeedData();

await GetUserInputAsync();

async Task GetUserInputAsync()
{
    Console.Clear();
    while (true)
    {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("------------------------------------------\n");

        string? command = await inputService.GetInputAsync();

        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye!\n");
                return;

            case "1":
                habitController.GetAllRecords();
                break;

            case "2":
                await habitController.InsertAsync();
                break;

            case "3":
                await habitController.DeleteAsync();
                break;

            case "4":
                await habitController.UpdateAsync();
                break;

            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}