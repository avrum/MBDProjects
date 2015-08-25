using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Algorithms
{
    public static class MinC_Utils
    {
        public static bool isConflictSetEmpty(ConflictSet conflicts)
        {
            if (conflicts == null)
            {
                return true;
            }
            else
            {
                List<CompSet> compList = conflicts.getSets();
                if (compList.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //returns true of the conflicts contains the empty set
        public static bool containEmptySet(ConflictSet conflicts)
        {
            if (conflicts == null)
            {
                return false;
            }
            List<CompSet> compList = conflicts.getSets();
            foreach (CompSet confilct in compList)
            {
                if (confilct.getComponents().Count == 0) return true;
            }
            return false;
        }

        //returns the occurs most frequently in conflicts
        public static Gate getMostfrequentlyComp(ConflictSet conflicts)
        {
            Gate mostFreqGate = null;
            if (conflicts == null)
            {
                return null;
            }
            List<CompSet> conflictsList = conflicts.getSets();
            int mostFreqGateOccurances = 0;
            foreach (CompSet confilct in conflictsList)
            {
                if (confilct!= null)
                {
                    List<Gate> gates = confilct.getComponents();
                    foreach (Gate gate in gates)
                    {
                        int currentGateOccurances = numOfConflictsContainsComponent(conflictsList, gate);
                        if(mostFreqGateOccurances<currentGateOccurances)
                        {
                            mostFreqGateOccurances = currentGateOccurances;
                            mostFreqGate = gate;
                        }
                    }
                }
            }
            return mostFreqGate;
        }
        
        //Creates a new conflict set with the conflicts that contain the gate
        //{ c\gate | c in conflicts }
        public static ConflictSet ConflictsMinusComponent(ConflictSet conflicts, Gate gate)
        {
            ConflictSet retValConflictSet = new ConflictSet();
            List<Conflict> retValConflictsList = new List<Conflict>();
            if (conflicts == null)
            {
                return retValConflictSet;
            }
            List<CompSet> conflictsList = conflicts.getSets();
            foreach (CompSet conflict in conflictsList)
            {
                CompSet conflictMinusGate = conflictMinstComponent(conflict, gate);
                retValConflictsList.Add((Conflict)conflictMinusGate);
            }
            retValConflictSet.Conflicts = retValConflictsList;
            return retValConflictSet;
        }

        //Creates a new conflict set with the conflicts that does not contain the gate
        //{ c in conflicts | gate not in C }
        public static ConflictSet ConflictsWithoutComponent(ConflictSet conflicts, Gate gate)
        {
            ConflictSet retValConflictSet = new ConflictSet();
            List<Conflict> retValConflictsList = new List<Conflict>();
            if (conflicts == null)
            {
                return retValConflictSet;
            }
            List<CompSet> conflictsList = conflicts.getSets();
            foreach (CompSet conflict in conflictsList)
            {
                if (conflictConatinsComponent(conflict, gate) == false)
                {
                    retValConflictsList.Add((Conflict)conflict);
                }
            }
            retValConflictSet.Conflicts = retValConflictsList;
            return retValConflictSet;
        }

        public static void DualReduce(ConflictSet conflicts)
        {

            //reduce V (conflicts)

            //reduce U (components)

            
        }

        //returns the number of conflcits (i.e. compSets) that contains gate
        private static int numOfConflictsContainsComponent(List<CompSet> conflictsList, Gate gate)
        {
            int numOfOccurance = 0;
            foreach (CompSet conflict in conflictsList)
            {
                if(conflictConatinsComponent(conflict,gate) == true)
                {
                    numOfOccurance++;
                }
            }
            return numOfOccurance;
        }

        //returns true of the conflict (i.e. Compset) contains gate, or false otherwise
        private static bool conflictConatinsComponent(CompSet conflict, Gate gate)
        {
            if(conflict == null)
            { 
                return false; 
            }
            List<Gate> gates = conflict.getComponents();
            foreach (Gate g in gates)
            {
                if (g != null && g.CompareTo(gate) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        //return the conflict
        private static CompSet conflictMinstComponent(CompSet conflict, Gate gate)
        {
            if (conflict == null || conflictConatinsComponent(conflict,gate) == false)
            {
                return conflict;
            }
            List<Gate> gates = conflict.getComponents();
            gates = removeGateFromSet(gates, gate);

            Conflict retValConflict = new Conflict(gates);

            return retValConflict;
        }

        private static List<Gate> removeGateFromSet(List<Gate> gates, Gate gate)
        {
            List<Gate> retValGates = new List<Gate>() ;
            if (gates == null)
            {
                return null;
            }
            foreach (Gate g in gates)
            {
                if (g != null && g.CompareTo(gate) != 0)
                {
                    retValGates.Add(g);
                }
            }
            return retValGates;
        }


    }
}
