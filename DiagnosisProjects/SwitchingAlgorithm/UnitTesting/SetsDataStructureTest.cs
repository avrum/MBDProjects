
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DiagnosisProjects.SwitchingAlgorithm.SubSetMinimal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiagnosisProjects.SwitchingAlgorithm.UnitTesting
{
    [TestClass]
    public class SetsDataStructureTest
    {
        [TestMethod]
        public void TestAddComponentToDataStructure()
        {
            Gate gate1 = new MultipleInputComponent(1, Gate.Type.and);
            Gate gate2 = new MultipleInputComponent(2, Gate.Type.or);
            Gate gate3 = new MultipleInputComponent(3, Gate.Type.xor);
            Gate gate4 = new MultipleInputComponent(4, Gate.Type.and);

            List<Gate> gateList1 = new List<Gate>();
            gateList1.Add(gate1);
            gateList1.Add(gate4);
            gateList1.Add(gate2);
            gateList1.Add(gate3);

            Diagnosis set1 = new Diagnosis(gateList1);
            
            SetsDataStructure diagnosiSetsDataStructure = new SetsDataStructure("Diagnosis");
            diagnosiSetsDataStructure.AddSet(set1);
            
            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 1);

            Gate gate5 = new MultipleInputComponent(5, Gate.Type.or);
            //trying to add new set
            List<Gate> gateList2 = new List<Gate>();
            gateList2.Add(gate1);
            gateList2.Add(gate5);
            gateList2.Add(gate4);

            Diagnosis set2 = new Diagnosis(gateList2);
            diagnosiSetsDataStructure.AddSet(set2);

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 2);

            //trying to add super set
            List<Gate> gateList3 = new List<Gate>();
            gateList3.Add(gate1);
            gateList3.Add(gate5);
            gateList3.Add(gate4);
            gateList3.Add(gate2);
            gateList3.Add(gate3);

            Diagnosis set3 = new Diagnosis(gateList3);
            diagnosiSetsDataStructure.AddSet(set3);

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 2);

            //trying to add sub set
            List<Gate> gateList4 = new List<Gate>();
            gateList4.Add(gate1);
            gateList4.Add(gate5);

            Diagnosis set4 = new Diagnosis(gateList4);
            diagnosiSetsDataStructure.AddSet(set4);

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 2);

            DiagnosisSet diagnosisSet = new DiagnosisSet();
            foreach (CompSet compSet in diagnosiSetsDataStructure.GetCompSets())
            {
                diagnosisSet.AddDiagnosis((Diagnosis) compSet);
            }

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, diagnosisSet.Count);
           
        }
    }
}
