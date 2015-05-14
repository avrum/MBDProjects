using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects
{
    interface IDiagnoser
    {
        DiagnosisSet FindDiagnoses(Observation observation);
        ConflictSet FindConflicts(Observation observation);
        DiagnosisSet FindHittingSets(ConflictSet conflicts);
    }
}
