using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Unittests
{
    class HSUnittest_2
    {
        public static void RunTest()
        {
            Gate aGate = new OneInputComponent(1, Gate.Type.and);
            Gate bGate = new OneInputComponent(2, Gate.Type.and);
            Gate cGate = new OneInputComponent(3, Gate.Type.and);
            Gate dGate = new OneInputComponent(4, Gate.Type.and);
            Gate eGate = new OneInputComponent(5, Gate.Type.and);
            Gate aGate2 = new OneInputComponent(6, Gate.Type.and);
            Gate bGate2 = new OneInputComponent(7, Gate.Type.and);
            Gate cGate2 = new OneInputComponent(8, Gate.Type.and);
            Gate dGate2 = new OneInputComponent(9, Gate.Type.and);
            Gate eGate2 = new OneInputComponent(10, Gate.Type.and);
            Gate aGate3 = new OneInputComponent(11, Gate.Type.and);
            Gate bGate3 = new OneInputComponent(12, Gate.Type.and);
            Gate cGate3 = new OneInputComponent(13, Gate.Type.and);
            Gate dGate3 = new OneInputComponent(14, Gate.Type.and);
            Gate eGate3 = new OneInputComponent(15, Gate.Type.and);

            Conflict con1 = new Conflict(new List<Gate> { aGate, bGate, cGate });
            Conflict con2 = new Conflict(new List<Gate> { dGate, bGate, eGate });
            Conflict con3 = new Conflict(new List<Gate> { aGate2, bGate2, cGate2 });
            Conflict con4 = new Conflict(new List<Gate> { dGate2, bGate2, eGate2 });
            Conflict con5 = new Conflict(new List<Gate> { aGate3, bGate3, cGate3 });
            Conflict con6 = new Conflict(new List<Gate> { dGate3, bGate3, eGate3 });
            Conflict con7 = new Conflict(new List<Gate> { dGate, bGate2, eGate3 });

            ConflictSet conflictSet = new ConflictSet();
            conflictSet.Conflicts = new List<Conflict> { con1, con2, con3, con4, con5, con6, con7 };

            HittingSetFinder.FindHittingSets(null, conflictSet);
        }
    }
}
