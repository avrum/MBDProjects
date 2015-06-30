using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class ConstraintSystemSolverMock
    {
        private List<HashSet<int>> Diagnosises;
        private static ConstraintSystemSolverMock instance;

        public static ConstraintSystemSolverMock getInstance()
        {
            return instance ?? (instance = new ConstraintSystemSolverMock());
        }

        private ConstraintSystemSolverMock()
        {
            ModelObservationCreator cc = new ModelObservationCreator();
            List<Observation> observations = cc.ReadObsModelFiles("777.txt", "777_iscas85.txt");
            Observation observation = observations[0];
            List<Gate> allComponents = observation.TheModel.Components;
            Diagnosises = new List<HashSet<int>>();
            buildDiagnosisesList(allComponents);
        }

        private void buildDiagnosisesList(List<Gate> allComponents)
        {
            HashSet<int> diagnosis1 = new HashSet<int>() { allComponents[1].Id, allComponents[6].Id};
            HashSet<int> diagnosis2 = new HashSet<int>() { allComponents[4].Id, allComponents[6].Id };
            HashSet<int> diagnosis3 = new HashSet<int>() { allComponents[6].Id, allComponents[5].Id };
            HashSet<int> diagnosis4 = new HashSet<int>() { allComponents[0].Id, allComponents[4].Id, allComponents[7].Id };
            HashSet<int> diagnosis5 = new HashSet<int>() { allComponents[0].Id, allComponents[7].Id, allComponents[5].Id };
            HashSet<int> diagnosis6 = new HashSet<int>() { allComponents[0].Id, allComponents[1].Id, allComponents[6].Id };
            HashSet<int> diagnosis7 = new HashSet<int>() { allComponents[1].Id, allComponents[3].Id, allComponents[7].Id };
            HashSet<int> diagnosis8 = new HashSet<int>() { allComponents[3].Id, allComponents[4].Id, allComponents[7].Id };
            HashSet<int> diagnosis9 = new HashSet<int>() { allComponents[3].Id, allComponents[7].Id, allComponents[5].Id };
            Diagnosises.Add(diagnosis1);
            Diagnosises.Add(diagnosis2);
            Diagnosises.Add(diagnosis3);
            Diagnosises.Add(diagnosis4);
            Diagnosises.Add(diagnosis5);
            Diagnosises.Add(diagnosis6);
            Diagnosises.Add(diagnosis7);
            Diagnosises.Add(diagnosis8);
            Diagnosises.Add(diagnosis9);
        }

        public bool CheckConsistensy(Observation observation, List<Gate> gates)
        {
            HashSet<int> gatesIdSet = new HashSet<int>();
            foreach (Gate gate in gates)
            {
                gatesIdSet.Add(gate.Id);
            }

            foreach (HashSet<int> diagnosis in Diagnosises)
            {
                if (gatesIdSet.IsSupersetOf(diagnosis))
                {
                    return true;
                }
            }
            return false;
        }

        public List<HashSet<int>> GetDiagnosisSet()
        {
            return Diagnosises;
        }
    }
}
