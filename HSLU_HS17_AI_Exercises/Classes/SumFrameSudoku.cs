using HSLU_HS17_AI_Exercises.Interfaces;
using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Module:  "HSLU - Artificial Intelligence"
 * Lecture: "Introduction to Artificial Intelligence"
 * Chapter: "Constraint Programming 1 – Modelling with OR-Tools"
 * Task:    "SumFrameSudoku-Solver with Google OR-Tools"
 * Author:  "Adrian Kauz"
 */

namespace HSLU_HS17_AI_Exercises.Classes
{
    class SumFrameSudoku:IExercises
    {
        // Numbers of the outside frame of the Sum Frame Sudoku
        // Values are read from left to right or from top to bottom
        private readonly int[] topFrame = { 15, 18, 12, 11, 21, 13, 15, 17, 13 };
        private readonly int[] bottomFrame = { 15, 9, 21, 10, 16, 19, 13, 15, 17 };
        private readonly int[] leftFrame = { 8, 15, 22, 11, 13, 21, 18, 19, 8 };
        private readonly int[] rightFrame = { 22, 8, 15, 22, 12, 11, 15, 13, 17 };

        public void doWork ()
        {
            Solver solver = new Solver("SumFrameSudoku");

            // 9x9 Matrix of Decision Variables in {1..9}:
            IntVar[,] board = solver.MakeIntVarMatrix(9, 9, 1, 9);

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

            // Feed solver with the outside frame numbers
            for (int x = 0; x < 9; x++) {
                // Row
                solver.Add(board[x, 0] + board[x, 1] + board[x, 2] == this.leftFrame[x]);
                solver.Add(board[x, 6] + board[x, 7] + board[x, 8] == this.rightFrame[x]);

                // Column
                solver.Add(board[0, x] + board[1, x] + board[2, x] == this.topFrame[x]);
                solver.Add(board[6, x] + board[7, x] + board[8, x] == this.bottomFrame[x]);
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
        }

        private void PrintSodoku (IntVar[,] currentSolution, int counter)
        {
            Console.WriteLine("Solution " + counter + ":\n");

            // Top Line
            Console.WriteLine("  ╔═══════════╦═══════════╦═══════════╗");

            // Field
            for (int row = 0; row < 9; row++) {
                Console.Write("  ║ ");
                for (int column = 0; column < 9; column++) {
                    Console.Write("{0}", currentSolution[row, column].Value());

                    if ((column + 1) % 3 == 0) {
                        Console.Write(" ║ ");
                    }
                    else {
                        Console.Write(" │ ");
                    }
                }

                if ((row == 2) || (row == 5)) {
                    Console.WriteLine("\n  ╠═══════════╬═══════════╬═══════════╣");
                }
                else {
                    if (row != 8) {
                        Console.WriteLine("\n  ║───┼───┼───║───┼───┼───║───┼───┼───║");
                    }
                }
            }

            // Bottom Line
            Console.WriteLine("\n  ╚═══════════╩═══════════╩═══════════╝");
        }
    }
}
