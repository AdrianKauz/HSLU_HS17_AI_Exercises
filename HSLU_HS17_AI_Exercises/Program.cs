using AI_HS2017_Exercises.Exercises;
using AI_HS2017_Exercises.Classes;
using System;
using System.Collections.Generic;

namespace AI_HS2017_Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<String, IExercises> exercises = new Dictionary<string, IExercises>();

            exercises.Add("Grocery Store", new GroceryStore());
            exercises.Add("Cryptogram Puzzle", new CryptogramPuzzle());
            exercises.Add("Sudoku", new Sudoku());

            exercises["Sudoku"].doWork();
            
            Console.ReadLine();
        }
    }
}
