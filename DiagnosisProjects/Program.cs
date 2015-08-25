using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            ModelObservationCreator cc = new ModelObservationCreator();
            List<Observation> a = cc.ReadObsModelFiles("74181.txt", "74181_iscas85.txt");
            Safari.HillClimb run =  new Safari.HillClimb();
            run.FindDiagnoses( a[0]);
        }
    }
}
