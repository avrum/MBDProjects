using System;
using System.Collections.Generic;
using System.Diagnostics;
using DiagnosisProjects.SwitchingAlgorithm.HittingSet;
using DiagnosisProjects.SwitchingAlgorithm.SubSetMinimal;
using Microsoft.SolverFoundation.Services;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class SwitchingAlgorithm
    {
        private readonly Observation _observation;
        private readonly int _requiredNumOfDiagnosis;

        private readonly SetsDataStructure _diagnosisesSetDataStructure;
        private readonly SetsDataStructure _conflictsSetDataStructure;

        public static Dictionary<int, Gate> IdToGates = new Dictionary<int, Gate>();

        public static ConstraintSystemSolverMock Solver = ConstraintSystemSolverMock.getInstance();
        //public static ConstraintSystemSolver Solver = ConstraintSystemSolver.Instance;

        public SwitchingAlgorithm(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int requiredNumOfDiagnosis)
        {
            ConflictSet conflictSet;
            this._observation = observation;
            this._requiredNumOfDiagnosis = requiredNumOfDiagnosis;
            this._conflictsSetDataStructure = new SetsDataStructure("Conflicts");
            if (initialConflictsSet != null)
            {
                foreach (Conflict conflict in initialConflictsSet.Conflicts)
                {
                    _conflictsSetDataStructure.AddSet(conflict.TheConflict);
                }
            }
            this._diagnosisesSetDataStructure = new SetsDataStructure("Diagnosises");
            if (initialDiagnosisSet != null)
            {
                foreach (Diagnosis diagnosis in initialDiagnosisSet.Diagnoses)
                {
                    _diagnosisesSetDataStructure.AddSet(diagnosis.TheDiagnosis);
                }
            }
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
        public DiagnosisSet FindDiagnosis(int timeOut)
        {
            bool isTimeOut = false;

            startTimer(timeOut, obj =>
                {
                    isTimeOut = true;
                });
            
            int diagnosisCount = _diagnosisesSetDataStructure.GetCompSets().Count;
            int conflictsCount = _conflictsSetDataStructure.GetCompSets().Count;
            bool isNewSetsFound = true;
            
            //Probably we will have few 'findDiagnosis' function with different 'while' condition (i.e - findDiagnosisByTime, findDiagnosisByCount,...)
            while (!isTimeOut && isNewSetsFound && (diagnosisCount < _requiredNumOfDiagnosis))
            {
                FindDiagnosisFromConflicts();
                FindConflictsFromDiagnosis();

                int newDiagnosisCount = _diagnosisesSetDataStructure.GetCompSets().Count;
                int newConflictsCount = _conflictsSetDataStructure.GetCompSets().Count;
                isNewSetsFound = (newConflictsCount != conflictsCount) || (newDiagnosisCount != diagnosisCount);
                diagnosisCount = newDiagnosisCount;
                conflictsCount = newConflictsCount;
            }
            return buildDiagnosisSet();
        }

        private void startTimer(int timeOut, System.Threading.TimerCallback callback)
        {
            if (timeOut > 0)
            {
                System.Threading.Timer TimerItem = new System.Threading.Timer(
                callback
                , null
                , TimeSpan.FromMilliseconds(timeOut)
                , TimeSpan.FromMilliseconds(-1));
            }
        }

        private DiagnosisSet buildDiagnosisSet()
        {
            DiagnosisSet diagnosisSet = new DiagnosisSet();
            foreach (List<Gate> gates in _diagnosisesSetDataStructure.GetCompSets())
            {
                Diagnosis diagnosis = new Diagnosis(gates);
                diagnosisSet.AddDiagnosis(diagnosis);
            }
            return diagnosisSet;
        }

        private void FindDiagnosisFromConflicts()
        {
            List<List<Gate>> hittingSets = SwitchingAlgorithmHittingSetFinder.FindHittingSet(_conflictsSetDataStructure.GetCompSets(), 1000, IdToGates);
            foreach (List<Gate> hittingSet in hittingSets)
            {
                bool isConsistent = Solver.CheckConsistensy(_observation, hittingSet);
                if (isConsistent) //it is a diagnosis
                {
                    AddComponentToSet(_diagnosisesSetDataStructure, hittingSet, true); 
                }
                else //it is a conflict
                {
                    AddComponentToSet(_conflictsSetDataStructure, (GetOppositeComponenetsList(_observation.TheModel.Components, hittingSet)), false); 
                }
            }
        }

        private void FindConflictsFromDiagnosis()
        {
            List<List<Gate>> hittingSets = SwitchingAlgorithmHittingSetFinder.FindHittingSet(_diagnosisesSetDataStructure.GetCompSets(), 1000, IdToGates); // null = _diagnosisSet
            foreach (List<Gate> hittingSet in hittingSets)
            {
                List<Gate> oppositeSet = GetOppositeComponenetsList(_observation.TheModel.Components, hittingSet);
                //bool isConsistent = Solver.CheckConsistensy(Observation, hittingSet);
                bool isConsistent = Solver.CheckConsistensy(_observation, oppositeSet);
                if (!isConsistent) //it is a conflict
                {
                    AddComponentToSet(_conflictsSetDataStructure, hittingSet, false); 
                }
                else //it is a diagnosis
                {
                    AddComponentToSet(_diagnosisesSetDataStructure, oppositeSet, true);
                }
            }
        }

        public static List<Gate> GetOppositeComponenetsList(List<Gate> allSystemComponents, List<Gate> compSetComponents)
        {
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

        private void AddComponentToSet(SetsDataStructure sets , List<Gate> gates , bool needToBeSatisfied)
        {
            List<Gate> minimizedSet = MinimizeCompSet(gates, needToBeSatisfied);
            //add to data structure while saving minimal subset
            sets.AddSet(minimizedSet);
        }

        private List<Gate> MinimizeCompSet(List<Gate> gates, bool needToBeSatisfied)
        {
            List<Gate> minimizedGatesList = SetMinimizer.SetMinimizer.Minimize(_observation, gates, needToBeSatisfied, 20);
            return minimizedGatesList;
        }

    }

    public static class TestingEnvironment
    {
        public static String SystemFile = "777.txt";
        public static String ObservationFile = "777_iscas85.txt";
        public static String DiagnosisFile = "777_1_Diag.txt";
        public static int ObservationIndex = 0;
    }
}
