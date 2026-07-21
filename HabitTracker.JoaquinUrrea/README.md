# HabitLogger

A simple console-based habit tracker I built while learning C#!

Log your daily habits, set goals, and keep track of your progress with a straightforward menu. It's my first serious project, so if you find something that could be better, feel free to let me know!

## Features

- ✅ Create & log your habits with custom quantity goals
- ✅ Update or delete habit records
- ✅ View all your logged habits in a nice table format
- ✅ Data saves to SQLite, so nothing gets lost
- ✅ Built with Spectre.Console for a better-looking terminal UI
- ✅ Includes unit tests (still learning TDD!)

## Technology Stack

- C# with .NET 8.0
- SQLite for data storage (Microsoft.Data.Sqlite)
- Spectre.Console for prettier terminal output
- NUnit for unit tests

## Project Structure

```
HabitLogger/
├── HabitLogger/                    # Main application
│   ├── Program.cs                 # Application entry point
│   ├── HabitInterface.cs          # User interface & prompts
│   ├── HabitController.cs         # Business logic & workflow
│   ├── DatabaseFunctions.cs       # Database CRUD operations
│   ├── HabitInterface.cs          # Interface definitions
│   ├── Enums.cs                   # Enumeration definitions
│   ├── Helpers.cs                 # Utility functions
│   └── HabitLogger.csproj         # Project configuration
├── HabitLogger.Tests/              # Unit tests
│   └── HabitLogger.Tests.csproj   # Test project configuration
├── HabitLogger.sln                # Solution file
└── README.md                       # This file
```

## Core Components

### HabitInterface
Handles the user interactions and prompts. This is where the menu lives.

### HabitController
The main logic - handles inserting, updating, deleting, and reading habit records.

### DatabaseFunctions
All the SQLite database operations live here - pretty straightforward CRUD stuff.

### Helpers
Small utility functions for things like database setup and validation.

## Usage

Just run it:

```bash
dotnet run
```

Then pick what you want to do from the menu:
1. **Insert Record** - Log a habit
2. **Delete Record** - Remove a log entry
3. **Update Record** - Edit a log entry
4. **View All Records** - See all your habits
5. **Exit** - Close the app

## Getting Started

### Prerequisites
- .NET 8.0
- Visual Studio or any code editor you like

### Installation

1. Clone it:
```bash
git clone https://github.com/joaquinauc/HabitLogger.git
cd HabitLogger
```

2. Restore packages:
```bash
dotnet restore
```

3. Run it:
```bash
dotnet run --project HabitLogger
```

## Testing

I added unit tests with NUnit! Run them with:

```bash
dotnet test
```

These helped me learn how to write testable code and catch bugs early. I'm still learning TDD, so there's probably room for improvement! 😅

## Dependencies

- Microsoft.Data.Sqlite - SQLite support
- Spectre.Console - Nice terminal formatting
- SQLitePCLRaw.core - SQLite bindings
- NUnit - Testing framework