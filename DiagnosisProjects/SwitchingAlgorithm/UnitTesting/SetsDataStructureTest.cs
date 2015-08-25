
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
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
            diagnosiSetsDataStructure.AddSet(set1.TheDiagnosis);
            
            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 1);

            Gate gate5 = new MultipleInputComponent(5, Gate.Type.or);
            //trying to add new set
            List<Gate> gateList2 = new List<Gate>();
            gateList2.Add(gate1);
            gateList2.Add(gate5);
            gateList2.Add(gate4);

            Diagnosis set2 = new Diagnosis(gateList2);
            diagnosiSetsDataStructure.AddSet(set2.TheDiagnosis);

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 2);

            //trying to add super set
            List<Gate> gateList3 = new List<Gate>();
            gateList3.Add(gate1);
            gateList3.Add(gate5);
            gateList3.Add(gate4);
            gateList3.Add(gate2);
            gateList3.Add(gate3);

            Diagnosis set3 = new Diagnosis(gateList3);
            diagnosiSetsDataStructure.AddSet(set3.TheDiagnosis);

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 2);

            //trying to add sub set
            List<Gate> gateList4 = new List<Gate>();
            gateList4.Add(gate1);
            gateList4.Add(gate5);

            Diagnosis set4 = new Diagnosis(gateList4);
            diagnosiSetsDataStructure.AddSet(set4.TheDiagnosis);

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, 2);

            DiagnosisSet diagnosisSet = new DiagnosisSet();
            foreach (List<Gate> gates in diagnosiSetsDataStructure.GetCompSets())
            {
                diagnosisSet.AddDiagnosis(new Diagnosis(gates));;
            }

            Assert.AreEqual(diagnosiSetsDataStructure.SetIdsToSet.Count, diagnosisSet.Count);
           
        }

         [TestMethod]
        public void TestComapreSimpleDateStructureAndCompSetTree()
         {
             const int numOfSets = 10000;
             var rand = new Random();
             var diagnosises = new List<Diagnosis>();
             var id = 1;
             for (var j = 0; j < numOfSets; j++)
             {
                 var numOfGates = rand.Next(1, 100);
                 var diagnosis = new Diagnosis();
                 for (var i = 0; i < numOfGates; i++)
                 {
                     Gate gate = new MultipleInputComponent(id, Gate.Type.and);
                     diagnosis.AddCompToDiagnosis(gate);
                     id++;
                 }
                 diagnosises.Add(diagnosis);
             }

             var diagnosiSetsDataStructure = new SetsDataStructure("Diagnosis");
             var diagnosisesTree = new CompSetTree.CompSetTree();

             var sw = new Stopwatch();
             sw.Start();
             foreach (var diagnosise in diagnosises)
             {
                 diagnosiSetsDataStructure.AddSet(diagnosise.TheDiagnosis);
             }
             sw.Stop();
             var timeSpan1 = sw.Elapsed; sw.Start();
             sw.Reset();
             sw.Start();
             foreach (var diagnosise in diagnosises)
             {
                 diagnosisesTree.AddSet(diagnosise.TheDiagnosis);
             }
             sw.Stop();
             var timeSpan2 = sw.Elapsed; sw.Start();
             sw.Reset();
             var count1 = diagnosiSetsDataStructure.GetCompSets().Count;
             var count2 = diagnosisesTree.GetAllCompsSets().Count;
             Debug.WriteLine("Simple Data Structure Time: "+timeSpan1+", Comp Tree Time: "+timeSpan2);
             Assert.AreEqual(count1, count2);
             Assert.IsTrue(timeSpan2 < timeSpan1);
         }
    }


}
