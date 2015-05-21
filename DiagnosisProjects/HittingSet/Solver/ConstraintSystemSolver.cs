using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    public class ConstraintSystemSolver
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
    }
}
