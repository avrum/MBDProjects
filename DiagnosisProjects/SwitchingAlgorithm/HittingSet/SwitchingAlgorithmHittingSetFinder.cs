using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Services;

namespace DiagnosisProjects.SwitchingAlgorithm.HittingSet
{
    class SwitchingAlgorithmHittingSetFinder
    {

        private static int NUM_OF_RETRIES = 10;

        public static List<HashSet<int>> findHittingSet(List<HashSet<int>> setsList, int requiredNumOfHittinSets)
        {
            if (setsList == null || setsList.Count == 0 || requiredNumOfHittinSets <= 0)
            {
                return new List<HashSet<int>>();
            }
            List<HashSet<int>> hittingSets = new List<HashSet<int>>();
            //bool isNewHittingSetFound = true;
            int countNumberOfNoNewHitingSetFound = 0;
         
            while (hittingSets.Count < requiredNumOfHittinSets && countNumberOfNoNewHitingSetFound < NUM_OF_RETRIES){
                //add first item to hitting set
                foreach (int item in setsList[0]){
                    HashSet<int> hittingSet = new HashSet<int>();
                    hittingSet.Add(item);
                    for (int i = 1; i < setsList.Count; i++){
                        if (!hittingSet.Overlaps(setsList[i])){
                            Random randomizer = new Random();
                            int[] asArray = setsList[i].ToArray();
                            int newItem = asArray[randomizer.Next(asArray.Length)];
                            hittingSet.Add(newItem);
                        }
                    }
                    HashSet<int> set = CheckIfSubSet(hittingSets, hittingSet);
                    if (set == null) // already exist or there is a subset of this set
                    {
                        countNumberOfNoNewHitingSetFound++;
                    }
                    else if (set.Equals(hittingSet))
                    {
                        countNumberOfNoNewHitingSetFound = 0; ;
                        hittingSets.Add(hittingSet);
                    } 
                    else
                    {
                        hittingSets.Remove(set);
                        hittingSets.Add(hittingSet);
                    }
                }
            }
            return hittingSets;
        }

        //check if the new set is subset of another set or the opssite.
        private static HashSet<int> CheckIfSubSet(List<HashSet<int>> hittingSets, HashSet<int> hittingSet)
        {
           
            foreach (HashSet<int> current in hittingSets)
            {
                if (hittingSet.SetEquals(current))
                {
                    return null; // already exist
                }
                if (hittingSet.IsSubsetOf(current))
                {
                    return current; //need to delete current
                }
                if (hittingSet.IsSupersetOf(current))
                {
                    return null; //no need to add the new set
                }
            }
            return hittingSet; // add new set - it is not superset or subset

        }

    }
}
