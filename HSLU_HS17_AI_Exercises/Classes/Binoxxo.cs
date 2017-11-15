using Google.OrTools.ConstraintSolver;
using HSLU_HS17_AI_Exercises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSLU_HS17_AI_Exercises.Classes
{
    class Binoxxo:IExercises
    {
        private readonly int size = 4;

        public void doWork()
        {
            if (this.size % 2 == 0) {
                Solver solver = new Solver("Binoxxo");

                // n x n Matrix
                IntVar[,] board = solver.MakeIntVarMatrix(this.size, this.size, 0, 1);

                // Handy C# LINQ type for sequences:
                IEnumerable<int> RANGE = Enumerable.Range(0, this.size);

                // Set all rows and columns Different
                foreach (int i in RANGE) {
                    // Rows
                    IntVar[] currentRow = (from j in RANGE select board[i, j]).ToArray();

                    int asdad = currentRow.Where(x => x.Value().Equals(0));

                    solver.Add(currentRow.Where(x => x is 0))

                    solver.Add((from j in RANGE select board[i, j]).ToArray().Count() == (from j in RANGE select board[i, j]).ToArray().Count());


                    for (int x = i; x < this.size; x++) {
                        // Rows
                        (from j in RANGE select board[i, j]).ToArray().Count();
                    }


                }
            } else {
                Console.WriteLine("Given size of {0} not allowed! Must be multible of 2!", this.size);
            }
        }
    }
}
