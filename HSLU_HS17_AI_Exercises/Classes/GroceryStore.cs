using HSLU_HS17_AI_Exercises.Interfaces;
using Google.OrTools.ConstraintSolver;
using System;

/*
 * Module:  "HSLU - Artificial Intelligence"
 * Lecture: "Introduction to Artificial Intelligence"
 * Chapter: "Constraint Programming 1 – Modelling with OR-Tools"
 * Task:    "GroceryStore-Solver with Google OR-Tools"
 * Author:  "Adrian Kauz", with code-examples by Prof. Dr. Marc Pouly
 */

namespace HSLU_HS17_AI_Exercises.Classes
{
    class GroceryStore: IExercises
    {
        public void doWork()
        {
            Solver solver = new Solver("Grocery");

            // One variable for each product:
            IntVar p1 = solver.MakeIntVar(0, 711);
            IntVar p2 = solver.MakeIntVar(0, 711);
            IntVar p3 = solver.MakeIntVar(0, 711);
            IntVar p4 = solver.MakeIntVar(0, 711);

            // Prices add up to 711:
            solver.Add(p1 + p2 + p3 + p4 == 711);

            // Product of prices is 711:
            solver.Add(p1 * p2 * p3 * p4 == 711 * 100 * 100 * 100);

            // Part B: Add symmetry breaking constrains:
            // Otherwise we get 24 possible solutions
            solver.Add(p1 <= p2);
            solver.Add(p2 <= p3);
            solver.Add(p3 <= p4);

            DecisionBuilder db = solver.MakePhase(
                new IntVar[] { p1, p2, p3, p4 },    // Decision variables to resolve
                Solver.INT_VAR_SIMPLE,              // Variable selection polivy for search
                Solver.INT_VALUE_SIMPLE);           // Value selection policy for search

            solver.NewSearch(db);

            int counter = 1;
            while (solver.NextSolution()) {
                Console.WriteLine("Solution " + counter++);
                Console.WriteLine("--------------");
                Console.WriteLine("Product 1: " + p1.Value());
                Console.WriteLine("Product 2: " + p2.Value());
                Console.WriteLine("Product 3: " + p3.Value());
                Console.WriteLine("Product 4: " + p4.Value());
                Console.WriteLine("\n");
            }

            solver.EndSearch();
        }
    }
}
