using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiagnosisProjects.SwitchingAlgorithm.UnitTesting
{
    [TestClass]
    public class SetMinimizerTest
    {
        private List<Observation> _observations;
        private ConflictSet _initialConflictSet;
        private int NUM_OF_DIAGNOSIS_REQUIRED = 10;

        [TestInitialize()]
        public void Initialize() {
            ModelObservationCreator modelObservationCreator = new ModelObservationCreator();
            _observations = modelObservationCreator.ReadObsModelFiles("777.txt", "777_iscas85.txt");
            _initialConflictSet = new ConflictSet();
            Conflict conflict = new Conflict(_observations[0].TheModel.Components);
            _initialConflictSet.Conflicts = new List<Conflict>(){conflict};
        }

        [TestMethod]
        public void TestMinimize()
        {
            Observation observation = _observations[0];
            List<Gate> allGates = observation.TheModel.Components;
            List<Gate> diadnosis = new List<Gate>() { allGates[1], allGates[6], allGates[2], allGates[3] };
            
            List<Gate> minimizeList = SetMinimizer.SetMinimizer.Minimize(observation, diadnosis, true, 10);
            Assert.AreEqual(minimizeList.Count, 2);
        }
    }
}
