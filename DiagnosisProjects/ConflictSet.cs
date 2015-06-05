using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects
{
    public class ConflictSet: Sets
    {
        public List<Conflict> Conflicts { get; set; }

        public override List<CompSet> getSets()
        {
            List<CompSet> ans = base.getSets();
            foreach (Conflict conflict in Conflicts)
            {
                ans.Add(conflict);
            }
            return ans;
        }
    }
}
