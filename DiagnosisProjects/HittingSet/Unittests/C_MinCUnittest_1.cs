using DiagnosisProjects.HittingSet.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Unittests
{
    class C_MinCUnittest_1
    {
        public static void RunTest()
        {
            Gate g1 = new OneInputComponent(1, Gate.Type.and);
            Gate g2 = new OneInputComponent(2, Gate.Type.and);
            Gate g3 = new OneInputComponent(3, Gate.Type.and);
            Gate g4 = new OneInputComponent(4, Gate.Type.and);
            Gate g5 = new OneInputComponent(5, Gate.Type.and);
            Gate g6 = new OneInputComponent(6, Gate.Type.and);

            Conflict c1 = new Conflict(new List<Gate>() { g1, g2, g3, g4 });
            Conflict c2 = new Conflict(new List<Gate>() { g1, g5, g6 });
            Conflict c3 = new Conflict(new List<Gate>() { g5, g6 });

            ConflictSet cs = new ConflictSet();
            cs.Conflicts = new List<Conflict>() { c1, c2, c3 };

            int minc = C_MinCAlgorithm.FindMinC(cs);
            Console.WriteLine("C_MinC Unit test 1. minc = " + minc);

            int x = 0;
        }
    }
}
