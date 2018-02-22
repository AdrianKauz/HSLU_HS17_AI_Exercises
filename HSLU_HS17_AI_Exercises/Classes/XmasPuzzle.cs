using HSLU_HS17_AI_Exercises.Interfaces;
using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Module:  "HSLU - Artificial Intelligence"
 * Lecture: "Introduction to Artificial Intelligence"
 * Chapter: "Constraint Programming 1 – Modelling with OR-Tools"
 * Task:    "XmasPuzzle-Solver with Google OR-Tools"
 * Author:  "Adrian Kauz"
 */

namespace HSLU_HS17_AI_Exercises.Classes
{
    class XmasPuzzle: IExercises
    {
        private readonly int size = 8;
        private readonly int X = -1; // X defines a possible place for a lost xmas-gift
        private readonly int[,] initialField;
        private readonly int maxGifts = 16;

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
            Solver solver = new Solver("Xmas Puzzle");

            // n x m Matrix
            IntVar[,] board = new IntVar[this.size, this.size];

            // Feed IntVar-matrix and feed solver with the predefined Field value...
            for (int row = 0; row < this.size; row++) {
                for (int column = 0; column < this.size; column++) {
                    if (this.initialField[row, column] == X) {
                        board[row, column] = solver.MakeIntVar(X, 0);  // Range of -1 to 0
                    } else {
                        board[row, column] = solver.MakeIntVar(0, 8);
                        
                        // board[row, column] MUST have the value of field[row, column]
                        solver.Add(board[row, column] == this.initialField[row, column]);
                    }
                }
            }

            // Search field-matrix for hints about the lost gifts and group the neighbours.
            // Sum of neighbours with "-1"-Value should be the same value like the inverted hint-value.
            // As an example: If hint is "4" then there should be four times a value of "-1" around the hint.
            // This should be enought to solve this puzzle.

            // Handy C# LINQ type for sequences:
            IEnumerable<int> RANGE = Enumerable.Range(0, 8);
            IEnumerable<int> ROW_LENGTH;
            IEnumerable<int> COLUMN_LENGTH;
            int rowOffset;
            int columnOffset;

            // Maximum of lost gifts
            solver.Add((from di in RANGE from dj in RANGE where this.initialField[di, dj] == -1 select board[di, dj]).ToArray().Sum() == -this.maxGifts);

            // Outer and inner loop searches for hints
            for (int row = 0; row < this.size; row++) {
                for (int column = 0; column < this.size; column++) {
                    // If a hint was found, then group all neighbours with a value of "-1"
                    if (this.initialField[row, column] != X) {

                        // Now it's getting ugly...
                        if(row == 0) {
                            if(column == 0) {
                                ROW_LENGTH = Enumerable.Range(0, 2);
                                COLUMN_LENGTH = Enumerable.Range(0, 2);
                                rowOffset = 0;
                                columnOffset = 0;
                            }
                            else if (column == this.size - 1) {
                                ROW_LENGTH = Enumerable.Range(0, 2);
                                COLUMN_LENGTH = Enumerable.Range(0, 2);
                                rowOffset = 0;
                                columnOffset = -1;
                            }
                            else {
                                ROW_LENGTH = Enumerable.Range(0, 2);
                                COLUMN_LENGTH = Enumerable.Range(0, 3);
                                rowOffset = 0;
                                columnOffset = -1;
                            }
                        }
                        else if (row == this.size - 1) {
                            if (column == 0) {
                                ROW_LENGTH = Enumerable.Range(0, 2);
                                COLUMN_LENGTH = Enumerable.Range(0, 2);
                                rowOffset = -1;
                                columnOffset = 0;
                            }
                            else if (column == this.size - 1) {
                                ROW_LENGTH = Enumerable.Range(0, 2);
                                COLUMN_LENGTH = Enumerable.Range(0, 2);
                                rowOffset = -1;
                                columnOffset = -1;
                            }
                            else {
                                ROW_LENGTH = Enumerable.Range(0, 2);
                                COLUMN_LENGTH = Enumerable.Range(0, 3);
                                rowOffset = -1;
                                columnOffset = -1;
                            }
                        }
                        else {
                            if (column == 0) {
                                ROW_LENGTH = Enumerable.Range(0, 3);
                                COLUMN_LENGTH = Enumerable.Range(0, 2);
                                rowOffset = -1;
                                columnOffset = 0;
                            }
                            else if (column == this.size - 1) {
                                ROW_LENGTH = Enumerable.Range(0, 3);
                                COLUMN_LENGTH = Enumerable.Range(0, 2);
                                rowOffset = -1;
                                columnOffset = -1;
                            }
                            else {
                                ROW_LENGTH = Enumerable.Range(0, 3);
                                COLUMN_LENGTH = Enumerable.Range(0, 3);
                                rowOffset = -1;
                                columnOffset = -1;
                            }
                        }

                        foreach (int i in ROW_LENGTH) {
                            foreach (int j in COLUMN_LENGTH) {
                                solver.Add((from di in ROW_LENGTH from dj in COLUMN_LENGTH where this.initialField[row + rowOffset + di, column + columnOffset + dj] == -1 select board[row + rowOffset + di, column + columnOffset + dj]).ToArray().Sum() == -this.initialField[row, column]);
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
