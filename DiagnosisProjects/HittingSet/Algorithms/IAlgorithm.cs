using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    interface IHSAlgorithm
    {
        DiagnosisSet FindHittingSets(ConflictSet conflicts);
    }
}
