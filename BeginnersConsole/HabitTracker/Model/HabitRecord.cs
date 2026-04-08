namespace HabitTracker.Model
{
    public class HabitRecord
    {
        public int Id { get; set; }
        public string HabitName { get; set; }
        public string Unit { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}