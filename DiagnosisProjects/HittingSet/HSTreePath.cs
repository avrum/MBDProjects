using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HSTreePath
    {

        #region Properties

        public List<Gate> Path { get; set; }

        #endregion


        #region Consructors

        public HSTreePath()
        {
            Path = new List<Gate>();
        }

        public HSTreePath(HSTreePath pathLabel, Gate c) : this()
        {
            List<Gate> oldList = pathLabel.Path;

            Path.Add(c);
            Path.AddRange(oldList);
        }

        #endregion


    }
}
