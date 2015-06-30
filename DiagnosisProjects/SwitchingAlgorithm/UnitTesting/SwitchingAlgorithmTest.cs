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
        private int NUM_OF_DIAGNOSIS_REQUIRED = 10;

        [TestInitialize()]
        public void Initialize() 
        {
            ModelObservationCreator modelObservationCreator = new ModelObservationCreator();
            _observations = modelObservationCreator.ReadObsModelFiles("777.txt", "777_iscas85.txt");
            _initialConflictSet = new ConflictSet();
            Conflict conflict = new Conflict(_observations[0].TheModel.Components);
            _initialConflictSet.Conflicts = new List<Conflict>(){conflict};

        }

        [TestMethod]
        public void TestFindDiagnosis()
        {
            SwitchingAlgorithm switchingAlgorithm = new SwitchingAlgorithm(_observations[0], _initialConflictSet,
                null, NUM_OF_DIAGNOSIS_REQUIRED);
            DiagnosisSet diagnosisSet = switchingAlgorithm.FindDiagnosis();
            printSetList(diagnosisSet);
            List<HashSet<int>> mockDiagnosisList= ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();
            printSetList(mockDiagnosisList);
        }

        private static void printSetList(DiagnosisSet diagnosisSet)
        {
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
