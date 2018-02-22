using HSLU_HS17_AI_Exercises.Interfaces;
using Google.OrTools.ConstraintSolver;
using System;

/*
 * Module:  "HSLU - Artificial Intelligence"
 * Lecture: "Introduction to Artificial Intelligence"
 * Chapter: "Constraint Programming 1 – Modelling with OR-Tools"
 * Task:    "CryptogramPuzzle-Solver with Google OR-Tools"
 * Author:  "Adrian Kauz", with code-examples by Prof. Dr. Marc Pouly
 */

namespace HSLU_HS17_AI_Exercises.Classes
{
    class CryptogramPuzzle: IExercises
    {
        public void doWork()
        {
            Solver solver = new Solver("Cryptogram");

            // One decision variable for each character:
            IntVar S = solver.MakeIntVar(0, 9);
            IntVar E = solver.MakeIntVar(0, 9);
            IntVar N = solver.MakeIntVar(0, 9);
            IntVar D = solver.MakeIntVar(0, 9);
            IntVar M = solver.MakeIntVar(0, 9);
            IntVar O = solver.MakeIntVar(0, 9);
            IntVar R = solver.MakeIntVar(0, 9);
            IntVar Y = solver.MakeIntVar(0, 9);

            IntVar[] vars = new IntVar[] { S, E, N, D, M, O, R, Y };

            // SEND + MORE = MONEY:
            IntVar send = (S * 1000 + E * 100 + N * 10 + D ).Var();
            IntVar more = (M * 1000 + O * 100 + R * 10 + E).Var();
            IntVar money = (M * 10000 + O * 1000 + N * 100 + E * 10 + Y).Var();

            solver.Add(send + more == money);

            //Leading characters must not be zero:
            solver.Add(S != 0);
            solver.Add(M != 0);

            // Part B: All characters must take different values:
            // Otherwise we get 155 possible solutions!
            solver.Add(vars.AllDifferent());

            DecisionBuilder db = solver.MakePhase(
                vars,                               // Decision variables to resolve
                Solver.INT_VAR_SIMPLE,              // Variable selection policy for search
                Solver.INT_VALUE_SIMPLE);           // Value selection policy for search

            solver.NewSearch(db);

            while (solver.NextSolution()) {
                Console.WriteLine("    SEND   |      " + send.Value());
                Console.WriteLine(" +  MORE   |   +  " + more.Value());
                Console.WriteLine(" -------   |   -------");
                Console.WriteLine("   MONEY   |     " + money.Value() + "\n");
            }

            solver.EndSearch();

            // Display the number of solutions found:
            Console.WriteLine("Total solution(s): " + solver.Solutions());
        }
    }
}
