using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    static class HSHelper
    {

        #region Helpers Functions

        /// <summary>
        /// Convert ConflictSet To List<HSTreeNode>
        /// </summary>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        public static List<HSTreeNode> ConvertConflictSetToNodes(ConflictSet conflicts)
        {
            List<HSTreeNode> nodes = new List<HSTreeNode>();

            foreach (Conflict con in conflicts.Conflicts)
            {
                HSTreeNode node = new HSTreeNode(con);

                nodes.Add(node);
            }

            return nodes;
        }


        /// <summary>
        /// Is Subset Of Path Does Exit In Diagnosis
        /// For example:
        /// path = {'a', 'b'}   diagnosis.TheDiagnosis = {'a','b','c'}  ===> Return true
        /// path = {'a', 'b'}   diagnosis.TheDiagnosis = {'a','c','d'}  ===> Return false
        /// </summary>
        /// <param name="newPathLabel"></param>
        /// <param name="diagnosisSet"></param>
        /// <returns></returns>
        public static bool IsSubsetOfPathDoesExitInDiagnosis(HSTreePath newPathLabel, DiagnosisSet diagnosisSet)
        {

            //Check if in any diagnosis:
            for (int j = 0; j < diagnosisSet.Diagnoses.Count; j++)
            {
                Diagnosis diagnosis = diagnosisSet.Diagnoses[j];


                List<Gate> path = newPathLabel.Path;

                //Check if path is in diagnosis:
                if(!diagnosis.TheDiagnosis.Except(path).Any())
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Is A Conflict Exist Conj With "newPathLabel" Is Empty
        /// </summary>
        /// <param name="conflicts"></param>
        /// <param name="newPathLabel"></param>
        /// <returns> Return the conflict if found. If not found return null </returns>
        public static Conflict IsAConflictExistConjWithPathLabelIsEmpty(ConflictSet conflicts, HSTreePath newPathLabel)
        {
            List<Gate> pathLabelsGates = newPathLabel.Path;

            //Check if in any conflict:
            for (int j = 0; j < conflicts.Conflicts.Count; j++)
            {
                Conflict conflict = conflicts.Conflicts[j];

                List<Gate> conflictGats = conflict.TheConflict;

                //Check if pathLabelsGates Conj (CHITOCH)  conflictGats is empty
                bool intersect = conflictGats.Intersect(pathLabelsGates).Any();
                if (!intersect)
                {
                    return conflict;
                }
            }

            return null;
        }

        #endregion
    }
}
