
using System.Globalization;

namespace Kerem.HabitTracker ;

    public class HabitService
    {
        private readonly SqlService _sqlService = new SqlService();
        public void CreateHabit()
        {
            Console.WriteLine();
            var habit = new Habit();
            Console.WriteLine("Please enter the name of the habit:");
            string name = habit.Name = Console.ReadLine() ?? string.Empty;
            bool validText = ValidationHelper.ValidateTextInput(name);
            if (!validText) return;
            
            bool correctDate = false;
            DateTime date = new DateTime();

            while (!correctDate)
            {
                Console.WriteLine("Please enter the date of the habit you want to create, in the format of dd/MM/yyyy");
                string selectedDate = Console.ReadLine() ?? string.Empty;
                if (DateTime.TryParseExact(selectedDate, "dd/MM/yyyy", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    Console.WriteLine($"Date format {date} is valid");
                    correctDate = true;
                }
                else
                {
                    Console.WriteLine($"Date format {selectedDate} is invalid");
                }
            }
            
   
      
            Console.WriteLine("Enter how many times you did the habit today:");
            int quantity = habit.Quantity = int.TryParse(Console.ReadLine() ?? string.Empty, out quantity) ? quantity : 0;
            bool validNumericalInput = ValidationHelper.ValidateNumericalInput(quantity);
            if (!validNumericalInput) return;
            
            _sqlService.InsertCommand(name, quantity, date);
            Console.WriteLine("Habit created successfully.");
            Console.WriteLine();
        }
        
        public void ViewHabit()
        {
            Console.WriteLine();
            Console.WriteLine("Habit list:");
            _sqlService.ViewAllCommand();
            Console.WriteLine();
        }
        
        public void DeleteHabit()
        {
            Console.WriteLine();
            _sqlService.ViewAllCommand();
            Console.WriteLine("Please enter the id of the habit you want to delete:");
            int habitId = int.TryParse(Console.ReadLine() ?? string.Empty, out habitId) ? habitId : -1;
            bool validNumericalInput = ValidationHelper.ValidateNumericalInput(habitId);
            if (!validNumericalInput) return;
            
            _sqlService.DeleteByIdCommand(habitId);
            Console.WriteLine("Habit deleted successfully.");
            Console.WriteLine();
        }
        
        public void UpdateHabitName()
        {
            Console.WriteLine();
            _sqlService.ViewAllCommand();
            Console.WriteLine("Please enter the id of the habit you want to update:");
            int habitId = int.TryParse(Console.ReadLine() ?? string.Empty, out habitId) ? habitId : 0;
            bool validNumericalInput = ValidationHelper.ValidateNumericalInput(habitId);
            if (!validNumericalInput) return;
            
            Console.WriteLine("Please enter the new name of the habit:");
            string habitName = Console.ReadLine() ?? string.Empty;
            bool validText = ValidationHelper.ValidateTextInput(habitName);
            if (!validText) return;
            
            _sqlService.UpdateNameById(habitId, habitName);
            Console.WriteLine("Habit updated successfully.");
            Console.WriteLine();
        }
        
        public void UpdateHabitDate()
        {
            Console.WriteLine();
            _sqlService.ViewAllCommand();
            Console.WriteLine("Please enter the id of the habit you want to update:");
            int habitId = int.TryParse(Console.ReadLine() ?? string.Empty, out habitId) ? habitId : 0;
            bool validNumericalInput = ValidationHelper.ValidateNumericalInput(habitId);
            if (!validNumericalInput) return;
            
            bool correctDate = false;
            DateTime newDate = new DateTime();

            while (!correctDate)
            {
                Console.WriteLine("Please enter the date of the habit you want to create, in the format of dd/MM/yyyy");
                string selectedDate = Console.ReadLine() ?? string.Empty;
                if (DateTime.TryParseExact(selectedDate, "dd/MM/yyyy", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out newDate))
                {
                    Console.WriteLine($"Date format {newDate} is valid");
                    correctDate = true;
                }
                else
                {
                    Console.WriteLine($"Date format {selectedDate} is invalid");
                }
            }
            _sqlService.UpdateDateById(habitId, newDate);
            Console.WriteLine("Habit updated successfully.");
            Console.WriteLine();
        }
        
        
    }