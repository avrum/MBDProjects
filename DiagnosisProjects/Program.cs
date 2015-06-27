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
using ModelBasedDiagnosis;

namespace DiagnosisProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            ModelObservationCreator cc = new ModelObservationCreator();
            List<Observation> a = cc.ReadObsModelFiles("777.txt", "777_iscas85.txt");
            Observation observation = a[0];
            SystemModel model = observation.TheModel;
            List<Gate> components = model.Components;
            bool[] inputValues = observation.InputValues;
            //HashSet<int> numberSet1 = new HashSet<int>();
            //numberSet1.Add(1);
            //numberSet1.Add(2);
            //HashSet<int> numberSet2 = new HashSet<int>();
            //numberSet2.Add(2);
            //numberSet2.Add(3);
            //numberSet2.Add(4);
            //List<int> numberSet3 = new List<int>();
            //numberSet3.Add(4);
            //numberSet3.Add(5);
            //numberSet3.Add(6);
            //numberSet3.Add(7);
            //numberSet3.Add(8);
            //numberSet3.Add(9);
            //SetMinimizer.Minimize(null, numberSet3, true, 10);

            Trie<List<Gate>> trie = new Trie<List<Gate>>();
            Gate gate2 = new MultipleInputComponent(2, Gate.Type.or);
            Gate gate4 = new MultipleInputComponent(4, Gate.Type.or);
            Gate gate3 = new MultipleInputComponent(3, Gate.Type.or);
            Gate gate1 = new MultipleInputComponent(1, Gate.Type.and);
            List<Gate> set1 = new List<Gate>();
            set1.Add(gate2);
            set1.Add(gate3);
            set1.Add(gate1);
            set1.Sort(Comparison);

            List<Gate> set2 = new List<Gate>();
            set2.Add(gate4);
            set2.Add(gate2);
            set2.Add(gate3);
            set2.Add(gate1);
            set2.Sort(Comparison);
            String key1 = createKeyForSet(set1);
            String key2 = createKeyForSet(set2);
            trie.Put(key1 , set1);
            trie.Put(key2, set2);

            trie.Matcher.NextMatch('#');
            List<List<Gate> > list = trie.Matcher.GetPrefixMatches();

            List<Gate> set3 = new List<Gate>();
            set3.Add(gate4);
            set3.Add(gate2);
            set3.Sort(Comparison);
            String key3 = createKeyForSet(set3);
            trie.Matcher.ResetMatch();
            trie.Matcher.GetPrefixMatches();
            for (int i = 0; i < key3.Count(); i++)
            {
                trie.Matcher.NextMatch(key3[i]);             
            }

            List<List<Gate> > prefixList = trie.Matcher.GetPrefixMatches();
            if (prefixList.Count != 0)
            {
                foreach (List<Gate> gates in prefixList)
                {
                    trie.Remove(createKeyForSet(gates));    
                }
                
            }

            trie.Matcher.ResetMatch();
            List<List<Gate>> list2 = trie.Matcher.GetPrefixMatches();







            //List<HashSet<int>> setsList = new List<HashSet<int>>();
            //setsList.Add(numberSet1);
            //setsList.Add(numberSet2);
            //setsList.Add(numberSet3);
            //List<HashSet<int>> hittingSets = SwitchingAlgorithmHittingSetFinder.findHittingSet(setsList, 5);
            //printSetList(hittingSets);
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
