using DiagnosisProjects.HittingSet.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Unittests.E_MinC
{
    class E_MinCUnittest_2
    {
        public static void RunTest()
        {
            ModelObservationCreator cc = new ModelObservationCreator();
            List<Observation> a = cc.ReadObsModelFiles("111.txt", "111_obs.txt");
            Observation bObservation = a[0];

            List<Gate> allComponents = bObservation.TheModel.Components;

            // 11 , 22 , 33
            // 44 , 22 , 55

            Conflict c1 = new Conflict(new List<Gate>() { allComponents[0], allComponents[2], allComponents[3] });
            Conflict c2 = new Conflict(new List<Gate>() { allComponents[1], allComponents[2], allComponents[4] });

            ConflictSet cs = new ConflictSet();
            cs.Conflicts = new List<Conflict>() { c1, c2 };

            MicC_Diagnosis minCDiagnosis = E_MinCAlgorithm.FindMinC(cs);
            foreach (Gate g in minCDiagnosis.TheDiagnosis)
            {
                Console.WriteLine("Gate = " + g.Id);
            }
            Console.WriteLine("E_MinC Unit test 1. minc = " + minCDiagnosis.cardinality);

            int x = 0;
        }
    }
}
