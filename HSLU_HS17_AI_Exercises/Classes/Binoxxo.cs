using Google.OrTools.ConstraintSolver;
using HSLU_HS17_AI_Exercises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSLU_HS17_AI_Exercises.Classes
{
    class Binoxxo:IExercises
    {
        private readonly int size = 14;

        public void doWork()
        {
            if (this.size % 2 == 0 && this.size > 3) {
                Solver solver = new Solver("Binoxxo");

                // n x n Matrix
                IntVar[,] board = solver.MakeIntVarMatrix(this.size, this.size, 0, 1);

                // Handy C# LINQ type for sequences:
                IEnumerable<int> RANGE = Enumerable.Range(0, this.size);

                // Each row and each column should have an equal amount of X and O's:
                foreach (int i in RANGE) {
                    // Row
                    solver.Add((from j in RANGE select board[i, j]).ToArray().Sum() == (this.size / 2));

                    // Column
                    solver.Add((from j in RANGE select board[j, i]).ToArray().Sum() == (this.size / 2));
                }

                // Each row and column should be different: (Yes! Shit is working....)
                foreach (int i in RANGE) {
                    for (int x = i + 1; x < this.size; x++) {
                        // Rows:
                        solver.Add(
                        (from j in RANGE select board[i, j] * (int)Math.Pow(2, j)).ToArray().Sum() !=
                        (from j in RANGE select board[x, j] * (int)Math.Pow(2, j)).ToArray().Sum()
                        );

                        // Columns:
                        solver.Add(
                            (from j in RANGE select board[j, i] * (int)Math.Pow(2, j)).ToArray().Sum() !=
                            (from j in RANGE select board[j, x] * (int)Math.Pow(2, j)).ToArray().Sum()
                            );
                    };
                }

                // Max two cells next to each other should have the same value (This shit is working too!)
                IEnumerable<int> GROUP_START = Enumerable.Range(0, this.size - 2);
                IEnumerable<int> GROUP = Enumerable.Range(0, 3);
                foreach (int x in RANGE) {
                    foreach (int y in GROUP_START) {
                        // Rows:
                        solver.Add((from j in GROUP select board[x, y + j]).ToArray().Sum() > 0);
                        solver.Add((from j in GROUP select board[x, y + j]).ToArray().Sum() < 3);

                        // Columns:
                        solver.Add((from j in GROUP select board[y + j, x]).ToArray().Sum() > 0);
                        solver.Add((from j in GROUP select board[y + j, x]).ToArray().Sum() < 3);
                    }
                }

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

                if (solver.Solutions() == 0) {
                    Console.WriteLine("No possible solution was found for the given Binoxxo! :-(");
                }
            } else {
                Console.WriteLine("Given size of {0} not allowed! Must be multiple of 2!", this.size);
            }
        }

        private void printSquare(IntVar[,] currentSolution)
        {
            Console.WriteLine("Possible solution:\n");

            for (int row = 0; row < this.size; row++) {
                for (int column = 0; column < this.size; column++) {
                    Console.Write(" {0}", (currentSolution[row, column].Value() == 0 ? '0' : 'X'));
                }
                Console.Write("\n");
            }

            Console.WriteLine();
        }
    }
}
