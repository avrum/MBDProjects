using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public object Locker = ConstraintSystemSolverLocker.Locker;
        private static Dictionary<Wire, CspTerm> wireTermsDictionary;

        public static ConstraintSystemSolver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConstraintSystemSolver();
                    wireTermsDictionary = new Dictionary<Wire, CspTerm>();
                }

                return instance;
            }
        }

        private ConstraintSystemSolver()
        {
            Solver = ConstraintSystem.CreateSolver();
        }

        /// <summary>
        ///  If true its a diagnosis
        //   If false its a conflict
        /// </summary>
        /// <param name="observation"></param>
        /// <param name="posibleConflict"></param>
        /// <returns></returns>
        public bool CheckConsistensy(Observation observation, List<Gate> posibleConflict)
        {
            lock (Locker)
            {
                //Debug.WriteLine("SAT CheckConsistensy IN!");

                // Set broken gates
                foreach (Gate gate in posibleConflict)
                {
                    gate.IsNotHealthy = true;
                }

                /*
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
            */

                // Add components constrain
                List<Gate> allSystemGates = observation.TheModel.Components;
                foreach (Gate gate in allSystemGates)
                {
                    gate.AddConstaint();
                }


                ConstraintSolverSolution solution;
                try
                {
                    solution = Solver.Solve();
                }
                catch (Exception)
                {
                    Reset();
                    solution = Solver.Solve();
                }
                


                // If true its a diagnosis
                // If false its a conflict
                bool explainOutput = solution.HasFoundSolution;

                if (!explainOutput)
                {
                    Debug.WriteLine("SAT Doesn't found a solution. The new node is N-O-T a diagnosis!");
                }

                //Reset
                instance = null;
                Solver.ResetSolver();
                ConstraintSystemSolver newSolver = ConstraintSystemSolver.Instance;
                wireTermsDictionary.Clear();

                //Revert broken
                foreach (Gate gate in posibleConflict)
                {
                    gate.IsNotHealthy = false;
                }

                //Debug.WriteLine("SAT CheckConsistensy OUT!");
                return explainOutput;

            }
        }

        public static void Reset()
        {
            //Debug.WriteLine("SAT Reset IN!");
            instance = null;
            ConstraintSystemSolver newSolver = ConstraintSystemSolver.Instance;
            wireTermsDictionary.Clear();
            //Debug.WriteLine("SAT Reset OUT!");
        }

        public CspTerm AddWireTerm(Wire wire)
        {
            //Debug.WriteLine("SAT IN!");
      //      lock (Locker)
      //      {
                if (!wireTermsDictionary.ContainsKey(wire))
                {
                    CspTerm term = Solver.CreateBoolean();

                    if (wire.Type == Wire.WireType.i)
                    {
                        if (wire.Value)
                        {
                            term = Solver.True;
                        }
                        else
                        {
                            term = Solver.False;
                        }
                    }
                    else
                    {
                        term = Solver.CreateBoolean();
                    }

                    wire.term = term;

                    wireTermsDictionary.Add(wire, term);
                }      
        //    }
            //Debug.WriteLine("SAT OUT!");
            return wireTermsDictionary[wire];
        }


    }
}
