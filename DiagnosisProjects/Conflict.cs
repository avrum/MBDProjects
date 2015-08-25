using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects
{
    public class Conflict: CompSet
    {
        public List<Gate> TheConflict { get; private set; }

        public Conflict(List<Gate> conflict)
        {
            this.TheConflict = conflict;
        }

        public override List<Gate> getComponents()
        {
            //return base.getComponents();
            return TheConflict;
        }
    }
}
