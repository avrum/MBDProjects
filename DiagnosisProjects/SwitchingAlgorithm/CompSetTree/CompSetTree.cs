using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosisProjects.SwitchingAlgorithm.CompSetTree
{
    public class CompSetTree
    {
        private Dictionary<int, List<CompSetNode>> _idToCompSetNodes;

        private readonly CompSetNode _root;
      
        public CompSetTree()
        {
            _root = new CompSetNode(0, null, null, this);    
             _idToCompSetNodes = new Dictionary<int, List<CompSetNode>>(); 
        }

        public void AddSet(List<Gate> gates)
        {
            gates.Sort(CompSetNode.GateComparison);
            RemoveSuperSets(gates);
            var isSubSetPresent = IsSubSetPresent(gates);
            if (!isSubSetPresent)
            {
                _root.AddChild(gates);
            }
        }


        //TODO - there are Needless check because we are not deleteing elements when removin them from tree(not possible)
        private void RemoveSuperSets(List<Gate> gates)
        {
            if (_idToCompSetNodes.ContainsKey(gates[0].Id))
            {
                var nodes = _idToCompSetNodes[gates[0].Id];
                var tempGatesList = new List<Gate>(gates);
                tempGatesList.RemoveAt(0); //check from second element
                foreach (var compSetNode in nodes)
                {
                    compSetNode.RemoveSuperSetsIfExist(tempGatesList.Select(gate => gate.Id).ToList());
                }
            }
        }

        private bool IsSubSetPresent(List<Gate> gates)
        {
            var isSubSetPresent = false;
            
            var tempGates = new List<Gate>(gates);
            for (var i = 0; i < gates.Count - 1; i++)
            {
                tempGates.RemoveAt(0);
                isSubSetPresent = _root.IsContainSubSetOf(tempGates);
                if (isSubSetPresent)
                {
                    break;
                }
            }
            return isSubSetPresent;
        }

        public List<List<int>> GetAllCompsSets()
        {
            return _root.GetBranchs();
        }

        public void PrintTree()
        {
            _root.PrintNode();
        }

        public List<List<Gate>> GetCompSets()
        {
            List<List<Gate>> result = new List<List<Gate>>();
            foreach (var compSet in GetAllCompsSets())
            {
                List<Gate> gates = new List<Gate>();
                foreach (var i in compSet)
                {
                    Gate gate = SwitchingAlgorithm.IdToGates[i];
                    gates.Add(gate);
                }
                result.Add(gates);
            }
            return result;
        }

        public void AddChildToAllCompSetDictionary(CompSetNode newChild)
        {
            var newChildId = newChild.Id;
            if (!_idToCompSetNodes.ContainsKey(newChildId))
            {
                var compSetNodes = new List<CompSetNode>();
                _idToCompSetNodes.Add(newChildId, compSetNodes);
            }
            _idToCompSetNodes[newChildId].Add(newChild);
        }
    }
}
