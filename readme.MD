# HabitTracker

A Console App that logs the occurrences of a habit.

# Functionality
- SQLite database 
- CRUD functions

# Given requirements

- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- Users need to be able to input the date of the occurrence of the habit
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The users should be able to insert, delete, update and view their logged habit.
- You should handle all possible errors so that the application never crashes.
- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- Seed data into the database when the database gets created for the first time

# Challenges

- The design of classes. It's kind of hard for me to write clean code.
- Even if it was the first time I used ADO, in other languages I had written something similar

# Lessons learned

- I tried to create a database class that only returned the SqliteConnection, but I didn't remember that when they keyword **using** is applied to a disposable object, this object is discarded when the program gets out of the local scope of that function. So this class was useless
- I separate the user input when a lot of the options were using the same methods (asking for date, asking for id habit, etc)
- I created some flow diagrams for the application, but they're in spanish so for the future either I use a dedicated program for this or I write my diagrams in English.

# Areas to improve
- Single Responsibility. 
- Unit Testing
