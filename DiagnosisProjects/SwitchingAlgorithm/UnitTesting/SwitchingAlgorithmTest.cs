using System;
using System.Collections.Generic;
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

        [ClassInitialize]
        public void Initialize()
        {
            ModelObservationCreator modelObservationCreator = new ModelObservationCreator();
            _observations = modelObservationCreator.ReadObsModelFiles("777.txt", "777_iscas85.txt");
            _initialConflictSet = new ConflictSet();
            Conflict conflict = new Conflict(_observations[0].TheModel.Components);
            _initialConflictSet.Conflicts.Add(conflict);

        }

        [TestMethod]
        public void TestFindDiagnosis()
        {
            SwitchingAlgorithm switchingAlgorithm = new SwitchingAlgorithm(_observations[0], _initialConflictSet,
                null, NUM_OF_DIAGNOSIS_REQUIRED);
            switchingAlgorithm.FindDiagnosis();
            
        }

    }
}
