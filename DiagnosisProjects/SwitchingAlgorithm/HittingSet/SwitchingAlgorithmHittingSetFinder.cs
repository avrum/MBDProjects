using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosisProjects.SwitchingAlgorithm.HittingSet
{
    class SwitchingAlgorithmHittingSetFinder
    {
        private const int NumOfRetries = 10;

        public static List<List<Gate>> FindHittingSet(Sets sets, int requiredNumOfHittinSets,
            Dictionary<int, Gate> idToGate)
        {
            List<CompSet> setsList = sets.getSets();

            if (setsList == null || setsList.Count == 0 || requiredNumOfHittinSets <= 0)
            {
                return new List<List<Gate>>();
            }
            List<HashSet<int>> hittingSets = new List<HashSet<int>>();
            int countNumberOfNoNewHitingSetFound = 0;
            Random rand = new Random();
            while (hittingSets.Count < requiredNumOfHittinSets && countNumberOfNoNewHitingSetFound < NumOfRetries)
            {
                int startIndexForIteration = rand.Next(0, setsList.Count);
                //add first item to hitting set - item from first CompSet(setsList[0])
                foreach (Gate item in setsList[startIndexForIteration].getComponents())
                {
                    if (countNumberOfNoNewHitingSetFound >= NumOfRetries)
                    {
                        break;
                    }
                    HashSet<int> hittingSet = new HashSet<int>();
                    hittingSet.Add(item.Id);
                    // add item from each set (if necessary)
                    for (int j=0, i = (startIndexForIteration + 1) % setsList.Count; j < setsList.Count; i = (i + 1) % setsList.Count, j++)
                    {
                        HashSet<int> gatesIdsSet = GetGateIdsHashSet(setsList[i]);
                        if (!hittingSet.Overlaps(gatesIdsSet))
                        {
                            int[] asArray = gatesIdsSet.ToArray();
                            int newItem = asArray[rand.Next(asArray.Length)];
                            hittingSet.Add(newItem);
                        }
                    }

                    bool isNewHittingSetFound = AddHittingSet(hittingSets, hittingSet);
                    if (!isNewHittingSetFound)
                    {
                        countNumberOfNoNewHitingSetFound ++;
                    }
                    else
                    {
                        countNumberOfNoNewHitingSetFound = 0;
                    }
                }

            }
            return GetGateListFromHashSet(hittingSets, idToGate);
        }

        private static List<List<Gate>> GetGateListFromHashSet(List<HashSet<int>> hittingSets, Dictionary<int,Gate> idToGate)
        {
            List<List<Gate>> gateList = new List<List<Gate>>();
            foreach (HashSet<int> hittingSet in hittingSets)
            {
                List<Gate> gates = new List<Gate>();
                foreach (int id in hittingSet)
                {
                    Gate gate = idToGate[id];
                    gates.Add(gate);
                }
                gateList.Add(gates);
            }
            return gateList;
        }

        private static HashSet<int> GetGateIdsHashSet(CompSet compSet)
        {
            HashSet<int> hashSet = new HashSet<int>();
            foreach (Gate component in compSet.getComponents())
            {
                hashSet.Add(component.Id);
            }
            return hashSet;
        }

        //check if the new set is subset of another set or the opssite.
        // return true if new set added, false otherwise
        private static bool AddHittingSet(List<HashSet<int>> hittingSets, HashSet<int> hittingSet)
        {
            List<HashSet<int>> superSetToRemove = new List<HashSet<int>>();
            foreach (HashSet<int> current in hittingSets)
            {
                if (hittingSet.SetEquals(current) || hittingSet.IsSupersetOf(current)) //not need to add
                {
                    return false;
                }
                if (hittingSet.IsSubsetOf(current))
                {
                    superSetToRemove.Add(current);
                }
            }

            if (superSetToRemove.Count != 0) //remove super sets
            {
                foreach (HashSet<int> hashSet in superSetToRemove)
                {
                    hittingSets.Remove(hashSet);
                }
            }
            hittingSets.Add(hittingSet); // new set or subset
            return true; 
        }

    }
}
