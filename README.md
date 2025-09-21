# HabitLogger

A console-based CRUD application to track daily habits. This project serves as a practical exercise in C# fundamentals and database interaction with SQLite.

Developed using **C#** and **SQLite**.

***

## Project Requirements

- [x] When the application starts, it creates a SQLite database if one isn’t present.
- [x] It also creates a `Habit` table in the database where the records will be logged.
- [x] The database is automatically seeded with 100 random records on its first creation to facilitate testing and development.
- [x] Users can **insert**, **delete**, **update**, and **view** their logged habits.
- [x] All user inputs are validated to prevent errors and application crashes.
- [x] The application only terminates when the user selects '0' from the main menu.
- [x] All database interactions are performed using raw SQL and parameterized queries.

***

## Features

* **SQLite Database Connection**
    * The program uses a local SQLite database (`HabitLogger.sqlite`) to store and retrieve all information.
    * If no database or `Habit` table exists, they are automatically created on program startup.

* **Automatic Data Seeding**
    * To make development easier, the application checks if the `Habit` table is empty on startup. If it is, it automatically inserts 100 randomly generated records with various habits, quantities, and dates from the past year.

* **Console-Based UI**
    * A simple and intuitive menu allows users to navigate the application's features by entering a number. The interface is cleared after each operation to keep the view clean.

* **Full CRUD Functionality**
    * **Create**: Insert new habit records with a name, unit of measurement, quantity, and date.
    * **Read**: View all existing records in the database.
    * **Update**: Modify any field (name, measurement, quantity, or date) of an existing record by specifying its ID.
    * **Delete**: Remove a specific habit record from the database using its ID.

* **Robust Input Validation**
    * The application validates all user inputs to ensure data integrity. This includes checks for valid dates (no future dates allowed), valid numbers for quantities and IDs, and non-empty strings for text fields.

***

## Challenges

* **Learning C# and SQLite**: As a foundational project, a key challenge was learning the C# syntax and the `System.Data.SQLite` library to interact with a database from scratch. This involved understanding connections, commands, readers, and proper resource management with `using` statements.

* **Date and Number Handling**: Dates are a common hurdle. I had to ensure the program could parse multiple date formats (`yyyy-MM-dd`, `dd-MM-yyyy`) and also handle the special "today" keyword. Similarly, handling numeric inputs required parsing with `CultureInfo.InvariantCulture` to avoid issues with different regional settings (e.g., using a comma vs. a period as a decimal separator).

* **Code Organization**: A primary focus was to avoid "spaghetti code." In early stages, logic can get mixed up. I addressed this by creating a dedicated `Validation` class, separating the concern of validating user input from the main program flow and database logic. This made the `Program.cs` file cleaner and the validation logic reusable.

* **Raw SQL and Security**: Writing raw SQL queries requires careful attention to syntax. A major challenge was not just making the queries work but also making them secure. I learned to use parameterized queries (`@id`, `@name`, etc.) for all operations that involve user input, which is the standard practice to prevent SQL injection attacks.

***

## Lessons Learned

* **Separate Your Concerns**: Having a distinct `Validation` class was a great lesson. It's much better to have small, focused methods and classes that do one thing well. Separating user input, validation, and database logic makes the code easier to read, debug, and maintain.

* **Always Validate User Input**: Never trust user input. Building robust validation checks from the start prevents a wide range of bugs and potential crashes. It's a key part of writing a reliable application.

* **Plan Before You Code**: Creating a basic plan for the required methods and how they'll interact can save a lot of time and prevent major refactoring later. Thinking about what data you need and how it will flow through the application is a crucial first step.

* **Parameterize Your Queries**: I learned not just the "how" but the "why" of using parameterized queries. It's a fundamental security practice that's essential for any application that interacts with a database.

***

## Areas to Improve

* **Add a Reporting Feature**: The current application only allows viewing all records. A great next step would be to add a reporting menu where users can see analytics, such as the total quantity for a specific habit (e.g., "Total km run") or the number of times a habit was performed in a given month.

* **Improve Data Display**: The output for viewing all records is functional but basic. I could integrate a library like **ConsoleTableExt** to display the data in a neatly formatted ASCII table, making it much easier to read.

* **Refine Error Handling**: While there's input validation, the application could benefit from more specific `try-catch` blocks around database operations to handle file-level exceptions, such as the database file being locked or read-only.

* **Add Unit Tests**: Creating a separate test project to write unit tests for the `Validation` class would be a great way to learn about testing in .NET and ensure the logic is always correct.
