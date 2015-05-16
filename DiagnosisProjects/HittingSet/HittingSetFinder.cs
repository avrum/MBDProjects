using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HittingSetFinder
    {
        public static DiagnosisSet FindHittingSets(ConflictSet conflicts)
        {
            IHSAlgorithm s = new HSOneThread();
            //IHSAlgorithm s = new  HSMultiThreads();
            //IHSAlgorithm s = new HSMultiTasks();

            return s.FindHittingSets(conflicts);
        }
    }
}
