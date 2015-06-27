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
        public static Dictionary<int, Gate> IdToGates = new Dictionary<int, Gate>(); 


        public SwitchingAlgorithm(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int requiredNumOfDiagnosis)
        {
            this._observation = observation;
            this._constraintSystemSolver = ConstraintSystemSolver.Instance;
            this._conflictSet = initialConflictsSet ?? new ConflictSet();
            this._diagnosisSet = initialDiagnosisSet ?? new DiagnosisSet();
            this._requiredNumOfDiagnosis = requiredNumOfDiagnosis;
            if (IdToGates.Count == 0)
            {
                buildIdToGateDictionary(observation.TheModel.Components);
            }
        }

        private void buildIdToGateDictionary(List<Gate> components)
        {
            foreach (Gate component in components)
            {
                IdToGates.Add(component.Id, component);
            }
        }

        //The Main Algorithm
        public DiagnosisSet FindDiagnosis()
        {
            //Probably we will have few 'findDiagnosis' function with different 'while' condition (i.e - findDiagnosisByTime, findDiagnosisByCount,...)
            while (_diagnosisSet.Count < _requiredNumOfDiagnosis)
            {
                FindDiagnosisFromConflicts();
                FindConflictsFromDiagnosis();
            }
            return _diagnosisSet;
        }

        private void FindDiagnosisFromConflicts()
        {
            //hittingSet should be List<HashSet<CompSet>> but my hittingSet algoritm not yet support it
            List<List<Gate>> hittingSets = SwitchingAlgorithmHittingSetFinder.FindHittingSet(_conflictSet, 10, IdToGates);//null = _conflictsSet
            foreach (List<Gate> hittingSet in hittingSets)
            {
                bool isConsistent = _constraintSystemSolver.CheckConsistensy(_observation, hittingSet);//null = hittingSet (this is aviram code)
                if (isConsistent)
                {
                    AddComponentToSet(_diagnosisSet, hittingSet, true); //it is a diagnosis
                }
                else
                {
                    AddComponentToSet(_conflictSet, (GetOppositeComponenetsList(hittingSet)), false); //it is a conflict
                }
            }
        }

        private void FindConflictsFromDiagnosis()
        {
            //hittingSet should be List<HashSet<CompSet>> but my hittingSet algoritm not yet support it
            List<List<Gate>> hittingSets = SwitchingAlgorithmHittingSetFinder.FindHittingSet(_diagnosisSet, 10, IdToGates); // null = _diagnosisSet
            foreach (List<Gate> hittingSet in hittingSets)
            {
                bool isConsistent = _constraintSystemSolver.CheckConsistensy(_observation, hittingSet);// null = hittingSet
                if (!isConsistent) // it is 
                {
                    AddComponentToSet(_conflictSet, hittingSet, false); //it is a conflict
                }
                else
                {
                    AddComponentToSet(_diagnosisSet, GetOppositeComponenetsList(hittingSet), true);
                }
            }
        }

        private List<Gate> GetOppositeComponenetsList(List<Gate> compSetComponents)
        {
            List<Gate> allSystemComponents  = _observation.TheModel.Components;
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

        private void AddComponentToSet(Sets sets , List<Gate> gates , bool needToBeSatisfied)
        {
            CompSet minimizedSet = MinimizeCompSet(gates, needToBeSatisfied);
            //add to global diagnosis set while keepin minimal subset - using Trie
        }

        private CompSet MinimizeCompSet(List<Gate> gates , bool needToBeSatisfied)
        {
            //SetMinimizer.Minimize(_observation, gates, needToBeSatisfied, 10);
            return null;
        }

    }
}
