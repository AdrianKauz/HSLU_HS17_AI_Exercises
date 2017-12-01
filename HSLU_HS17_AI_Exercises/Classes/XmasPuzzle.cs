using Google.OrTools.ConstraintSolver;
using HSLU_HS17_AI_Exercises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSLU_HS17_AI_Exercises.Classes
{
    class XmasPuzzle: IExercises
    {
        private readonly int size = 8;
        private readonly int X = -1; // Marks a paket
        private readonly int[,] initialField;

        public XmasPuzzle()
        {
            this.initialField = new int[,] {
                {1, X, X, X, 2, X, X, 1},
                {X, 1, 2, X, 3, X, X, 1},
                {X, 2, X, 1, X, X, X, 0},
                {X, 2, 1, X, X, 2, 3, 1},
                {X, X, X, 2, X, X, X, X},
                {1, X, X, X, 4, 3, X, X},
                {X, 1, X, X, 4, X, 3, X},
                {1, X, X, 2, X, 2, X, 1}
            };
        }


        public void doWork()
        {
            Solver solver = new Solver("Binoxxo");

            // n x n Matrix
            //IntVar[,] board = solver.MakeIntVarMatrix(this.size, this.size, -1, 8);
            IntVar[,] board = new IntVar[this.size, this.size];

            // Feed IntVar-matrix and feed solver with the predefined Field value...
            for (int row = 0; row < this.size; row++) {
                for (int column = 0; column < this.size; column++) {
                    if (initialField[row, column] == X) {
                        board[row, column] = solver.MakeIntVar(X, 0);
                    } else {
                        board[row, column] = solver.MakeIntVar(0, 8);
                        
                        // board[row, column] must have the value of field[row, column]
                        solver.Add(board[row, column] == (initialField[row, column]));
                    }
                }
            }

            // Search field-matrix for hints about the lost gifts and group the neighbours.
            // Sum of neighbours with "-1"-Value should be the same value like the inverted hint-value.
            // This should be enought to solve this puzzle.

            // Handy C# LINQ type for sequences:
            IEnumerable<int> RANGE = Enumerable.Range(0, 8);
            IEnumerable<int> FULL_CELL = Enumerable.Range(0, 3);
            IEnumerable<int> SMALL_CELL = Enumerable.Range(0, 2);

            // Maximum of lost gifts
            solver.Add((from di in RANGE from dj in RANGE where this.initialField[di, dj] == -1 select board[di, dj]).ToArray().Sum() == -16);

            // Outer loop searches for hints
            for (int row = 0; row < this.size; row++) {
                for (int column = 0; column < this.size; column++) {
                    // If a hint was found, then group all neighbours with a value of "-1"
                    if (this.initialField[row, column] != X) {

                        // Now it's getting ugly...
                        if(row == 0) {
                            if(column == 0) {
                                foreach (int i in SMALL_CELL) {
                                    foreach (int j in SMALL_CELL) {
                                        solver.Add((from di in SMALL_CELL from dj in SMALL_CELL where this.initialField[row + di, column + dj] == -1 select board[row + di, column + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            } else if (column == this.size - 1) {
                                foreach (int i in SMALL_CELL) {
                                    foreach (int j in SMALL_CELL) {
                                        solver.Add((from di in SMALL_CELL from dj in SMALL_CELL where this.initialField[row  + di, column - 1 + dj] == -1 select board[row + di, column - 1 + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                            else {
                                foreach (int i in SMALL_CELL) {
                                    foreach (int j in FULL_CELL) {
                                        solver.Add((from di in SMALL_CELL from dj in FULL_CELL where this.initialField[row + di, column - 1 + dj] == -1 select board[row + di, column - 1 + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                        } else if (row == this.size - 1) {
                            if (column == 0) {
                                foreach (int i in SMALL_CELL) {
                                    foreach (int j in SMALL_CELL) {
                                        solver.Add((from di in SMALL_CELL from dj in SMALL_CELL where this.initialField[row - 1 + di, column + dj] == -1 select board[row - 1 + di, column + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                            else if (column == this.size - 1) {
                                foreach (int i in SMALL_CELL) {
                                    foreach (int j in SMALL_CELL) {
                                        solver.Add((from di in SMALL_CELL from dj in SMALL_CELL where this.initialField[row - 1 + di, column - 1 + dj] == -1 select board[row - 1 + di, column - 1 + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                            else {
                                foreach (int i in SMALL_CELL) {
                                    foreach (int j in FULL_CELL) {
                                        solver.Add((from di in SMALL_CELL from dj in FULL_CELL where this.initialField[row - 1 + di, column - 1 + dj] == -1 select board[row - 1 + di, column - 1 + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                        } else {
                            if (column == 0) {
                                foreach (int i in FULL_CELL) {
                                    foreach (int j in SMALL_CELL) {
                                        solver.Add((from di in FULL_CELL from dj in SMALL_CELL where this.initialField[row - 1 + di, column + dj] == -1 select board[row - 1 + di, column + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                            else if (column == this.size - 1) {
                                foreach (int i in FULL_CELL) {
                                    foreach (int j in SMALL_CELL) {
                                        solver.Add((from di in FULL_CELL from dj in SMALL_CELL where this.initialField[row - 1 + di, column - 1 + dj] == -1 select board[row - 1 + di, column - 1 + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                            else {
                                foreach (int i in FULL_CELL) {
                                    foreach (int j in FULL_CELL) {
                                        solver.Add((from di in FULL_CELL from dj in FULL_CELL where this.initialField[row - 1 + di, column - 1 + dj] == -1 select board[row - 1 + di, column - 1 + dj]).ToArray().Sum() == -this.initialField[row, column]);
                                    }
                                }
                            }
                        }
                    }
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
                printField(board);
            }
        }


        private void printField(IntVar[,] currentSolution)
        {
            Console.WriteLine("Possible solution:\n");
            string tempString;

            for (int row = 0; row < this.size; row++) {
                for (int column = 0; column < this.size; column++) {
                    switch(currentSolution[row, column].Value()) {
                        case -1:
                            tempString = "X";
                            break;
                        case 0:
                            // Make a short comparison with the initial field to keep the zero value
                            if (this.initialField[row, column] == 0) {
                                tempString = "0";
                            } else {
                                tempString = " ";
                            }

                            break;
                        default:
                            tempString = this.initialField[row, column].ToString();
                            break;
                    }

                    Console.Write(" {0}", tempString);
                }
                Console.Write("\n");
            }

            Console.WriteLine();
        }
    }
}
