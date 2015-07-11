using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.LTMS;

namespace DiagnosisProjects.HittingSet
{
    class HittingSetTester
    {
        private static List<HittingSetStatistics> _hittingSetStatisticses;

        public static void RunAllTest()
        {
            _hittingSetStatisticses = new List<HittingSetStatistics>();

            RunAllTest("74181.sys", "74181_iscas85.obs");
            // RunAllTest("74182.sys", "74182_iscas85.obs");
            //RunAllTest("74283.sys", "74283_iscas85.obs");
            //RunAllTest("c1355.sys", "c1355_iscas85.obs"); - NOTWORKING
            //RunAllTest("c17.sys", "c17_iscas85.obs");
            //RunAllTest("c1908.sys", "c1908_iscas85.obs"); - NOTWORKING
            // RunAllTest("c2670.sys", "c2670_iscas85.obs");  - NOTWORKING
            //  RunAllTest("c3540.sys", "c3540_iscas85.obs"); - NOTWORKING
            //  RunAllTest("c432.sys", "c432_iscas85.obs"); - NOTWORKING
            // RunAllTest("c499.sys", "c499_iscas85.obs"); - NOTWORKING
            //RunAllTest("c5315.sys", "c5315_iscas85.obs"); - NOTWORKING
            //RunAllTest("c6288.sys", "c6288_iscas85.obs"); - NOTWORKING
            //RunAllTest("c7552.sys", "c7552_iscas85.obs"); - NOTWORKING
            //RunAllTest("c880.sys", "c880_iscas85.obs"); - NOTWORKING
        }

        private static void RunAllTest(string fileModel, string fileObs)
        {
            fileModel = "SystemFiles/" + fileModel;
            fileObs = "SystemFiles/" + fileObs;

            ModelObservationCreator cc = new ModelObservationCreator();
            List<Observation> allObservetions = cc.ReadObsModelFiles(fileModel, fileObs);

            int index = 1;
            foreach (Observation observation in allObservetions)
            {
                if (index > 50)
                {
                    break;
                }
                Debug.WriteLine("****************************************************************** Obs number = " + index + "   CAL CONFLICATES");
                //Get Conflict Set
                LtmsAlgorithm ltmsAlgorithm = new LtmsAlgorithm(observation.TheModel, observation);
                List<List<Gate>> gatelList = ltmsAlgorithm.findConflicts();
                ConflictSet conflictSet = ltmsAlgorithm.ConvertGateListToConflict(gatelList);

                Debug.WriteLine("****************************************************************** Obs number = " + index + "   Number Of Conflicts=" + conflictSet.Conflicts.Count);
                index ++;

                
                ConstraintSystemSolver.Reset();

                Stopwatch sw = Stopwatch.StartNew();
                HSOneThread s1 = new HSOneThread();
                DiagnosisSet diagnosisSet1 = s1.FindHittingSets(observation, conflictSet);
                sw.Stop();
                HittingSetStatistics statistics = new HittingSetStatistics(observation.TheModel.Id, observation.Id, conflictSet.Conflicts.Count, sw.Elapsed.TotalMilliseconds, "HSOneThread", diagnosisSet1.Count);
                _hittingSetStatisticses.Add(statistics);

                ConstraintSystemSolver.Reset();
                
                sw = Stopwatch.StartNew();
                HSMultiTasks s2 = new HSMultiTasks();
                DiagnosisSet diagnosisSet2 = s2.FindHittingSets(observation, conflictSet);
                sw.Stop();
                statistics = new HittingSetStatistics(observation.TheModel.Id, observation.Id, conflictSet.Conflicts.Count, sw.Elapsed.TotalMilliseconds, "HSMultiTasks", diagnosisSet2.Count);
                _hittingSetStatisticses.Add(statistics);
                
                ConstraintSystemSolver.Reset();




                sw = Stopwatch.StartNew();
                HSMultiThreads s3 = new HSMultiThreads();
                DiagnosisSet diagnosisSet3 = s3.FindHittingSets(observation, conflictSet);
                sw.Stop();
                statistics = new HittingSetStatistics(observation.TheModel.Id, observation.Id, conflictSet.Conflicts.Count, sw.Elapsed.TotalMilliseconds, "HSMultiThreads", diagnosisSet3.Count);
                _hittingSetStatisticses.Add(statistics);

                ConstraintSystemSolver.Reset();
                
            }

            Debug.WriteLine("ModelID, ObservationID, NumberOfConflicts, AlgorithmName, NumberOfDiagnoses, RunTime");

            foreach (HittingSetStatistics stat in _hittingSetStatisticses)
            {
                string prints = "";
                prints += stat.ModelID + " ,";
                prints += stat.ObservationID + " ,";
                prints += stat.NumberOfConflicts + " ,";
                prints += stat.AlgorithmName + " ,";
                prints += stat.NumberOfDiagnoses + " ,";
                prints += stat.RunTime;

                Debug.WriteLine(prints);
            }

        }



    }
}
