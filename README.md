# Habit Logger
This is a basic console app focusing on allowing the user to track different habits, put together with over a week's worth of procrastination, frustration, and confusion.  
This has been put together for the CSharp Acdemy's Habit Logger project, following the [requirments](https://thecsharpacademy.com/project/12/habit-logger) set there.  

# App Features
* Uses Sqlite database for more a more permanent way to store data. (Sample Image)
  ![Screenshot of a sample Sqlite database viewed through DB browser](https://i.imgur.com/ZPmuqs1.png)
  
* Presents a basic but functional user interface when dealing with actions  
  ![](https://i.imgur.com/WyjuX6A.png)

* Many action menus employ enum mapping for easier use.
    * Common shortcuts are included such as "y" or "n" instead of "yes" or "no"
    * As well as more natural ways of writing actions such as "delete habit" instead of "deleteHabit"

* Supports multiple habits
    * Habits are generated using customized names and unit types that are entered by the user.
    * When creating a habit, names are verified to be unique. The user will be prompted for a new input if that name is already taken.
    * Each habit stores its own instances, each having instance specific data.

* Input is run through specified Get Input methods throughout the program's code.
    * Reusable methods for getting different types including: Int, DateTime, Enum, and String.
    * GetEnumInput checks responses against an enum map, while GetStringInput allows for an optional list of valid responses to check for.
    * All Input methods are tested under various test cases using NUnit.

* Makes use of Habit and HabitInstance record models for an easier and more natural passing of data between methods, especially between Program.cs and SqliteHelper.cs methods.
 
# Behind the Scenes (the boring part)
When I first started this project, the idea of having to use Sqlite, which I hadn't even heard of before, seemed very daunting, so daunting that I barely made any progress in starting over the first 2 days. At first the problem was just figuring out how to start. Since I had never used Sql or Sqlite before, I had no idea how to even put down the foundation for the project. I started by writing down a jumbled mess of psuedocode, and after a while I was able to map out a rough idea of how to start. The firsts things I ended up writing were the input helper methods, since I had already used them in other projects, as well as a simple method to make an Sqlite database when the program starts. From there, I slowly branched out into other parts of the program, slowly replacing my psuedocode with real functional code, and slowly the foundation I was working with began to grow. The code I wrote ended up becoming more and more messy as it grew. Therefore, even before I had finished the code, I knew I would need to refactor it later. While in past projects I used AI a lot in order to progress through code, for this project I would estimate that I used it a little less, despite having to learn more new subjects with Sqlite. I did definitly still use it, but I like to think that being able to use it a little less, atleast for the first version of the code, is hopefully a tiny step towards being more capable on my own with things I've learned. And throughout that process, I eventually managed to create a fully functional project, although it was still messy. From that point onwards however, I did use AI a lot more to refactor the base code I had already written. In fact, most of the code currently in this application is not something I made on my own, or something I could have made. But, I do know how each line works, and I did put in the effort to learn the different topics, hoping that next time I'll be able to integrate those same topics on my own, now that I know of them and have used them before. By my reasoning atleast, I figured that for a project focused on learning, that should be okay, even though I didn't come up with most of the code on my own.
