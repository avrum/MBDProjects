using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.HittingSet;
using Microsoft.SolverFoundation.Solvers;
using DiagnosisProjects.LTMS;

namespace DiagnosisProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            ModelObservationCreator cc = new ModelObservationCreator();
            List<Observation> a = cc.ReadObsModelFiles("777.txt", "777_iscas85.txt");
            //List<Observation> a = cc.ReadObsModelFiles("74181.txt", "74181_iscas85.txt");
            SystemModel ss = a[0].TheModel;
            foreach (Observation o in a)
            {
                LtmsAlgorithm ltms = new LtmsAlgorithm(ss, o);
                List<List<Gate>> conf = ltms.findConflicts();
            }

        }



    }
}
