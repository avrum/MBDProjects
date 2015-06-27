using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.SwitchingAlgorithm.HittingSet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiagnosisProjects.SwitchingAlgorithm.UnitTesting
{
    [TestClass]
    public class HittingSetFinderTest
    {
        [TestMethod]
        public void TestFindHittingSet()
        {
            Dictionary<int, Gate> idTGates = new Dictionary<int, Gate>();

            Gate gate1 = new MultipleInputComponent(1, Gate.Type.and);
            idTGates.Add(gate1.Id, gate1);
            Gate gate2 = new MultipleInputComponent(2, Gate.Type.or);
            idTGates.Add(gate2.Id, gate2);
            Gate gate3 = new MultipleInputComponent(3, Gate.Type.xor);
            idTGates.Add(gate3.Id, gate3);
            Gate gate4 = new MultipleInputComponent(4, Gate.Type.and);
            idTGates.Add(gate4.Id, gate4);

            List<Gate> gateList1 = new List<Gate>();
            gateList1.Add(gate1);
            gateList1.Add(gate2);
            gateList1.Add(gate3);
            Conflict conflict1 = new Conflict(gateList1);

            List<Gate> gateList2 = new List<Gate>();
            gateList2.Add(gate3);
            gateList2.Add(gate4);
            Conflict conflict2 = new Conflict(gateList2);

            List<Gate> gateList3 = new List<Gate>();
            gateList3.Add(gate2);
            gateList3.Add(gate4);
            Conflict conflict3 = new Conflict(gateList3);

            ConflictSet conflictSet = new ConflictSet();
            conflictSet.Conflicts = new List<Conflict>();
            conflictSet.Conflicts.Add(conflict1);
            conflictSet.Conflicts.Add(conflict2);
            conflictSet.Conflicts.Add(conflict3);
            List<List<Gate>> hittingSets = SwitchingAlgorithmHittingSetFinder.FindHittingSet(conflictSet, 10, idTGates);
            Assert.AreEqual(hittingSets.Count, 4);

            printSetList(hittingSets);

        }

        private static void printSetList(List<List<Gate>> hittingSets)
        {
            foreach (List<Gate> hittingSet in hittingSets)
            {
                Debug.Write("{ ");
                foreach (Gate gate in hittingSet)
                {
                    Debug.Write(gate.Id + " ");
                }
                Debug.WriteLine("}");
            }
        }
    }
}
