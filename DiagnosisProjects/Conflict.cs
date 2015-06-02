using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects
{
    class Conflict
    {
        public List<Gate> TheConflict { get; private set; }

        public Conflict(List<Gate> conflict)
        {
            this.TheConflict = conflict;
        }
    }
}
