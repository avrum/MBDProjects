using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HSMultiThreads : IHSAlgorithm
    {

        public EventWaitHandle waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        private Object expendLock = new Object();

        public DiagnosisSet FindHittingSets(ConflictSet conflicts)
        {
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
                for (int j = 0; j < (size - lastSize); j++)
                {
                    HSTreeNode node = nodesToExpand[lastSize + j];

                    for (int i = 0; i < node.Conflict.TheConflict.Count; i++)
                    {
                        Gate c = node.Conflict.TheConflict[i];
                        Thread thread = new Thread(() => Expand(node, c, diagnosisSet, paths, conflicts, nodesToExpand));
                        thread.Start();
                    }

                }

                lastSize = size;

                /*
                for (int i = 0; i < nodesToExpand.Count; i++)
                {
                    HSTreeNode node = nodesToExpand[i];
                    System.Diagnostics.Debug.WriteLine(node.ToString());
                }*/

                waitEvent.WaitOne();

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
                    //TODO: CHECKCONSISTENCY......
                    //consistency checker, which tests if the new node is a diagnosis or returns a minimal conflict otherwise.
                    Conflict conFromCheckConsistensy = null;
                    node.Conflict = conFromCheckConsistensy;
                }

                lock (expendLock)
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
                        for (int i = 0; i < diagnosisSet.Diagnoses.Count; i++)
                        {
                            Diagnosis d = diagnosisSet.Diagnoses[i];

                            /// path = {'a', 'b'}   diagnosis.TheDiagnosis = {'a','b','c'}  ===> Return true
                            /// path = {'a', 'b'}   diagnosis.TheDiagnosis = {'a','c','d'}  ===> Return false
                            if (d.TheDiagnosis.Count <= newPathLabel.Path.Count)
                            {
                                continue;
                            }
                            if (d.TheDiagnosis.Except(newPathLabel.Path).Any())
                            {
                                diagnosisSet.Diagnoses.RemoveAt(i);
                                i--;
                            }

                        }
                    }

                }

            }

            waitEvent.Set(); 

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
