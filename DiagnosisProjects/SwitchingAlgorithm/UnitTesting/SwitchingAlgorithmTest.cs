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
            DiagnosisSet diagnosisSet = switchingAlgorithm.FindDiagnosis();
            printSetList(diagnosisSet);
            //printSetList(mockDiagnosisList);
            Assert.AreEqual(diagnosisSet.Count, mockDiagnosisList.Count);
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
