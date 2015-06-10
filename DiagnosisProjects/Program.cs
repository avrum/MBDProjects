using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.SwitchingAlgorithm.HittingSet;
using Microsoft.SolverFoundation.Services;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<int> numberSet1 = new HashSet<int>();
            numberSet1.Add(1);
            numberSet1.Add(2);
            HashSet<int> numberSet2 = new HashSet<int>();
            numberSet2.Add(2);
            numberSet2.Add(3);
            numberSet2.Add(4);
            HashSet<int> numberSet3 = new HashSet<int>();
            numberSet3.Add(4);
            numberSet3.Add(5);

            List<HashSet<int> > setsList = new List<HashSet<int>>();
            setsList.Add(numberSet1);
            setsList.Add(numberSet2);
            setsList.Add(numberSet3);
            List<HashSet<int>> hittingSets = SwitchingAlgorithmHittingSetFinder.findHittingSet(setsList, 5);
            printSetList(hittingSets);
        }

        private static void printSetList(List<HashSet<int>> hittingSets)
        {
            foreach (HashSet<int> hittingSet in hittingSets)
            {
                Console.Write("{ ");
                foreach (int i in hittingSet)
                {
                    Console.Write(i+" ");
                }
                Console.WriteLine("}");
            }
        }
    }
}
