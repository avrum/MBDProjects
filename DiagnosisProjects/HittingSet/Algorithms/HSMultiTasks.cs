using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HSMultiTasks : IHSAlgorithm
    {
        private readonly Object _expendLock = new Object();

        private Observation _observation;

        public DiagnosisSet FindHittingSets(Observation observation, ConflictSet conflicts)
        {
            this._observation = observation;
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

            int size = 1;
            int lastSize = 0;

            while (size != lastSize ||
                Process.GetCurrentProcess().Threads.OfType<ProcessThread>().Where(t => t.ThreadState == System.Diagnostics.ThreadState.Running).Count() != 1)
            {
                //for (int j = 0; j < (size - lastSize); j++)
                Parallel.For(0, size - lastSize, j =>
                {
                    HSTreeNode node = nodesToExpand[lastSize + j];

                    // Parallelize the outer loop to partition the source array by rows.
                    Parallel.For(0, node.Conflict.TheConflict.Count, i =>
                    {
                        Gate c = node.Conflict.TheConflict[i];
                        Expand(node, c, diagnosisSet, paths, conflicts, nodesToExpand);
                    }); // Parallel.For

                });

                lastSize = size;

                /*
                for (int i = 0; i < nodesToExpand.Count; i++)
                {
                    HSTreeNode node = nodesToExpand[i];
                    System.Diagnostics.Debug.WriteLine(node.ToString());
                }*/



                size = nodesToExpand.Count;
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
                    bool IsDiagnosis = ConstraintSystemSolver.Instance.CheckConsistensy(_observation, node.PathLabel.Path);

                    //If its not a diagnosis we add it as a conflict
                    if (!IsDiagnosis)
                    {
                        node.Conflict = new Conflict(node.PathLabel.Path);
                    }
                }

                lock (_expendLock)
                {

                    if (node.Conflict != null && node.Conflict.TheConflict.Count > 0)
                    {
                        // Add if not exist
                        if (!newNodes.Any(e => e.CompareTo(node)==0 ))
                        {
                            newNodes.Add(node);
                        }

                        // Add if not exist
                        if (!conflicts.Conflicts.Contains(node.Conflict))
                        {
                            conflicts.Conflicts.Add(node.Conflict);
                        }
                    }
                    else if (!HSHelper.IsSubsetOfPathDoesExitInDiagnosis(newPathLabel, diagnosisSet))
                    {
                        Diagnosis diagnosis = new Diagnosis(node.PathLabel.Path);
                        diagnosisSet.AddDiagnosis(diagnosis);
                    }

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
