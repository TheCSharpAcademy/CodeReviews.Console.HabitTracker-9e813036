# **ConsoleHabitLogger**
C# Console Application to track time spent on habits and save it in a database.

## **Requirements**
This is an application where you’ll log occurrences of a habit.
This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
Users need to be able to input the date of the occurrence of the habit
The application should store and retrieve data from a real database
When the application starts, it should create a sqlite database, if one isn’t present.
It should also create a table in the database, where the habit will be logged.
The users should be able to insert, delete, update and view their logged habit.
You should handle all possible errors so that the application never crashes.
You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
Follow the DRY Principle, and avoid code repetition.

## **Features**

### SQLite Database
  Program uses SQLite as a basic database.
  If there is no database found one is created with default Hydration and Gym tables inserted with random data.
  
### Console Menu
User can navigate through program by use of menu.
User can proceed with many actions such as:
  - Create new table
  - Change table in use
  - Insert data into table
  - Delete data from table
  - Update records
  - Add newly created habits!

## **Challenges**
  - The most difficult problem was with checking if user created table has TEXT or DATA Column in it before inserting data into it.
    The problem was that there was no data and i couldn't check if it's a normal string or if it's supposed to be a date by it and
    to make it even more difficult SQLite does not use DATE, it just save it as TEXT. I found out about many solutions to this problem
    thanks to Stackoverflow and ChatGPT, but still couldn't fit it in my project. In the end I made it work with kinda roundabout solution,
    but I'm happy that it works.
  - Dates were a little difficult to deal with, but I made it through
  - I never used SQLite before and as mentioned above it made some hard time for me. I didn't use SQL much and needed to review some commands.
  - Because of implementation of new things I needed to change my code several times.

## **What Should I Remember In The Future**
  - Spend some time thinkning about design of project before coding
  - Make better use of git and it's functionalities
  - Use more classes if it's possible to make code more readable
    
