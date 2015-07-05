using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiagnosisProjects.SwitchingAlgorithm.UnitTesting
{
    [TestClass]
    public class SwitchingAlgorithmTest
    {

        private List<Observation> _observations;
        private ConflictSet _initialConflictSet;
        private int NUM_OF_DIAGNOSIS_REQUIRED = 10000;

        [TestInitialize()]
        public void Initialize() 
        {
            ModelObservationCreator modelObservationCreator = new ModelObservationCreator();
            _observations = modelObservationCreator.ReadObsModelFiles(TestingEnvironment.SystemFile, TestingEnvironment.ObservationFile);
            _initialConflictSet = new ConflictSet();
            Conflict conflict = new Conflict(_observations[TestingEnvironment.ObservationIndex].TheModel.Components);
            _initialConflictSet.Conflicts = new List<Conflict>(){conflict};
        }

        [TestMethod]
        public void TestFindDiagnosis()
        {
            List<HashSet<int>> mockDiagnosisList = ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();
            SwitchingAlgorithm switchingAlgorithm = new SwitchingAlgorithm(_observations[TestingEnvironment.ObservationIndex], _initialConflictSet,
                null, NUM_OF_DIAGNOSIS_REQUIRED);
            DiagnosisSet diagnosisSet = switchingAlgorithm.FindDiagnosis(-1);
            printSetList(diagnosisSet);
            //printSetList(mockDiagnosisList);
            Assert.AreEqual(diagnosisSet.Count, mockDiagnosisList.Count);
        }

        [TestMethod]
        public void TestFindDiagnosisHaltByFirstDiagnosis()
        {
            List<HashSet<int>> mockDiagnosisList = ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();

            DiagnosisSet diagnosisSet = SwitchingDiagnosticEngine.findDiagnosisHaltByFirstDiagnosis(_observations[TestingEnvironment.ObservationIndex], _initialConflictSet, null);
            
            printSetList(diagnosisSet);
            
            Assert.IsTrue(diagnosisSet.Count >= 1);
        }

        [TestMethod]
        public void TestfindFirtDiagnosisHaltByQuantiy()
        {
            int quantity = 4;
            List<HashSet<int>> mockDiagnosisList = ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();

            DiagnosisSet diagnosisSet = SwitchingDiagnosticEngine.findDiagnosisHaltByQuantiy(_observations[TestingEnvironment.ObservationIndex], _initialConflictSet, null, quantity);

            printSetList(diagnosisSet);
            //printSetList(mockDiagnosisList);
            Assert.IsTrue(mockDiagnosisList.Count<quantity || diagnosisSet.Count >= quantity);
        }

        [TestMethod]
        public void TestfindFirtDiagnosisHaltByTime()
        {
            int timeOut = 2 * 1000;
            int epsilon = 1000;//error margin if timer is called in the begining of the while loop
            
            List<HashSet<int>> mockDiagnosisList = ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DiagnosisSet diagnosisSet = SwitchingDiagnosticEngine.findDiagnosisHaltByTime(_observations[TestingEnvironment.ObservationIndex], _initialConflictSet, null, timeOut);
            sw.Stop();
            printSetList(diagnosisSet);
            
            bool algorithmFoundAll = diagnosisSet.Count == mockDiagnosisList.Count;

            Assert.IsTrue(sw.ElapsedMilliseconds <= (timeOut + epsilon) && (algorithmFoundAll || sw.ElapsedMilliseconds>= (timeOut)));
        }

      
        private static void printSetList(DiagnosisSet diagnosisSet)
        {
            Debug.WriteLine("############ Diagnosis-Set ######################");
            foreach (Diagnosis diagnosis in diagnosisSet.Diagnoses)
            {
                Debug.Write("{ ");
                foreach (Gate gate in diagnosis.TheDiagnosis)
                {
                    Debug.Write(gate.Id + " ");
                }
                Debug.WriteLine("}");
            }
        }

        private static void printSetList(List<HashSet<int>> diagnosisList)
        {
            Debug.WriteLine("############ Diagnosis-Set(from mock) ######################");
            foreach (HashSet<int> diagnosis in diagnosisList)
            {
                Debug.Write("{ ");
                foreach (int gate in diagnosis)
                {
                    Debug.Write(gate + " ");
                }
                Debug.WriteLine("}");
            }
        }

    }
}
