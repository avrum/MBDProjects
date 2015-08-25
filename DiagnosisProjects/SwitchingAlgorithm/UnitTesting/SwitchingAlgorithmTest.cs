using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiagnosisProjects.SwitchingAlgorithm.UnitTesting
{
    [TestClass]
    public class SwitchingAlgorithmTest
    {

        private List<Observation> _observations;
        private ConflictSet _initialConflictSet;
        private const int NumOfDiagnosisRequired = 10000;

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
                null, NumOfDiagnosisRequired);
            DiagnosisSet diagnosisSet = switchingAlgorithm.FindDiagnosis(-1);
            PrintSetList(diagnosisSet, "diagnosis.txt");
            //printSetList(mockDiagnosisList);
            Assert.AreEqual(diagnosisSet.Count, mockDiagnosisList.Count);
        }

        [TestMethod]
        public void TestFindDiagnosisHaltByFirstDiagnosis()
        {
            List<HashSet<int>> mockDiagnosisList = ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();

            DiagnosisSet diagnosisSet = SwitchingDiagnosticEngine.FindDiagnosisHaltByFirstDiagnosis(_observations[TestingEnvironment.ObservationIndex], _initialConflictSet, null);

            PrintSetList(diagnosisSet, "Diagnosis_" + TestingEnvironment.SystemFile);
            
            Assert.IsTrue(diagnosisSet.Count >= 1);
        }

        [TestMethod]
        public void TestfindFirtDiagnosisHaltByQuantiy()
        {
            const int quantity = 4;
            var mockDiagnosisList = ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();

            var diagnosisSet = SwitchingDiagnosticEngine.FindDiagnosisHaltByQuantiy(_observations[TestingEnvironment.ObservationIndex], _initialConflictSet, null, quantity);

            PrintSetList(diagnosisSet, "Diagnosis_" + TestingEnvironment.SystemFile);
            Assert.IsTrue(mockDiagnosisList.Count<quantity || diagnosisSet.Count >= quantity);
        }

        [TestMethod]
        public void TestfindFirtDiagnosisHaltByTime()
        {
            const int timeOut = 20 * 60 * 1000;
            const int epsilon = timeOut/5; //error margin if timer is called in the begining of the while loop
            
            var mockDiagnosisList = ConstraintSystemSolverMock.getInstance().GetDiagnosisSet();
            var sw = new Stopwatch();
            sw.Start();
            var diagnosisSet = SwitchingDiagnosticEngine.FindDiagnosisHaltByTime(_observations[TestingEnvironment.ObservationIndex], _initialConflictSet, null, timeOut);
            sw.Stop();
            PrintSetList(diagnosisSet,"Diagnosis_"+TestingEnvironment.SystemFile);
            
            var algorithmFoundAll = diagnosisSet.Count == mockDiagnosisList.Count;

            Assert.IsTrue(sw.ElapsedMilliseconds <= (timeOut + epsilon) && (algorithmFoundAll || sw.ElapsedMilliseconds >= (timeOut)));
        }

      
        public static void PrintSetList(Sets sets, String fileName)
        {
            var compSetList = new List<CompSet>(sets.getSets());
            compSetList.Sort(Comparison);
            var builder = new StringBuilder();
            builder.Append("############ Set Total count:"+sets.getSets().Count+" ######################"+Environment.NewLine);
            Debug.WriteLine("############ Diagnosis-Set ######################");
            
            var setSize = 1;
            while (compSetList.Count != 0)
            {
                var alreayPrinted = new List<CompSet>();
                var size = setSize;
                foreach (var set in compSetList.Where(set => set.getComponents().Count == size))
                {
                    alreayPrinted.Add(set);
                    builder.Append("{ ");
                    Debug.Write("{ ");
                    foreach (var gate in set.getComponents())
                    {
                        builder.Append(gate.Id + " ");
                        Debug.Write(gate.Id + " ");
                    }
                    builder.Append("}" + Environment.NewLine);
                    Debug.WriteLine("}");
                }
                compSetList.RemoveAll(alreayPrinted.Contains);
                setSize++;
            }
            File.WriteAllText(@"C:\Users\niv_av\Documents\University\Year 4\סמסטר ח\איתור תקלות\פרויקט\FILES\Diagnosis\"+fileName, builder.ToString());
        }

        private static int Comparison(CompSet set1, CompSet set2)
        {
            var list1 = set1.getComponents();
            var list2 = set2.getComponents();
            var length = list1.Count < list2.Count ? list1.Count : list2.Count;
            for (var i = 0; i < length; i++)
            {
                if (list1[i].Id > list2[i].Id)
                {
                    return 1;
                }
                if (list1[i].Id < list2[i].Id)
                {
                    return -1;
                }
            }

            if (list1.Count == list2.Count)
            {
                return 0;
            }

            return list1.Count < list2.Count ? 1 : -1;
        }

    }
}
