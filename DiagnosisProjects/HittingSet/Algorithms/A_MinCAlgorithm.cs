using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Algorithms
{
    class A_MinCAlgorithm
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

            Gate s = MinC_Utils.getMostfrequentlyComp(conflicts);
            ConflictSet conflictsWithoutS = MinC_Utils.ConflictsWithoutComponent(conflicts, s);

            MicC_Diagnosis mincDiagnosisContaintS = FindMinC(conflictsWithoutS);
            
            mincDiagnosisContaintS.AddCompToDiagnosis(s);
            return mincDiagnosisContaintS;

        }
    }
}
