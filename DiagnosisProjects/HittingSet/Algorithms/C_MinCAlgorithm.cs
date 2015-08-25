using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Algorithms
{
    static class C_MinCAlgorithm 
    {
        static MicC_Diagnosis mincDiagnosis = new MicC_Diagnosis();

        public static MicC_Diagnosis FindMinC(ConflictSet conflicts)
        {
            int infinity = int.MaxValue;
            mincDiagnosis.cardinality = infinity;

            MicC_Diagnosis mDiagnosis = new MicC_Diagnosis();
            FindMinCHelper(conflicts, mDiagnosis);
            return mincDiagnosis;
        }

        private static void FindMinCHelper(ConflictSet conflicts, MicC_Diagnosis mDiagnosis)
        {
            if (mDiagnosis.cardinality >= mincDiagnosis.cardinality || MinC_Utils.containEmptySet(conflicts) == true)
            { //1st & 3rd base case
                return;
            }
            else if (MinC_Utils.isConflictSetEmpty(conflicts) == true)
            {  //2nd base case
                mincDiagnosis = mDiagnosis;
                return;
            }
            // Dual Reduce
            MinC_Utils.DualReduce(conflicts);

            // compute conflicts
            Gate s = MinC_Utils.getMostfrequentlyComp(conflicts);
            ConflictSet conflictsMinusS = MinC_Utils.ConflictsMinusComponent(conflicts, s);
            ConflictSet conflictsWithoutS = MinC_Utils.ConflictsWithoutComponent(conflicts, s);

            mDiagnosis.AddCompToDiagnosis(s);
            MicC_Diagnosis diagnosisWithoutS = new MicC_Diagnosis(mDiagnosis);
            FindMinCHelper(conflictsMinusS, diagnosisWithoutS);
            FindMinCHelper(conflictsWithoutS, mDiagnosis);
        }
    }
}
