using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class SwitchingDiagnosticEngine
    {

        public static DiagnosisSet findDiagnosisHaltByFirstDiagnosis(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet)
        {
            SwitchingAlgorithm switchingAlgorithm = new SwitchingAlgorithm(observation, initialConflictsSet, initialDiagnosisSet, 1);
            return switchingAlgorithm.FindDiagnosis(-1);
        }

        public static DiagnosisSet findDiagnosisHaltByTime(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int timeOutMillis){
            SwitchingAlgorithm switchingAlgorithm = new SwitchingAlgorithm(observation, initialConflictsSet, initialDiagnosisSet, Int32.MaxValue);
            return switchingAlgorithm.FindDiagnosis(timeOutMillis);
        }

        public static DiagnosisSet findDiagnosisHaltByQuantiy(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int quantity)
        {
            SwitchingAlgorithm switchingAlgorithm = new SwitchingAlgorithm(observation, initialConflictsSet, initialDiagnosisSet, quantity);
            return switchingAlgorithm.FindDiagnosis(-1);
        }

    }
}
