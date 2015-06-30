using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Algorithms
{
    static class R_MinCAlgorithm
    {
        static int infinity = int.MaxValue;

        public static int FindMinC(ConflictSet conflicts)
        {
            int minc = infinity;

            if (MinC_Utils.isConflictSetEmpty(conflicts) == true)
            {  //1st base case
                return 0;
            }
            else if (MinC_Utils.containEmptySet(conflicts) == true)
            { //2nd base case
                return infinity;
            }
            // Dual Reduce

            // compute conflicts
            Gate s = MinC_Utils.getMostfrequentlyComp(conflicts);
            ConflictSet conflictsMinusS= MinC_Utils.ConflictsMinusComponent(conflicts, s);
            ConflictSet conflictsWithoutS = MinC_Utils.ConflictsWithoutComponent(conflicts, s);

            int mincWithS = FindMinC(conflictsMinusS);
            int mincWithoutS = FindMinC(conflictsWithoutS) + 1;
            return Math.Min(mincWithS, mincWithoutS);

        }
    }
}
