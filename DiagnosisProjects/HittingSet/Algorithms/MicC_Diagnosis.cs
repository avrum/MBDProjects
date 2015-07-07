using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet.Algorithms
{
    class MicC_Diagnosis
    {
        public int cardinality { get; set; }
        public List<Gate> TheDiagnosis { get; private set; }

        public MicC_Diagnosis() : base()
        {
            TheDiagnosis = new List<Gate>();
            cardinality = 0;
        }

        public MicC_Diagnosis(MicC_Diagnosis diagnosis)
            : base()
        {
            TheDiagnosis = new List<Gate>();
            foreach (Gate g in diagnosis.TheDiagnosis)
            {
                this.AddCompToDiagnosis(g);
            }
        }

        public void AddCompToDiagnosis(Gate comp)
        {
            if (comp != null && !TheDiagnosis.Contains(comp))
            {
                TheDiagnosis.Add(comp);
                cardinality++;
            }
        }
        


       

    }
}
