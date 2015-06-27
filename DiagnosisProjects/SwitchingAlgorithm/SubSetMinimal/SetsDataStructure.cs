using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiagnosisProjects.SwitchingAlgorithm.SubSetMinimal
{
    class SetsDataStructure
    {
        public String Name;
        public Dictionary<HashSet<int>, CompSet> SetIdsToSet;

        public SetsDataStructure(String name)
        {
            this.Name = name;
            this.SetIdsToSet = new Dictionary<HashSet<int>, CompSet>();
        }

        public void AddSet(CompSet set)
        {
            List<Gate> gates = set.getComponents();
            HashSet<int> newSetKey = createKeyForSet(gates);
            Dictionary<HashSet<int>, CompSet>.KeyCollection setsIdsList = SetIdsToSet.Keys;
            List<HashSet<int>> setsToRmove = new List<HashSet<int>>();//List of super sets of the new set - should be removed

            foreach (HashSet<int> setKey in setsIdsList)
            {
                if (newSetKey.IsSupersetOf(setKey)) // The new Set is super set - no need keep checking
                {
                    Debug.WriteLine("New Set: [" + String.Join(",", newSetKey.ToArray()) + "] is super set of exist set: [" + String.Join(",", setKey.ToArray()) + "] - ignoring!");
                    return;
                }
                if (newSetKey.IsSubsetOf(setKey))//new set is sub set - maybe subset for more sets - keep checking
                {
                    setsToRmove.Add(setKey);
                    Debug.WriteLine("Removing set: [" + String.Join(",",setKey.ToArray()) + "] from: [" + Name + "] DataStructure (it is super set of the new set: ["+String.Join(",",newSetKey.ToArray())+"])");       
                }
            }
            Debug.WriteLine("Adding new Set: ["+ String.Join(",",newSetKey.ToArray()) +"] to: ["+Name+"] DataStructure");
            SetIdsToSet.Add(newSetKey, set);
            foreach (HashSet<int> hashSet in setsToRmove)
            {
                SetIdsToSet.Remove(hashSet);    
            }
        }

        public List<CompSet> GetCompSets()
        {
            List<CompSet> compSets = SetIdsToSet.Values.ToList();
            return compSets;

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
