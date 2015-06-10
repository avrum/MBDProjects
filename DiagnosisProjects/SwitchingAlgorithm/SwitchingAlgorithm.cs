using System;
using System.Collections.Generic;
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

        //The main algorithm
        public DiagnosisSet findDiagnosis()
        {
            //Probably we will have few 'findDiagnosis' function with different 'while' condition (i.e - findDiagnosisByTime, findDiagnosisByCount,...)
            while (_diagnosisSet.Count < _requiredNumOfDiagnosis)
            {
                findDiagnosisFromConflicts();
                findConflictsFromDiagnosis();
            }
            return _diagnosisSet;
        }

        private void findDiagnosisFromConflicts()
        {
            //hittingSet should be List<HashSet<CompSet>> but my hittingSet algoritm not yet support it
            List<HashSet<int>> hittingSets = SwitchingAlgorithmHittingSetFinder.findHittingSet(null, 10);//null = _conflictsSet
            foreach (HashSet<int> hittingSet in hittingSets)
            {
                bool isConsistent = _constraintSystemSolver.CheckConsistensy(_observation, null);//null = hittingSet (this is aviram code)
                if (isConsistent)
                {
                    addComponentToSet(_conflictSet, null);//send the hitting set as conflict 
                }
                else
                {
                    Diagnosis diagnosis = new Diagnosis(getOppositeComponenetsList(null));
                    addComponentToSet(_diagnosisSet, diagnosis);
                }
            }
        }

        private void findConflictsFromDiagnosis()
        {
            //hittingSet should be List<HashSet<CompSet>> but my hittingSet algoritm not yet support it
            List<HashSet<int>> hittingSets = SwitchingAlgorithmHittingSetFinder.findHittingSet(null, 10); // null = _diagnosisSet
            foreach (HashSet<int> hittingSet in hittingSets)
            {
                bool isConsistent = _constraintSystemSolver.CheckConsistensy(_observation, null);// null = hittingSet
                if (!isConsistent)
                {
                    addComponentToSet(_diagnosisSet, null);//send the hitting set as diagnosis 
                }
                else
                {
                    Conflict conflict = new Conflict(getOppositeComponenetsList(null));
                    addComponentToSet(_conflictSet, conflict);
                }
            }
        }

        private List<Gate> getOppositeComponenetsList(CompSet compSet)
        {
            List<Gate> allSystemComponents  = _observation.TheModel.Components;
            List<Gate> compSetComponents = compSet.getComponents();
            List<Gate> oppositeComponenetsList = new List<Gate>();
            foreach (Gate systemComponent in allSystemComponents)
            {
                if (!compSetComponents.Contains(systemComponent))
                {
                    oppositeComponenetsList.Add(systemComponent);
                }
            }
            return oppositeComponenetsList;
        }

        private void addComponentToSet(Sets sets , CompSet compSet)
        {
            CompSet minimizedSet = minimizeCompSet();
            //add to global diagnosis set while keepin minimal subset - using Trie
        }

        private CompSet minimizeCompSet()
        {
            throw new NotImplementedException();
        }

    }
}
