using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HittingSetFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observation"></param>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        public static DiagnosisSet FindHittingSets(Observation observation, ConflictSet conflicts)
        {
            IHSAlgorithm s = new HSOneThread();
            //IHSAlgorithm s = new  HSMultiThreads();
            //IHSAlgorithm s = new HSMultiTasks();

            return s.FindHittingSets(observation , conflicts);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observation"></param>
        /// <param name="alreadyFoundDiagnosisSet"></param>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        public static DiagnosisSet FindHittingSets(Observation observation,  DiagnosisSet alreadyFoundDiagnosisSet , ConflictSet conflicts)
        {
            DiagnosisSet fullSet = FindHittingSets(observation, conflicts);

            DiagnosisSet temp = new DiagnosisSet();

            foreach (Diagnosis diagnosis in fullSet.Diagnoses)
            {
                if (!alreadyFoundDiagnosisSet.Diagnoses.Contains(diagnosis))
                {
                   temp.AddDiagnosis(diagnosis); 
                }
            }

            return temp;
        }

    }
}
