using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Algorithms
{
    static class C_MinCAlgorithm 
    {
        static int minc = int.MaxValue;
        public static int FindMinC(ConflictSet conflicts)
        {
            FindMinCHelper(conflicts, 0);
            return minc;
        }

        private static void FindMinCHelper(ConflictSet conflicts, int m)
        {
            if (m >= minc || MinC_Utils.containEmptySet(conflicts) == true)
            { //1st & 3rd base case
                return;
            }
            else if (MinC_Utils.isConflictSetEmpty(conflicts) == true)
            {  //2nd base case
                minc = m;
                return;
            }
            // Dual Reduce

            // compute conflicts
            Gate s = MinC_Utils.getMostfrequentlyComp(conflicts);
            ConflictSet conflictsMinusS = MinC_Utils.ConflictsMinusComponent(conflicts, s);
            ConflictSet conflictsWithoutS = MinC_Utils.ConflictsWithoutComponent(conflicts, s);

            FindMinCHelper(conflictsMinusS, m);
            FindMinCHelper(conflictsWithoutS, m + 1);
        }
    }
}
