using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.SwitchingAlgorithm;
using DiagnosisProjects.SwitchingAlgorithm.HittingSet;
using Microsoft.SolverFoundation.Services;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    class Program
    {
        static void Main(string[] args)
        {
           
        }

        private static int Comparison(Gate gate, Gate gate1)
        {
            if (gate.Id == gate1.Id)
            {
                return 0;
            }
            if (gate.Id > gate1.Id)
            {
                return 1;
            }
            return -1;
        }

        private static string createKeyForSet(List<Gate> gates )
        {
            String key = "#";
            foreach (Gate gate in gates)
            {
                key += gate.Id;
            }
            return key;
        }

        private static void printSetList(List<HashSet<int>> hittingSets)
        {
            foreach (HashSet<int> hittingSet in hittingSets)
            {
                Console.Write("{ ");
                foreach (int i in hittingSet)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine("}");
            }
        }
    }
}
