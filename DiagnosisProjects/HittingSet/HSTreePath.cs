using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HSTreePath : IComparable<HSTreePath>
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

        public int CompareTo(HSTreePath other)
        {
            if(Enumerable.SequenceEqual(Path.OrderBy(t => t), other.Path.OrderBy(t => t)))
            {
                return 0;
            }
            return 1;
        }

        public new string ToString()
        {
            StringBuilder sB = new StringBuilder();
            sB.Append("Path: ");
            if (Path.Count > 0)
            {
                for (int i = 0; i < Path.Count - 1; i++)
                {
                    sB.Append(Path[i].Id + ", ");
                }
                sB.Append(Path[Path.Count - 1].Id);
            }

            return sB.ToString();
        }


    }
}
