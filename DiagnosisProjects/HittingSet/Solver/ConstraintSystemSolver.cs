using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    class ConstraintSystemSolver
    {
        private static ConstraintSystemSolver instance;
        public ConstraintSystem Solver { get; private set; }

        public static ConstraintSystemSolver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConstraintSystemSolver();
                }

                return instance;   
            }
        }

        private ConstraintSystemSolver()
        {
            Solver = ConstraintSystem.CreateSolver();
        }

        /// <summary>
        ///  If true its a diagnose
        //   If false its a conflict
        /// </summary>
        /// <param name="observation"></param>
        /// <param name="posibleConflict"></param>
        /// <returns></returns>
        public bool CheckConsistensy(Observation observation, List<Gate> posibleConflict)
        {
            // Set broken gates
            foreach (Gate gate in posibleConflict)
            {
                gate.IsBroken = true;
            }

            // Add input constrain
            List<Wire> allInputWires = observation.TheModel.Input;
            foreach (Wire wire in allInputWires)
            {
                Solver.AddConstraints(wire.GetTerm());
            }


            // Add output constrain
            List<Wire> allOutputWires = observation.TheModel.Output;
            foreach (Wire wire in allOutputWires)
            {
                Solver.AddConstraints(wire.GetTerm());
            }


            // Add components constrain
            List<Gate> allSystemGates = observation.TheModel.Components;
            foreach (Gate gate in allSystemGates)
            {
                gate.AddConstaint();
            }

            ConstraintSolverSolution solution = Solver.Solve();


            // If true its a diagnose
            // If false its a conflict
            bool explainOutput = solution.HasFoundSolution;

            //Reset
            Solver.ResetSolver();

            //Revert broken
            foreach (Gate gate in posibleConflict)
            {
                gate.IsBroken = false;
            }

            return explainOutput;
        }
    }
}
