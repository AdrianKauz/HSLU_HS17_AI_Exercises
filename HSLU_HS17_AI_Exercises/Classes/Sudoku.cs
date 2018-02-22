using HSLU_HS17_AI_Exercises.Interfaces;
using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Module:  "HSLU - Artificial Intelligence"
 * Lecture: "Introduction to Artificial Intelligence"
 * Chapter: "Constraint Programming 1 – Modelling with OR-Tools"
 * Task:    "Sudoku-Solver with Google OR-Tools"
 * Author:  "Adrian Kauz", with code-examples by Prof. Dr. Marc Pouly
 */

namespace HSLU_HS17_AI_Exercises.Classes
{
    class Sudoku: IExercises
    {
        public void doWork()
        {
            Solver solver = new Solver("Sudoku");

            // 9x9 Matrix of Decision Variables in {1..9}:
            IntVar[,] board = solver.MakeIntVarMatrix(9, 9, 1, 9);

            // 9*9 Sudoku to resolve:
            int[,] sudoku = new int[,] {
                {0, 0, 3,   0, 7, 8,   0, 6, 2},
                {8, 0, 0,   0, 6, 1,   5, 0, 7},
                {0, 0, 0,   4, 0, 0,   0, 0, 8},

                {0, 0, 5,   0, 0, 0,   8, 0, 0},
                {3, 9, 7,   8, 0, 5,   1, 4, 6},
                {0, 0, 6,   0, 0, 0,   7, 0, 0},

                {9, 0, 0,   0, 0, 4,   0, 0, 0},
                {6, 0, 1,   2, 8, 0,   0, 0, 4},
                {5, 7, 0,   6, 1, 0,   2, 0, 0},
            };

            // Feed solver with the predefined Sudoku...
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 9; column++) {
                    if (sudoku[row, column] != 0) {
                        solver.Add(board[row, column] == (sudoku[row, column]));
                    }
                }
            }

            // Handy C# LINQ type for sequences:
            IEnumerable<int> RANGE = Enumerable.Range(0, 9);

            // Each row/column contains only different values:
            foreach (int i in RANGE) {
                // Rows:
                solver.Add((from j in RANGE select board[i, j]).ToArray().AllDifferent());

                // Columns:
                solver.Add((from j in RANGE select board[j, i]).ToArray().AllDifferent());
            }

            // Handy C# LINQ type for sequences:
            IEnumerable<int> CELL = Enumerable.Range(0, 3);

            // Each Sub-matrix contains only different values:
            foreach (int i in CELL) {
                foreach (int j in CELL) {
                    solver.Add((from di in CELL from dj in CELL select board[i * 3 + di, j * 3 + dj]).ToArray().AllDifferent());
                }
            }

            DecisionBuilder db = solver.MakePhase(
                board.Flatten(),
                Solver.INT_VAR_SIMPLE,
                Solver.INT_VALUE_SIMPLE);

            solver.NewSearch(db);

            int counter = 1;
            Console.WriteLine();
            while (solver.NextSolution()) {
                PrintSodoku(board, counter++);
            }

            solver.EndSearch();

            // Display solver information:
            Console.WriteLine("  Time:\t\t" + solver.WallTime());        // "The WallTime() in ms since the creation of the solver."
            Console.WriteLine("  Solutions:\t" + solver.Solutions());    // "The number of solutions found since the start of the search."
            Console.WriteLine("  Failures:\t" + solver.Failures());      // "The number of failures encountered since the creation of the solver."
            Console.WriteLine("  Branches:\t" + solver.Branches());      // "The number of branches explored since the creation of the solver."
        }

        private void PrintSodoku(IntVar[,] currentSolution, int counter)
        {
            Console.WriteLine("Solution " + counter + ":");

            // Top Line
            Console.WriteLine("  ╔═══════════╦═══════════╦═══════════╗");

            // Field
            for (int row = 0; row < 9; row++) {
                Console.Write("  ║ ");
                for (int column = 0; column < 9; column++) {
                    Console.Write("{0}", currentSolution[row, column].Value());

                    if ((column + 1) % 3 == 0) {
                        Console.Write(" ║ ");
                    } else {
                        Console.Write(" │ ");
                    }
                }

                if ((row == 2) || (row == 5)) {
                    Console.WriteLine("\n  ╠═══════════╬═══════════╬═══════════╣");
                } else {
                    if(row != 8) {
                        Console.WriteLine("\n  ║───┼───┼───║───┼───┼───║───┼───┼───║");
                    }
                }
            }

            // Bottom Line
            Console.WriteLine("\n  ╚═══════════╩═══════════╩═══════════╝");
        }
    }
}
