﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Unittests
{
    class HSUnittest_3
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

            DiagnosisSet OUTPUT = HittingSetFinder.FindHittingSets(bObservation, cs);
        }
    }
}
