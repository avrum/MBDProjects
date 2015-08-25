using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.HittingSet;


namespace DiagnosisProjects.Safari
{
    class DiagnosisTools
    {
        protected Trie<Diagnosis> trie = new Trie<Diagnosis>();
        public DiagnosisSet possibleDiagnosis = new DiagnosisSet();
        public DiagnosisSet minimalDiagnoses = new DiagnosisSet();
        public Dictionary<int, List<int>> checkList = new Dictionary<int, List<int>>();
        public int numOfElement = 0;
       
       

        /// <summary>
        ///Accepts as input a set of components, a quarter of them randomly selects and creates a new set of all possible components (permutations).
        ///Returns the first permutation which is consistency with the SD and alpha.
        /// </summary>
        /// <param name="SD">The system model of the given system.</param>
        /// <param name="obs">The observation of the given system.</param>
        public Diagnosis RandomDiagnosis(SystemModel SD, Observation obs)
        {
            int diagnosisID = 0;
            double quantity = (Convert.ToDouble(SD.Components.Count)) * (0.10);
            int numOfFaultComp = Convert.ToInt32(quantity);
            Random random = new Random();
            List<Gate> randomComp = new List<Gate>();
            List<int> usedDiagnosis = new List<int>();

            for (int i = 0; i < numOfFaultComp; i++)
            {
                int randomNumber = random.Next(0, SD.Components.Count);
                if (!usedDiagnosis.Contains(randomNumber))
                {
                    randomComp.Add(SD.Components[randomNumber]);
                    usedDiagnosis.Add(randomNumber);
                }
                else
                    numOfFaultComp++;
            }
            IEnumerable<IList> permutate = Permutate(randomComp, randomComp.Count);
            foreach (var item in permutate)
            {
                List<Gate> temp = new List<Gate>();
                Diagnosis toAdd = new Diagnosis();
                temp = (List<Gate>)item;
                toAdd.AddCompsToDiagnosis(temp);
                possibleDiagnosis.AddDiagnosis(toAdd);
            }

            for (int i = 0; i < possibleDiagnosis.Diagnoses.Count; i++)
            {
                int rand = random.Next(0, possibleDiagnosis.Count);
                if (isConsistency(obs, possibleDiagnosis.Diagnoses[rand].TheDiagnosis))
                {
                    if (!isInTrie(possibleDiagnosis.Diagnoses[rand].TheDiagnosis))
                    {
                        diagnosisID = rand;
                        break;
                    }
                }
            }

            return possibleDiagnosis.Diagnoses[diagnosisID];
        }
        /// <summary>
        ///Accepts as input a diagnosis and random ID of one of the fault components.
        ///Change one of the faulty component to be healthy. 
        ///Returns a new diagnosis with one less component.
        /// </summary>
        /// <param name="W">The current diagnosis.</param>
        /// <param name="term">Random ID of the component that will change to be healty.</param>
        public Diagnosis ImproveDiagnosis(Diagnosis W, int term)
        {
            if (W.TheDiagnosis.Count > 1)
            {
                Diagnosis EnhancedDiagnosis = new Diagnosis();
                EnhancedDiagnosis = W;
                EnhancedDiagnosis.TheDiagnosis.RemoveAt(term);
                return EnhancedDiagnosis;
            }
            else
                return W;
        }
        /// <summary>
        ///Accepts as input a set of diagnoses and single diagnosis.
        ///Used trie data structure to alocate the diagnosis. 
        ///Returns a boolean variable that represents whether the diagnosis is part of the set.
        /// </summary>
        /// <param name="R">The data structure that alocate all the uniq diagnosis (minimal).</param>
        /// <param name="W">The current diagnosis.</param>
        public bool IsSubsumed(Trie<Diagnosis> R, Diagnosis w)
        {
            trie = R;
            return verifySubsumed(w.TheDiagnosis);
        }
        /// <summary>
        ///Accepts as input a set of diagnoses and single diagnosis.
        ///Used trie data structure to alocate the diagnosis. 
        ///Used toString function that used to represent the diagnosis as a string.
        ///Insert the single diagnosis to the trie data sturcture of all minimal diagnosis.
        /// </summary>
        /// <param name="R">The data structure that alocate all the uniq diagnosis (minimal).</param>
        /// <param name="W">The current diagnosis.</param>
        public void AddToTrie(Trie<Diagnosis> R, Diagnosis w)
        {
            if (w != null)
            {
                R.Put(diagnosisToString(w.TheDiagnosis), w);
                minimalDiagnoses.AddDiagnosis(w);
                addToCheckList(w.TheDiagnosis);
            }
        }
        /// <summary>
        ///Returns all of the minimal diagnosis which complying with consistency.
        /// </summary>
        public DiagnosisSet getMinimalDiagnoses()
        {
            Console.Write("*********************************************** ");
            Console.Write(this.minimalDiagnoses.Count);
            Console.Write("*********************************************** ");
            return this.minimalDiagnoses;
        }
        /// <summary>
        ///Accepts as input a set of diagnoses and single diagnosis.
        ///Used trie data structure to alocate the diagnosis. 
        ///Detemine whether the single diagnosis is a subset of other super set in the all set.
        ///Remove from trie all the superset of the given subset.
        /// </summary>
        /// <param name="R">The data structure that alocate all the uniq diagnosis (minimal).</param>
        /// <param name="W">The current diagnosis.</param>
        public void RemoveSubsumed(Trie<Diagnosis> R, Diagnosis w)
        {
            if (w != null)
            {
                foreach (var item in minimalDiagnoses.Diagnoses)
                {
                    List<int> tmp = new List<int>();
                    List<int> theItem = new List<int>();
                    tmp = toList(w.TheDiagnosis);
                    theItem = toList(item.TheDiagnosis);
                    if (tmp.Count < theItem.Count && theItem.Count > 0 && !clone(tmp, theItem))
                    {
                        tmp.RemoveAll(x => theItem.Contains(x));
                        if (tmp.Count == 0)
                            minimalDiagnoses.Diagnoses.Remove(item);
                    }
                }
            }
        }
        /// <summary>
        ///Auxiliary function.
        ///Used to rotate right list in "Permutate" function. 
        public static void RotateRight(IList sequence, int count)
        {
            object tmp = sequence[count - 1];
            sequence.RemoveAt(count - 1);
            sequence.Insert(0, tmp);
        }
        /// <summary>
        ///Accepts as input a generic list and the size of her.
        ///Return all the possibale permutaion from the list (the size of the return list is: count!).
        /// </summary>
        /// <param name="sequence">The list of the elements.</param>
        /// <param name="W">The count of the given list.</param>
        public static IEnumerable<IList> Permutate(IList sequence, int count)
        {
            if (count == 1) yield return sequence;
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    foreach (var perm in Permutate(sequence, count - 1))
                        yield return perm;
                    RotateRight(sequence, count);
                }
            }
        }
        /// <summary>
        /// Used for checking consistency of SD AND alpha AND W.
        /// Used Hitting Set Algorithm to verify consistency.
        /// </summary>
        /// <param name="diag">The diagnosis that will be check with the all system.</param>
        public bool isConsistency(Observation obs, List<Gate> posibleConflict)
        {
            ConstraintSystemSolver consistensyHelper = new ConstraintSystemSolver();
            return consistensyHelper.CheckConsistensy(obs, posibleConflict);
        }

        /// <summary>
        /// Return random component ID from the list of components.
        /// </summary>
        /// <param name="W">The diagnosis that contains the list of all components.</param>
        public int GetRandomTerm(Diagnosis W)
        {
            Random random = new Random();
            return random.Next(0, W.TheDiagnosis.Count);
        }
        /// <summary>
        ///Accepts as input a list of gate.
        ///Used trie data structure to alocate the diagnosis
        ///Used PerfixMatcher interface to perform the test.
        ///Represents the list as a string.
        ///Return boolean variable that determine whether the current list is in the list of list.
        /// </summary>
        /// <param name="list">The list that will check if is part of the list of list.</param>
        protected bool isInTrie(List<Gate> list)
        {
            if (list == null || list.Count == 0)
                return false;
            bool ans = true;
            if (list.Count == 1)
            {
                Gate g = list.First();
                foreach (char c in g.Id + "")
                {
                    if (!trie.Matcher.NextMatch(c))
                    {
                        ans = false;
                        break;
                    }
                }
                if (ans)
                    ans = trie.Matcher.IsExactMatch();
                while (trie.Matcher.LastMatch() != 0 && trie.Matcher.LastMatch() != 32)
                {
                    trie.Matcher.BackMatch();
                }
                return ans;
            }
            List<Gate> temp = new List<Gate>(list);
            foreach (Gate g in list)
            {
                ans = true;
                temp.RemoveAt(0);
                foreach (char c in g.Id + "")
                {
                    if (!trie.Matcher.NextMatch(c))
                    {
                        ans = false;
                        break;//
                    }
                }
                if (ans)
                {
                    if (trie.Matcher.NextMatch(' '))
                    {
                        ans = isInTrie(temp);
                        trie.Matcher.BackMatch();
                    }
                }
                while (trie.Matcher.LastMatch() != 0 && trie.Matcher.LastMatch() != 32)
                {
                    trie.Matcher.BackMatch();
                }
                if (ans)
                    break;

            }
            return ans;
        }
        /// <summary>
        /// Accepts as input a list of gate.
        /// Return a representation of the list as a string.
        /// </summary>
        /// <param name="diag">The list of gate that we want to represent.</param>
        public string diagnosisToString(List<Gate> diag)
        {
            string answer = "";
            for (int i = 0; i < diag.Count; i++)
            {
                answer += diag[i].Id;
            }
            return answer;
        }
        /// <summary>
        /// Accepts as input a list of gate.
        /// Return boolean that represent whether list is subsumed in other list.
        /// </summary>
        /// <param name="diag">The list of gate that we want to check for subsumed.</param>
        public bool verifySubsumed(List<Gate> diag)
        {
            List<int> diagnosis = new List<int>();
            foreach (var item in diag)
            {
                diagnosis.Add(item.Id);
            }

            for (int i = 0; i < checkList.Count; i++)
            {
                if (clone(diagnosis, checkList[i]))
                    return true;
                List<int> tmp = new List<int>();
                duplicate(tmp, checkList[i]);
                checkList[i].RemoveAll(x => diagnosis.Contains(x));
                if (checkList[i].Count == 0)
                {
                    checkList[i].Clear();
                    duplicate(checkList[i], tmp);
                    return true;
                }
                else
                {
                    checkList[i].Clear();
                    duplicate(checkList[i], tmp);
                }
            }
            return false;
        }
        /// <summary>
        /// Accepts as input a list of gate.
        /// Generate a list for test.
        /// </summary>
        /// <param name="diag">The list of gate that we want to add to check list.</param>
        public void addToCheckList(List<Gate> diag)
        {
            List<int> tmp = new List<int>();
            for (int i = 0; i < diag.Count; i++)
            {
                tmp.Add(diag[i].Id);
            }
            checkList[numOfElement] = tmp;
            numOfElement++;
        }
        /// <summary>
        /// Accepts as input a list of gate.
        /// Generate a list of int from the given gate list.
        /// </summary>
        /// <param name="diag">The list of gate that we want to copy.</param>
        public List<int> toList(List<Gate> diag)
        {
            List<int> tmp = new List<int>();
            for (int i = 0; i < diag.Count; i++)
            {
                tmp.Add(diag[i].Id);
            }
            return tmp;
        }
        /// <summary>
        /// Accepts as input two lists of int
        /// </summary>
        /// <param name="copy">The list of int that we want to copy to.</param>
        /// <param name="toCopy">The list of int that we want to copy from.</param>
        public List<int> duplicate(List<int> copy, List<int> toCopy)
        {
            for (int i = 0; i < toCopy.Count; i++)
            {
                copy.Add(toCopy[i]);
            }
            return copy;
        }
        /// <summary>
        /// Accepts as input two lists of gate.
        /// Verify if one list if a duplicate ofthe other.
        /// </summary>
        /// <param name="a">List of int.</param>
        /// <param name="b">List of int.</param>
        public bool clone(List<int> a, List<int> b)
        {
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < b.Count; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Accepts as input two lists of gate.
        /// Print the all result data.
        /// </summary>
        /// <param name="components">List of input components.</param>
        public void printMinimalDiagnosis(List<Gate> components)
        {
            Console.WriteLine("The minimal diagnoses are:");
            foreach (var item in minimalDiagnoses.Diagnoses)
            {
                foreach (var i in item.TheDiagnosis)
                {
                    Console.Write(i.Id + ", ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
            Console.Write("*********************************************** ");
            Console.Write(this.minimalDiagnoses.Count);
            Console.Write("*********************************************** ");
            Console.WriteLine("");
            Console.WriteLine("The system model components are:");
            foreach (var i in components)
            {
                Console.Write(i.Id + ", ");
            }
        }
        /// <summary>
        /// Remove duplicate diagnosis in the all result data.
        /// </summary>
        public void removeDuplicate()
        {
            List<string> helper = new List<string>();
            DiagnosisSet answer = new DiagnosisSet();
            foreach (var item in minimalDiagnoses.Diagnoses)
            {
                bool asf = helper.Contains(diagnosisToString(item.TheDiagnosis));
                if (!helper.Contains(diagnosisToString(item.TheDiagnosis)))
                {
                    helper.Add(diagnosisToString(item.TheDiagnosis));
                    answer.AddDiagnosis(item);
                }
            }
            minimalDiagnoses = answer;
        }
    }
}
