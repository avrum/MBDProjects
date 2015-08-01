using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosisProjects.SwitchingAlgorithm.HittingSet
{
    class SwitchingAlgorithmHittingSetFinder
    {
        private const int NumOfRetries = 20;
        private static readonly Random Rand = SwitchingAlgorithm.Rand;

        public static List<List<Gate>> FindHittingSet(List<List<Gate>> setsList, int requiredNumOfHittinSets, Dictionary<int, Gate> idToGate)
        {
            if (setsList == null || setsList.Count == 0 || requiredNumOfHittinSets <= 0)
            {
                return new List<List<Gate>>();
            }

            var hittingSets = new List<SortedSet<int>>();
            var countNumberOfNoNewHitingSetFound = 0;

            while (hittingSets.Count < requiredNumOfHittinSets && countNumberOfNoNewHitingSetFound < NumOfRetries)
            {
                var startIndexForIteration = Rand.Next(0, setsList.Count); //random choose set for start iterate
                //add first item to hitting set(from the coshen set)
                for (var index = 0; index < setsList[startIndexForIteration].Count; index++)
                {
                    var item = setsList[startIndexForIteration][index];
                    if (countNumberOfNoNewHitingSetFound >= NumOfRetries)
                    {
                        break;
                    }
                    var hittingSet = new SortedSet<int> {item.Id};
                    // add item from each set (if necessary)
                    for (int j = 0, i = (startIndexForIteration + 1)%setsList.Count;
                        j < setsList.Count;
                        i = (i + 1)%setsList.Count, j++)
                    {
                        var gatesIdsSet = GetGateIdsHashSet(setsList[i]);
                        if (hittingSet.Overlaps(gatesIdsSet)) continue;
                        var asArray = gatesIdsSet.ToArray();
                        var newItem = asArray[Rand.Next(asArray.Length)];
                        hittingSet.Add(newItem);
                    }

                    var isNewHittingSetFound = AddHittingSet(hittingSets, hittingSet);
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

        private static List<List<Gate>> GetGateListFromHashSet(IEnumerable<SortedSet<int>> hittingSets, Dictionary<int,Gate> idToGate)
        {
            return hittingSets.Select(hittingSet => hittingSet.Select(id => idToGate[id]).ToList()).ToList();
        }

        private static HashSet<int> GetGateIdsHashSet(IEnumerable<Gate> compSet)
        {
            var hashSet = new HashSet<int>();
            foreach (var component in compSet)
            {
                hashSet.Add(component.Id);
            }
            return hashSet;
        }

        //check if the new set is subset of another set or the opssite.
        // return true if new set added, false otherwise
        private static bool AddHittingSet(ICollection<SortedSet<int>> hittingSets, SortedSet<int> hittingSet)
        {
            if (hittingSet.Count > SwitchingAlgorithm.MaxSetSize)
            {
                return false;
            }
            var superSetToRemove = new List<SortedSet<int>>();
            foreach (var current in hittingSets)
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
                foreach (var hashSet in superSetToRemove)
                {
                    hittingSets.Remove(hashSet);
                }
            }
            hittingSets.Add(hittingSet); // new set or subset
            return true; 
        }

    }
}
