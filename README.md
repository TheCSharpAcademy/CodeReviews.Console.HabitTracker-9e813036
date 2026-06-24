# Habit Tracker

A simple console-based habit tracker. This is a project for the C# Academy course.
You can view your habits, create new ones and log new entries, update them or delete them.
Entries have a date in "yyyy-MM-dd" format and the amount of times you've done the habit that day.

## How to use

As this is a console application you just need to input the option you want and press enter.

### Main Menu

[!Main menu image](Habit tracker/docs/images/main_menu.png)

On starting the program you get presented all the options available.

#### View habit list

[!View habit list image](Habit tracker/docs/images/view_habit_list.png)

Shows all the habits saved you are tracking.

#### View habit entries

[!View habit entries image](Habit tracker/docs/images/view_habit_entries.png)

Shows every entry of every habit.

#### View all information

[!View all information image](Habit tracker/docs/images/view_all_information.png)

Shows all the habits and their total amount and all the habit entries.

#### Create new habit

[!Create new habit image](Habit tracker/docs/images/create_new_habit.png)

Write the name for the new habit (you can't repeat names).

#### Insert habit entry

[!Insert habit entry image](Habit tracker/docs/images/create_new_habit.png)

Choose a habit then input a date in "yyyy-MM-dd" format and the number of ocurrences in that day.

#### Update habit entry

[!Update habit entry image](Habit tracker/docs/images/update_habit_entry.png)

Choose a habit then an entry to update its date and/or amount.

#### Delete habit

[!Delete habit image](Habit tracker/docs/images/delete_habit.png)

Delete a habit and all its entries.

#### Delete habit entries

[!Delete habit entries image](Habit tracker/docs/images/delete_habit_entries.png)

Delete one or all habit entries of a specific habit.

# My thoughts on this project

## What I've learned

- It's the first time I've used SQlite, but it's a fairly easy library to use and extremely useful for pretty much all my needs, including videogame development. It's great to have learned it.
- Even though I've used SQL a lot, I've never truly been confortable using it as I didn't understand the inner workings. Thanks to using SQlite I've demistified the magic of databases a lot and grown more confident about using them.
- Expanding on the previous point, I've been using AI to learn many new things about programming, including all the under the hood stuff that SQL does (in this case SQlite). 
	Using AI to code is probably not a good idea for learning (as it does the work for you), but it's amazing for asking questions and getting in depth explanations. Definitely a great tool.
- You really need to make many projects in order to have a vision on a program's architecture. It's obvious by the spaghetti code that I lack experience designing a program. Specially being the first time using SQlite I think my code is not well organized, but I think I'm improving bit by bit.

## Things to improve

- The architecture of the project, definitely. Now that I understand databases a bit better I know my next project using them will have cleaner and better structured code.
- The final polishing of the program. It's console-based so can't be much better, but there's probably some touches I could do to improve the flow.
- One of the cons of using AI is that you can grow too dependent on it. There's some code I've copied (not literal copy paste but mostly copied) and, at least for learning, I don't like to do that because half of the learning process is doing things yourself. So I need to find a good balance.
- This same file, the README. I've never actually written an extensive one (barely some lines), so I'm not sure how much should I document the functionality of the projects. I guess it's not the biggest problem as you are allowed to be creative.