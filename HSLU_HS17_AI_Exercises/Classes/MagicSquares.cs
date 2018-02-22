using HSLU_HS17_AI_Exercises.Interfaces;
using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Module:  "HSLU - Artificial Intelligence"
 * Lecture: "Introduction to Artificial Intelligence"
 * Chapter: "Constraint Programming 1 – Modelling with OR-Tools"
 * Task:    "MagicSquares-Solver with Google OR-Tools"
 * Author:  "Adrian Kauz"
 */

namespace HSLU_HS17_AI_Exercises.Classes
{
    class MagicSquares:IExercises
    {
        private readonly int size = 4;

        public void doWork()
        {
            Solver solver = new Solver("MagicSquares");

            // Calculate magic number --> Be carefull with very large numbers! --> (int) casting! 
            int magicNumber = (int)((Math.Pow(this.size, 3) + this.size) / 2);

            // n x n Matrix of Decision Variables:
            IntVar[,] board = solver.MakeIntVarMatrix(this.size, this.size, 1, size * size);

            // Set all squares different
            solver.Add(board.Flatten().AllDifferent());

            // Handy C# LINQ type for sequences:
            IEnumerable<int> RANGE = Enumerable.Range(0, this.size);

            // Each sum of row/column equals the magic number:
            foreach (int i in RANGE) {
                // Rows:
                solver.Add((from j in RANGE select board[i, j]).ToArray().Sum() == magicNumber);

                // Columns:
                solver.Add((from j in RANGE select board[j, i]).ToArray().Sum() == magicNumber);
            }
            
            // Now the two diagonal parts
            solver.Add((from j in RANGE select board[j, j]).ToArray().Sum() == magicNumber);
            solver.Add((from j in RANGE select board[j, this.size - 1 - j]).ToArray().Sum() == magicNumber);

            DecisionBuilder db = solver.MakePhase(
                board.Flatten(),
                Solver.INT_VAR_SIMPLE,
                Solver.INT_VALUE_SIMPLE);

            solver.NewSearch(db);

            // Calculate first result for this solution
            // If more solutions are necessary, you can extend this part with a while(...)-Loop
            if (solver.NextSolution()) {
                printSquare(board);
            }
            
            solver.EndSearch();

            if(solver.Solutions() == 0) {
                Console.WriteLine("No possible solution was found for the given magic Square! :-(");
            }
        }

        private void printSquare(IntVar[,] currentSolution)
        {
            Console.WriteLine("Possible solution:\n");

            for (int row = 0; row < this.size; row++) {
                for (int column = 0; column < this.size; column++) {
                    Console.Write("  {0,2}", currentSolution[row, column].Value());
                }
                Console.Write("\n");
            }

            Console.WriteLine();
        }
    }
}
