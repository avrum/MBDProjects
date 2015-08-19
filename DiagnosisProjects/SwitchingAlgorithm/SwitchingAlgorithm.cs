using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DiagnosisProjects.SwitchingAlgorithm.HittingSet;
using DiagnosisProjects.SwitchingAlgorithm.SubSetMinimal;
using DiagnosisProjects.SwitchingAlgorithm.UnitTesting;
using Microsoft.SolverFoundation.Services;
using Timer = System.Timers.Timer;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class SwitchingAlgorithm
    {
        private readonly Observation _observation;
        private readonly int _requiredNumOfDiagnosis;

        private readonly SetsDataStructure _diagnosisesSetDataStructure;
        //private readonly CompSetTree.CompSetTree _diagnosisesSetDataStructure;
        private readonly SetsDataStructure _conflictsSetDataStructure;
        //private readonly CompSetTree.CompSetTree _conflictsSetDataStructure;

        public static Dictionary<int, Gate> IdToGates = new Dictionary<int, Gate>();

        public static ConstraintSystemSolverMock Solver = ConstraintSystemSolverMock.getInstance();
        //public static ConstraintSystemSolver Solver = ConstraintSystemSolver.Instance;
        private const int RequiredNumOfHittinSets = 10000;
        private readonly Object _lock = new Object();
        public SwitchingAlgorithm(Observation observation, ConflictSet initialConflictsSet, DiagnosisSet initialDiagnosisSet, int requiredNumOfDiagnosis)
        {
            this._observation = observation;
            this._requiredNumOfDiagnosis = requiredNumOfDiagnosis;
            this._conflictsSetDataStructure = new SetsDataStructure("Conflicts");
            //this._conflictsSetDataStructure = new CompSetTree.CompSetTree();
            if (initialConflictsSet != null)
            {
                foreach (var conflict in initialConflictsSet.Conflicts)
                {
                    _conflictsSetDataStructure.AddSet(conflict.TheConflict);
                }
            }
            this._diagnosisesSetDataStructure = new SetsDataStructure("Diagnosises");
            //this._diagnosisesSetDataStructure = new CompSetTree.CompSetTree();
            if (initialDiagnosisSet != null)
            {
                foreach (var diagnosis in initialDiagnosisSet.Diagnoses)
                {
                    _diagnosisesSetDataStructure.AddSet(diagnosis.TheDiagnosis);
                }
            }
            if (IdToGates.Count == 0)
            {
                BuildIdToGateDictionary(observation.TheModel.Components);
            }
        }

        protected void BuildIdToGateDictionary(IEnumerable<Gate> components)
        {
            foreach (Gate component in components)
            {
                IdToGates.Add(component.Id, component);
            }
        }


        private static bool _isTimeOut = false;
        public static int MaxSetSize = 100;
        private const int NumOfThreads = 50;
        public static Random Rand = new Random();

        //The Main Algorithm
        public DiagnosisSet FindDiagnosis(int timeOut)
        {
            if (timeOut > 0)
            {
                var aTimer = new Timer();
                aTimer.Elapsed += OnTimedEvent;
                aTimer.Interval = timeOut;
                aTimer.Enabled = true;
            }
            var diagnosisCount = _diagnosisesSetDataStructure.GetCompSets().Count;
            var conflictsCount = _conflictsSetDataStructure.GetCompSets().Count;
            var isNewSetsFound = true;
            var steps = 0;
            while (!_isTimeOut && isNewSetsFound && (diagnosisCount < _requiredNumOfDiagnosis))
            {
                Debug.WriteLine("Step: "+steps+", Diagnosis Count: "+diagnosisCount+", Conflicts Count: "+conflictsCount);
                steps++;
                FindDiagnosisFromConflicts();
                FindConflictsFromDiagnosis();

                var newDiagnosisCount = _diagnosisesSetDataStructure.GetCompSets().Count;
                var newConflictsCount = _conflictsSetDataStructure.GetCompSets().Count;
                isNewSetsFound = (newConflictsCount != conflictsCount) || (newDiagnosisCount != diagnosisCount);
                diagnosisCount = newDiagnosisCount;
                conflictsCount = newConflictsCount;
            }
            SwitchingAlgorithmTest.PrintSetList(BuildConflictSet(), "Conflicts_"+TestingEnvironment.SystemFile);
            return BuildDiagnosisSet();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ((Timer) source).Enabled = false;
            _isTimeOut = true;
            Debug.WriteLine("Time elapsed");
        }

        private DiagnosisSet BuildDiagnosisSet()
        {
            var diagnosisSet = new DiagnosisSet();
            for (var index = 0; index < _diagnosisesSetDataStructure.GetCompSets().Count; index++)
            {
                var gates = _diagnosisesSetDataStructure.GetCompSets()[index];
                var diagnosis = new Diagnosis(gates);
                diagnosisSet.AddDiagnosis(diagnosis);
            }
            return diagnosisSet;
        }

        private ConflictSet BuildConflictSet()
        {
            var conflictSet = new ConflictSet {Conflicts = new List<Conflict>()};
            for (var index = 0; index < _conflictsSetDataStructure.GetCompSets().Count; index++)
            {
                var gates = _conflictsSetDataStructure.GetCompSets()[index];
                var conflict = new Conflict(gates);
                conflictSet.Conflicts.Add(conflict);
            }
            return conflictSet;
        }

        
        private readonly List<List<Gate>> _hittingSetsFromConflicts = new List<List<Gate>>();
        private void FindDiagnosisFromConflicts()
        {
            var hittingSets = SwitchingAlgorithmHittingSetFinder.FindHittingSet(_conflictsSetDataStructure.GetCompSets(), RequiredNumOfHittinSets, IdToGates);
            var filteredHittingSet = FilterNewHittingSsets(_hittingSetsFromConflicts, hittingSets);
            _hittingSetsFromConflicts.AddRange(filteredHittingSet);
            var numOfFilteredHittingSets = filteredHittingSet.Count;
            var taskCount = numOfFilteredHittingSets % NumOfThreads == 0 ? numOfFilteredHittingSets / NumOfThreads : (numOfFilteredHittingSets / NumOfThreads) + 1;
            var tasks = new Task[taskCount];
            var startIndex = 0;
            var endIndex = numOfFilteredHittingSets < NumOfThreads ? numOfFilteredHittingSets : NumOfThreads;
            for (var index = 0; index < taskCount; index++)
            {
                var localStartIndex = startIndex;
                var localEndIndex = endIndex;
                tasks[index] = Task.Factory.StartNew(() => AddComponentFromConflicts(filteredHittingSet, localStartIndex, localEndIndex));
                if (endIndex == numOfFilteredHittingSets)
                {
                    break;
                }
                startIndex = endIndex;
                if (endIndex + NumOfThreads > numOfFilteredHittingSets)
                {
                    endIndex = numOfFilteredHittingSets;
                }
                else
                {
                    endIndex = endIndex + NumOfThreads;
                }
            }
            Task.WaitAll(tasks);
        }

        public void AddComponentFromConflicts(List<List<Gate>> filteredHittingSet, int startIndex, int endIndex)
        {
            for (var index = startIndex; index < endIndex; index++)
            {
                var hittingSet = filteredHittingSet[index];
                var isConsistent = Solver.CheckConsistensy(_observation, hittingSet);
                if (isConsistent) //it is a diagnosis
                {
                    AddComponentToSet(_diagnosisesSetDataStructure, hittingSet, true);
                }
                else //it is a conflict
                {
                    AddComponentToSet(_conflictsSetDataStructure,
                        (GetOppositeComponenetsList(_observation.TheModel.Components, hittingSet)), false);
                }
            }
        }

        private readonly List<List<Gate>> _hittingSetsFromDiagnosis = new List<List<Gate>>();
        private void FindConflictsFromDiagnosis()
        {
            var hittingSets = SwitchingAlgorithmHittingSetFinder.FindHittingSet(_diagnosisesSetDataStructure.GetCompSets(), RequiredNumOfHittinSets, IdToGates); // null = _diagnosisSet
            var filteredHittingSet = FilterNewHittingSsets(_hittingSetsFromDiagnosis, hittingSets);
            _hittingSetsFromDiagnosis.AddRange(filteredHittingSet);
            if (filteredHittingSet == null) return;
            var numOfFilteredHittingSets = filteredHittingSet.Count;
            var taskCount = numOfFilteredHittingSets % NumOfThreads == 0 ? numOfFilteredHittingSets / NumOfThreads : (numOfFilteredHittingSets / NumOfThreads) + 1;
            var tasks = new Task[taskCount];
            var startIndex = 0;
            var endIndex = numOfFilteredHittingSets < NumOfThreads ? numOfFilteredHittingSets : NumOfThreads;
            for (var index = 0; index < taskCount; index++)
            {
                var localStartIndex = startIndex;
                var localEndIndex = endIndex;
                tasks[index] = Task.Factory.StartNew(() => AddComponentFromDiagnosis(filteredHittingSet, localStartIndex, localEndIndex));
                if (endIndex == numOfFilteredHittingSets)
                {
                    break;
                }
                startIndex = endIndex;
                if (endIndex + NumOfThreads > numOfFilteredHittingSets)
                {
                    endIndex = numOfFilteredHittingSets;
                }
                else
                {
                    endIndex = endIndex + NumOfThreads;
                }
            }
            Task.WaitAll(tasks);
        }

        public void AddComponentFromDiagnosis(List<List<Gate>> filteredHittingSet, int startIndex, int endIndex)
        {
            try
            {
                for (var index = startIndex; index < endIndex; index++)
                {
                    var hittingSet = filteredHittingSet[index];
                    var oppositeSet = GetOppositeComponenetsList(_observation.TheModel.Components, hittingSet);
                    var isConsistent = Solver.CheckConsistensy(_observation, oppositeSet);
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
            catch (Exception ex)
            {
                Debug.WriteLine("Can't Add Set :"+ex.Message);
            }
        }

        private static List<List<Gate>> FilterNewHittingSsets(IEnumerable<List<Gate>> sets, IEnumerable<List<Gate>> hittingSets)
        {
            return hittingSets.Where(hittingSet => !IsContainList(sets, hittingSet)).ToList();
        }

        private static bool IsContainList(IEnumerable<List<Gate>> allLists, IEnumerable<Gate> list)
        {
            return allLists.Select(aList => aList.SequenceEqual(list)).Any(areEqual => areEqual);
        }

        public static List<Gate> GetOppositeComponenetsList(List<Gate> allSystemComponents, List<Gate> compSetComponents)
        {
            var oppositeComponenetsList = allSystemComponents.Where(systemComponent => !compSetComponents.Contains(systemComponent)).ToList();
            oppositeComponenetsList.Sort(Comparison);
            return oppositeComponenetsList;
        }

        private void AddComponentToSet(SetsDataStructure sets, List<Gate> gates, bool needToBeSatisfied)
        {
            var minimizedSet = MinimizeCompSet(gates, needToBeSatisfied);
            //add to data structure while saving minimal subset
            if (minimizedSet.Count < MaxSetSize)
            {
                lock (_lock)
                {
                    sets.AddSet(minimizedSet);
                }
            }
        }

        private List<Gate> MinimizeCompSet(List<Gate> gates, bool needToBeSatisfied)
        {
            var minimizedGatesList = SetMinimizer.SetMinimizer.Minimize(_observation, gates, needToBeSatisfied, 100);
            return minimizedGatesList;
        }

        private static int Comparison(Gate gate, Gate gate1)
        {
            if (gate.Id == gate1.Id)
            {
                return 0;
            }
            if (gate.Id > gate1.Id)
            {
                return 1;
            }
            return -1;
        }

    }



    public static class TestingEnvironment
    {
        //public static String SystemFile = "74181.txt";
        //public static String ObservationFile = "74181_iscas85.txt";
        //public static String DiagnosisFile = "74181_1_Diag.txt";
        //public static int ObservationIndex = 0;

        public static String SystemFile = "777.txt";
        public static String ObservationFile = "777_iscas85.txt";
        public static String DiagnosisFile = "777_1_Diag.txt";
        public static int ObservationIndex = 0;
    }
}
