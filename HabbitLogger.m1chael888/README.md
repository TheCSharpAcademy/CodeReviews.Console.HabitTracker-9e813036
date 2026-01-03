# HabitLogger
Trackington HabitLogger is a simple console app developed in C# using SQLite for storage. The app allows the user to track occurances of a habit by supplying a quantity of occurances and the date they occured on.

# Requirements
The given requirements are as follows"
* The application must allow users to log occurances of a habit
* The application should initialize and use a real SQLite database for all CRUD operations
* The database may only be handled via ADO.NET
* Errors should be handled to prevent application crashes
* Avoid code repetition
* Include a read me file discussing the app and its development

Optional challenge requirements completed:
* Use parameterized quereies to protect against injection attacks
* Let the user create habits in addition to those provided during initialization
* Seed habits and occurance data into the application when initializing

# How it works
* When opening the app, the user will usually be greeted by the main menu
<img width="415" height="335" alt="image" src="https://github.com/user-attachments/assets/ad105115-eda9-478c-b2a8-d5d9618d0c16" />

* There may be a brief delay while the database seeds data when launching for the first time
<img width="295" height="28" alt="image" src="https://github.com/user-attachments/assets/f6922c2b-f60e-419f-a3be-08ba2af850d6" />

* When choosing a main menu option, the user is then prompted to enter the number of the habit they would like to work with. Users are also able to enter 0 to cancel any follow-up prompt like this throughout the app. In this case, we'll view the aoccurances of habit 1
<img width="526" height="90" alt="image" src="https://github.com/user-attachments/assets/4f6f83a3-b4be-4c2e-ac0d-25c2f9373589" />

* The user is then shown any records associated with their selected habit. Habits and occurances are saved in a relational database, with a one to many relationship between habits and occurances. Here we are viewing the data seeded during initialization
<img width="419" height="579" alt="image" src="https://github.com/user-attachments/assets/95bdfd89-c722-4520-a831-af2781dc6f5c" />

* The CRUD menu options on each habit page function similarly to the main menu, the user first enters the record number they would like to work with, and they are then prompted for relevant inputs depending on their chosen operation. For example when updating, users will be asked to enter a new date and quantity for their chosen occurance record. Lets change record 75 to reflect 1000 occurances on todays date (1/3/2026)
  - Similarly, the other main menu options to handle habits can be used via the same simple console interface

* After returning to the menu with d, entering x will close the app
<img width="441" height="200" alt="image" src="https://github.com/user-attachments/assets/ce7247b6-7d2d-4fa5-b857-d5ec7d4319b0" />

# Thoughts/Challenges

This was my first time implementing SQLite with C#, and my first time interfacing with a database at a lower level of abstraction (only ado.net). Ive also realized that even while trying to follow dry principles, even a seemingly simple console app like this can start to become messy with dozens of methods, an object oriented approach with seperate class files wouldve made changes easier
