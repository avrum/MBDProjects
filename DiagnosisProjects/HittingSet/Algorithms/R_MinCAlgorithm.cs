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

        public static MicC_Diagnosis FindMinC(ConflictSet conflicts)
        {
            int minc = infinity;

            if (MinC_Utils.isConflictSetEmpty(conflicts) == true)
            {  //1st base case
                return new MicC_Diagnosis();
            }
            else if (MinC_Utils.containEmptySet(conflicts) == true)
            { //2nd base case
                MicC_Diagnosis infinityDiagnosis = new MicC_Diagnosis();
                infinityDiagnosis.cardinality = infinity;
                return infinityDiagnosis;
            }
            // Dual Reduce
            MinC_Utils.DualReduce(conflicts);

            // compute conflicts
            Gate s = MinC_Utils.getMostfrequentlyComp(conflicts);
            ConflictSet conflictsMinusS= MinC_Utils.ConflictsMinusComponent(conflicts, s);
            ConflictSet conflictsWithoutS = MinC_Utils.ConflictsWithoutComponent(conflicts, s);

            MicC_Diagnosis mincDiagnosisNotContainS = FindMinC(conflictsMinusS);
            MicC_Diagnosis mincDiagnosisContaintS = FindMinC(conflictsWithoutS);
            mincDiagnosisContaintS.AddCompToDiagnosis(s);
            if (mincDiagnosisContaintS.cardinality <= mincDiagnosisNotContainS.cardinality)
            {
                return mincDiagnosisContaintS;
            }
            else
            {
                return mincDiagnosisNotContainS;
            }

        }
    }
}
