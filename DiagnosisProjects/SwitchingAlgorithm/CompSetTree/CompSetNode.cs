using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.SwitchingAlgorithm.CompSetTree
{
    public class CompSetNode
    {
        

        public Dictionary<int,CompSetNode> Childs;
        private readonly CompSetNode _parent;
        public readonly int Id;
        private CompSetTree _tree;

        public CompSetNode(int id, List<int> childs, CompSetNode parent, CompSetTree tree)
        {
            this._parent = parent;
            this.Childs = new Dictionary<int, CompSetNode>();
            this.Id = id;
            this._tree = tree;
            if ((childs != null) && (childs.Count > 0))
            {
                AddChild(childs);
            }
        }


        public void AddChild(List<Gate> compSet)
        {
            //compSet.Sort(GateComparison);
            AddChild(compSet.Select(gate => gate.Id).ToList());
        }

        private void AddChild(List<int> compSet )
        {
            int curentId = compSet[0];
            if (Childs.ContainsKey(curentId))
            {
                CompSetNode child = Childs[curentId];
                compSet.RemoveAt(0);
                if ((compSet.Count == 0) && (child.Childs.Count >= 0)) //it is sub set or equal - remove super sets (childs)
                {
                    child.Childs.Clear();
                    return;
                }
                if ((child.Childs.Count == 0) && (compSet.Count > 0)) // it is super set - ignore
                {
                    return;
                }
                Childs[curentId].AddChild(compSet);
            }
            else
            {
                compSet.RemoveAt(0);
                var newChild = new CompSetNode(curentId, compSet, this, _tree);
                _tree.AddChildToAllCompSetDictionary(newChild);
                Childs.Add(curentId, newChild);
            }
        }

     

        public bool IsContainSubSetOf(List<Gate> set)
        {
            return IsContainSubSetOf(set.Select(gate => gate.Id).ToList());
        }

    

        public bool IsContainSubSetOf(List<int> set)
        {
            int curentId = set[0];
            if (Childs.ContainsKey(curentId))
            {
                CompSetNode child = Childs[curentId];
                set.RemoveAt(0);
                if (child.Childs.Count == 0) // this branch is sub set(or equal) of the new set
                {
                    return true;
                }
                if (set.Count == 0) // no more elementes in new set - not subset in this branch
                {
                    return false;
                }
                return child.IsContainSubSetOf(set);
            }
            return false;
        }

        public void RemoveSuperSetsIfExist(List<int> set)
        {
            if (set.Count == 0)
            {
                RemoveSuperSet();
            }
            else
            {
                int curentId = set[0];
                if (Childs.ContainsKey(curentId))
                {
                    CompSetNode child = Childs[curentId];
                    set.RemoveAt(0);
                    child.RemoveSuperSetsIfExist(set);
                }
            }

        }

        private void RemoveSuperSet()
        {
            var currentParent = _parent;
            var currentChild = this;
            while (currentParent._parent != null && currentParent.Childs.Count == 1)
            {
                currentChild = currentParent;
                currentParent = currentParent._parent;
            }
            currentParent.Childs.Remove(currentChild.Id);
        }

        public static int GateComparison(Gate gate1, Gate gate2)
        {
            return gate1.Id - gate2.Id;
        }

        public List<List<int>> GetBranchs()
        {
            var childBranchs = new List<List<int>>();
            if (Childs.Count == 0 && this.Id != 0)
            {
                List<int> newBranch = new List<int>() {this.Id};
                childBranchs.Add(newBranch);
            }
            else
            {
                foreach (var compSetNode in Childs.Values)
                {
                    List<List<int>> branchs = compSetNode.GetBranchs();
                    childBranchs.AddRange(branchs);
                    foreach (var branch in branchs)
                    {
                        if (this.Id != 0) //not the root
                        {
                            branch.Add(this.Id);
                        }
                    }
                }
            }
            return childBranchs;
        }

        public void PrintNode()
        {
            List<List<int>> branchs = GetBranchs();
            foreach (var branch in branchs)
            {
                Debug.Write("{ ");
                foreach (var i in branch)
                {
                    Debug.Write(i+" ");
                }
                Debug.WriteLine("}");
            }
        }

  
    }
}
