# Habit Tracker

A simple console-based habit tracking application built with C# and SQLite. This project allows users to create, view, update, and delete habits and their associated records, making it easy to track progress on daily routines.  This is my first project working with SQLite.

## Features

- **Create Habits:** Add new habits with custom names and measurement units.
- **Track Records:** Log daily records for each habit (e.g., water intake, exercise, reading).
- **View Data:** Display all habits and their records in a tabular format.
- **Update & Delete:** Modify or remove habits and individual records.
- **Seed Test Data:** Quickly populate the database with sample habits and records for testing.
- **Error Handling:** Exceptions thrown to the console to prevent unexpected crashes.

## Project Structure

```
habit_tracker/
├── Program.cs                # Main entry point
├── habit_tracker.csproj      # Project file
├── scripts/
│   ├── helpers/              # Database helpers and seeders
│   ├── models/               # Data models (Habit, DrinkingWater)
│   ├── sql/                  # SQL CRUD operations
│   └── ui/                   # Console UI and input management
├── habit-Tracker.db          # SQLite database file
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- SQLite (no setup required; uses [`Microsoft.Data.Sqlite`](habit_tracker/scripts/helpers/DatabaseSeeder.cs ))

### Build and Run

1. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

2. **Build the project:**
   ```sh
   dotnet build
   ```

3. **Run the application:**
   ```sh
   dotnet run --project habit_tracker.csproj
   ```

### Usage

- Follow the on-screen menus to manage habits and records.
- Create, Read, Update, & Delete Habits and their associated Records.
- Each Habit generates a unique Table to store associated Records.
- Enter `.test` at the main menu to seed the database with sample data.

## Code Overview

- Main logic is in [`Program`](habit_tracker/Program.cs ).
- Database operations are handled by helpers in `scripts/helpers`.
- Models for Habits and Records are in `scripts/models`.
- SQL CRUD operations are in `scripts/sql`.
- Console UI and input validation are in `scripts/ui`.

## Project Retrospective

This project took a lot of research to complete.  Learning concepts like OOP, DRY, SOLID, SRP, and KISS to implement across the program was more difficult than I imagined.  I honestly feel like I could have spent another month designing everything to be "perfect", but the more I looked at the code the more overwhelming it became to think through.  As I get more comfortable with SQL and OOP, this will be a great project to look back on and redo.  Any feedback and direction on what I should focus on and improve is greatly appreciated!

## License

MIT License

---