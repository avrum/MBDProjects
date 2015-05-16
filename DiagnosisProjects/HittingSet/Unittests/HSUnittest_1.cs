using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Unittests
{

    public class HSUnittest_1
    {
        public static void RunTest()
        {
            Gate aGate = new OneInputComponent(1, Gate.Type.and);
            Gate bGate = new OneInputComponent(2, Gate.Type.and);
            Gate cGate = new OneInputComponent(3, Gate.Type.and);
            Gate dGate = new OneInputComponent(4, Gate.Type.and);
            Gate eGate = new OneInputComponent(5, Gate.Type.and);

            Conflict con1 = new Conflict(new List<Gate> { aGate, bGate, cGate });
            Conflict con2 = new Conflict(new List<Gate> { dGate, bGate, eGate });

            ConflictSet conflictSet = new ConflictSet();
            conflictSet.Conflicts = new List<Conflict> { con1, con2 };

            HittingSetFinder.FindHittingSets(conflictSet);

        }



    }
}
