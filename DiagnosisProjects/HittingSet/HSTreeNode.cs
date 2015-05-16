using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HSTreeNode : IComparable<HSTreeNode>
    {

        #region Properties

        public HSTreePath PathLabel { get; set; }
        public Conflict Conflict { get; set; }

        #endregion


        #region Consructors

        public HSTreeNode(Conflict conflict)
        {
            Conflict = conflict;
            PathLabel = new HSTreePath();
        }
        
        public HSTreeNode(HSTreePath pathLabel)
        {
            PathLabel = pathLabel;
        }

        #endregion

        public int CompareTo(HSTreeNode other)
        {
            if (PathLabel.CompareTo(other.PathLabel) == 0)
            {
                //Have sane path level..
                //now check if have same Conflict

                bool eq =Enumerable.SequenceEqual(Conflict.TheConflict.OrderBy(t => t), other.Conflict.TheConflict.OrderBy(t => t));
                if(eq)
                {
                    return 0;
                }
                return 1;
            }
            return 1;
        }



        public string ToString()
        {
            StringBuilder sB = new StringBuilder();
            sB.Append("Conflict: ");
            for (int i = 0; i < Conflict.TheConflict.Count - 1; i++)
            {
                sB.Append(Conflict.TheConflict[i].Id + ", ");
            }
            sB.Append(Conflict.TheConflict[Conflict.TheConflict.Count - 1].Id);

            sB.AppendLine("\n" + PathLabel.ToString());
            sB.AppendLine();

            return sB.ToString();
        }

    }
}
