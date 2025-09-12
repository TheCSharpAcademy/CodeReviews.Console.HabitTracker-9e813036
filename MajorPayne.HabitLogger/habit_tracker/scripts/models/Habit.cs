using System;

namespace models
{
    public class Habit
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required string TableName { get; set; }
    }
}