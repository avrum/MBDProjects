using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class SwitchingDiagnosticEngine
    {

        public static DiagnosisSet FindDiagnosisHaltByFirstDiagnosis(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet)
        {
            var switchingAlgorithm = new SwitchingAlgorithm(observation, initialConflictsSet, initialDiagnosisSet, 1);
            return switchingAlgorithm.FindDiagnosis(-1);
        }

        public static DiagnosisSet FindDiagnosisHaltByTime(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int timeOutMillis){
            var switchingAlgorithm = new SwitchingAlgorithm(observation, initialConflictsSet, initialDiagnosisSet, Int32.MaxValue);
            return switchingAlgorithm.FindDiagnosis(timeOutMillis);
        }

        public static DiagnosisSet FindDiagnosisHaltByQuantiy(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int quantity)
        {
            var switchingAlgorithm = new SwitchingAlgorithm(observation, initialConflictsSet, initialDiagnosisSet, quantity);
            return switchingAlgorithm.FindDiagnosis(-1);
        }

    }
}
