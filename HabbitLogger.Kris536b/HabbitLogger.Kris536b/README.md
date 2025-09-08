# Habit Logger

A simple console-based habit tracking application written in **C#** with **SQLite**.  
This app allows users to log daily water drinking habits, store the data in a database, and view, update, or delete their entries.

---

## Features

- **Database creation**  
  - On startup, the app creates a `HabitLogger.db` SQLite database if it doesn’t already exist.  
  - It also creates a `drinkingWater` table inside the database.  

- **CRUD operations**  
  - **View All Records**: List all saved records with ID, date, and quantity.  
  - **Insert Record**: Add a new record with date and quantity.  
  - **Update Record**: Modify the date and/or quantity of an existing record.  
  - **Delete Record**: Remove a record by ID.  

- **Error handling**  
  - Handles invalid inputs for dates and numbers.  
  - Handles invalid record IDs when updating or deleting.  
  - Uses try/catch blocks to ensure the application never crashes.  

- **Input validation**  
  - Dates must be entered in `dd-MM-yy` format.  
  - Quantities must be whole numbers (no decimals).  
  - Entering `0` always returns you to the main menu.  

---

## How to Use

1. **Run the application**  
   Start the console app. It will create the database and table if they don’t exist.  

2. **Main Menu**  
   You will see the following options:

	   MAIN MENU

	1. View All Records

	2. Insert Record

	3. Delete Record

	4. Update Record

	5. Exit Application


3. **Insert a record**  
- Choose `2` from the menu.  
- Enter the date in the format `dd-MM-yy`.  
- Enter the number of glasses (whole number).  

4. **View records**  
- Choose `1` to see all records.  
- Each record shows `Id - Date - Quantity`.  

5. **Update a record**  
- Choose `4`.  
- Enter the `Id` of the record to update.  
- Enter the new date and/or quantity.  

6. **Delete a record**  
- Choose `3`.  
- Enter the `Id` of the record to delete.  

7. **Exit**  
- Choose `0` to exit the application.  

---
