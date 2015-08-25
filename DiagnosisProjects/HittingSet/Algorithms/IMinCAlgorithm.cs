using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Algorithms
{
    interface IMinCAlgorithm
    {
        int FindMinC(ConflictSet conflicts); // optional - add the the hitting-set as a return value also (DiagnosisSet)
    }
}
