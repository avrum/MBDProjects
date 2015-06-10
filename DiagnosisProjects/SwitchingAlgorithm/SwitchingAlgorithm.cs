using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.SwitchingAlgorithm.HittingSet;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class SwitchingAlgorithm
    {
        private Observation _observation;
        private DiagnosisSet _diagnosisSet;
        private ConflictSet _conflictSet;
        private int _requiredNumOfDiagnosis;
        private ConstraintSystemSolver _constraintSystemSolver;


        public SwitchingAlgorithm(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int requiredNumOfDiagnosis)
        {
            this._observation = observation;
            this._constraintSystemSolver = ConstraintSystemSolver.Instance;
            if (initialConflictsSet != null)
            {
                this._conflictSet = initialConflictsSet;
            }
            if (initialDiagnosisSet != null)
            {
                this._diagnosisSet = initialDiagnosisSet;
            }
            this._requiredNumOfDiagnosis = requiredNumOfDiagnosis;
        }

        public DiagnosisSet findDiagnosis()
        {
            while (_diagnosisSet.Count < _requiredNumOfDiagnosis)
            {
                findDiagnosisFromConflicts();
                findConflictsFromDiagnosis();
            }
            return _diagnosisSet;
        }

        private void findConflictsFromDiagnosis()
        {
            List<HashSet<int>> hittingSets = SwitchingAlgorithmHittingSetFinder.findHittingSet(null, 10);
            foreach (HashSet<int> hittingSet in hittingSets)
            {
                bool isConsistent = _constraintSystemSolver.CheckConsistensy(_observation, null);
                if (isConsistent)
                {
                    
                }
                else
                {
                    
                }
            }
        }

        private void findDiagnosisFromConflicts()
        {
            throw new NotImplementedException();
        }
    }
}
