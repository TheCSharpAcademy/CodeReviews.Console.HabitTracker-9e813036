using System;
using System.Globalization;
using models;

namespace menu_manager
{
    public class MenuManager
    {
        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\nHABIT TRACKER");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("0. Close Application.");
            Console.WriteLine("1. View Existing Habits.");
            Console.WriteLine("2. Create New Habit.");
            Console.WriteLine("3. Update Existing Habit.");
            Console.WriteLine("4. Delete Existing Habit.");
            Console.Write("-----------------------------------\n");
        }

        public static void HabitMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\nHabit Menu");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("0. Return to Main Menu.");
            Console.WriteLine("1. View All Records.");
            Console.WriteLine("2. Insert Record.");
            Console.WriteLine("3. Update Record.");
            Console.WriteLine("4. Delete Record.");
            Console.Write("-----------------------------------\n");
        }

        public static void EnterHabit()
        {
            Console.WriteLine("\n\nEnter Habit name to access database.  Press 0 to return to main menu\n\n");
        }

        public static void HabitNameMenu()
        {
            Console.WriteLine("Enter the name of the habit you would like to track:  Press 0 to return to main menu\n");
        }

        public static void HabitTypeMenu()
        {
            Console.WriteLine("Enter the unit of measurement for your habit to track (e.g., oz, lbs, cups, gal):  Press 0 to return to main menu\n");
        }

        public static void DateMenu()
        {
            Console.WriteLine("Enter the date (mm-dd-yy):  Press 0 to return to main menu\n");
        }

        public static void WaterMenu()
        {
            Console.WriteLine("\n\nPlease insert number of glasses or other measure of your choice " + "(no decimals allowed)\n\n");
        }

        public static void UpdateMenu()
        {
            Console.WriteLine("\n\nPlease select the index number of the record you would like to update:  Press 0 to return to main menu\n\n");
        }

        public static void DeleteMenu()
        {
            Console.WriteLine("\n\nPlease select the index number of the record you would like to delete:  Press 0 to return to main menu\n\n");
        }     
    }
}