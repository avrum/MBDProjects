using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HSTreeNode
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

    }
}
