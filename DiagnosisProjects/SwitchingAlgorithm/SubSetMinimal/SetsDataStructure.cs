using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiagnosisProjects.SwitchingAlgorithm.SubSetMinimal
{
    class SetsDataStructure
    {
        public String Name;
        public Dictionary<HashSet<int>, List<Gate>> SetIdsToSet;

        public SetsDataStructure(String name)
        {
            this.Name = name;
            this.SetIdsToSet = new Dictionary<HashSet<int>, List<Gate>>();
        }

        //This method add the new set to the data structure while saving minimal subset
        // If the set is super set of another set it won't be added
        // If the set is subset of some other set - this other sets will be removed and this new set will be added
        public void AddSet(List<Gate> set)
        {
            HashSet<int> newSetKey = createKeyForSet(set);
            List<HashSet<int>> setsIdsList = SetIdsToSet.Keys.ToList();
            List<HashSet<int>> setsToRmove = new List<HashSet<int>>();//List of super sets of the new set - should be removed

            foreach (HashSet<int> setId in setsIdsList)
            {
                if (newSetKey.IsSupersetOf(setId)) // The new Set is super set - no need keep checking
                {
                    Debug.WriteLine("New Set: [" + String.Join(",", newSetKey.ToArray()) + "] is super set of exist set: [" + String.Join(",", setId.ToArray()) + "] - ignoring!");
                    return;
                }
                if (newSetKey.IsSubsetOf(setId))//new set is sub set - maybe subset for more sets - keep checking
                {
                    setsToRmove.Add(setId);
                    Debug.WriteLine("Removing set: [" + String.Join(",",setId.ToArray()) + "] from: [" + Name + "] DataStructure (it is super set of the new set: ["+String.Join(",",newSetKey.ToArray())+"])");       
                }
            }
            Debug.WriteLine("Adding new Set: ["+ String.Join(",",newSetKey.ToArray()) +"] to: ["+Name+"] DataStructure");
            SetIdsToSet.Add(newSetKey, set);
            foreach (HashSet<int> hashSet in setsToRmove)
            {
                SetIdsToSet.Remove(hashSet);    
            }
        }

        public List<List<Gate>> GetCompSets()
        {
           return SetIdsToSet.Values.ToList();  
        }

        private  HashSet<int> createKeyForSet(List<Gate> gates)
        {
            HashSet<int> hashSet = new HashSet<int>();
            foreach (Gate gate in gates)
            {
                hashSet.Add(gate.Id);
            }
            return hashSet;
        }

    }
}
