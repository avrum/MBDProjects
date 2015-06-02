using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HSOneThread : IHSAlgorithm
    {
        private Observation observation;

        public DiagnosisSet FindHittingSets(Observation observation, ConflictSet conflicts)
        {
            this.observation = observation;
            return DiagnoseMainLoop(conflicts);
        }

        #region Reiter’s HS-Tree Algorithm Functions

        /// <summary>
        /// Reiter’s HS-Tree Algorithm 1
        /// </summary>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        private DiagnosisSet DiagnoseMainLoop(ConflictSet conflicts)
        {
            DiagnosisSet diagnosisSet = new DiagnosisSet();
            List<HSTreePath> paths = new List<HSTreePath>();
            List<HSTreeNode> newNodes = new List<HSTreeNode>();

            List<HSTreeNode> nodesToExpand = HSHelper.ConvertConflictSetToNodes(conflicts);
            nodesToExpand.RemoveAt(1);

            //Not sure if this should be empty or not....
            //conflicts = new ConflictSet();
            //conflicts.Conflicts = new List<Conflict>();

            while (nodesToExpand.Count > 0)
            {
                newNodes = new List<HSTreeNode>();
                
                HSTreeNode node = nodesToExpand[0];
                nodesToExpand.RemoveAt(0);

                for (int i = 0; i < node.Conflict.TheConflict.Count ; i++)
                {
                    Gate c = node.Conflict.TheConflict[i];
                    Expand(node, c, diagnosisSet, paths, conflicts, newNodes);
                }

                nodesToExpand.AddRange(newNodes);
            }

            return diagnosisSet;
        }



        /// <summary>
        /// Reiter’s HS-Tree Algorithm 2
        /// </summary>
        /// <param name="existingNode"></param>
        /// <param name="c"></param>
        /// <param name="diagnosisSet"></param>
        /// <param name="paths"></param>
        /// <param name="conflicts"></param>
        /// <param name="newNodes"></param>
        private void Expand(HSTreeNode existingNode, Gate c,DiagnosisSet diagnosisSet, List<HSTreePath> paths, ConflictSet conflicts, List<HSTreeNode> newNodes)
        {
            HSTreePath newPathLabel = new HSTreePath(existingNode.PathLabel, c);

            if (!HSHelper.IsSubsetOfPathDoesExitInDiagnosis(newPathLabel, diagnosisSet) &&
                CheckAndAddPath(paths,newPathLabel))
            {
                HSTreeNode node = new HSTreeNode(newPathLabel);

                Conflict S = HSHelper.IsAConflictExistConjWithPathLabelIsEmpty(conflicts, newPathLabel);

                if( S!= null )
                {
                    node.Conflict = S;
                }
                else
                {
                    //consistency checker, which tests if the new node is a diagnosis or returns a minimal conflict otherwise.
                    bool IsDiagnosis = ConstraintSystemSolver.Instance.CheckConsistensy(observation, node.PathLabel.Path);
                    
                    //If its not a diagnosis we add it as a conflict
                    if (!IsDiagnosis)
                    {
                        node.Conflict = new Conflict(node.PathLabel.Path);
                    }
                }

                if (node.Conflict != null && node.Conflict.TheConflict.Count > 0)
                {
                    // Add if not exist
                    if (!newNodes.Contains(node))
                    {
                        newNodes.Add(node);
                    }
                    
                    // Add if not exist
                    if (!conflicts.Conflicts.Contains(node.Conflict))
                    {
                        conflicts.Conflicts.Add(node.Conflict);
                    }
                }
                else
                {
                    Diagnosis diagnosis = new Diagnosis(node.PathLabel.Path);
                    diagnosisSet.AddDiagnosis(diagnosis);
                }


            }

        }


        /// <summary>
        /// Reiter’s HS-Tree Algorithm 3
        /// </summary>
        /// <param name="paths">Already explored paths</param>
        /// <param name="newPathLabel">the newPathLabel to be explored</param>
        /// <returns>Flag indicating successful addition</returns>
        private bool CheckAndAddPath(List<HSTreePath> paths, HSTreePath newPathLabel)
        {
            if (!paths.Contains(newPathLabel))
            {
                paths.Add(newPathLabel);
                return true;
            }
            return false;
        }

        #endregion

    }
}
