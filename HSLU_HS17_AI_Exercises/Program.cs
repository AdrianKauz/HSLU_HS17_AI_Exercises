using HSLU_HS17_AI_Exercises.Interfaces;
using HSLU_HS17_AI_Exercises.Classes;
using System;
using System.Collections.Generic;

/*
 * Module:  "HSLU - Artificial Intelligence"
 * Lecture: "Introduction to Artificial Intelligence"
 * Chapter: "Constraint Programming 1 – Modelling with OR-Tools"
 * Author:  "Adrian Kauz"
 */

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
            exercises.Add("Sum Frame Sudoku", new SumFrameSudoku());
            exercises.Add("Magic Squares", new MagicSquares());
            exercises.Add("Binoxxo", new Binoxxo());
            exercises.Add("XmasPuzzle", new XmasPuzzle());
            

            exercises["XmasPuzzle"].doWork();

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
