using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects
{
    public class DiagnosisSet :Sets
    {
        public List<Diagnosis> Diagnoses { get; private set; }
        public double SetProbability { get; set; }
        public int Count
        {
            get
            {
                if (Diagnoses == null)
                    return 0;
                else
                    return Diagnoses.Count;
            }
        }

        public DiagnosisSet()
        {
            Diagnoses = new List<Diagnosis>();
            SetProbability = 0;
        }

        public void AddDiagnosis(Diagnosis diagnosis)
        {
            if (diagnosis != null && diagnosis.TheDiagnosis != null && diagnosis.TheDiagnosis.Count != 0)
            {
                Diagnoses.Add(diagnosis);
                if (diagnosis.Probability == 0)
                    diagnosis.CalcAndSetProb();
                SetProbability += diagnosis.Probability;
            }
        }

        public override List<CompSet> getSets()
        {
            List<CompSet> ans = base.getSets();
            foreach (Diagnosis diagnosis in Diagnoses)
            {
                ans.Add(diagnosis);
            }
            return ans;
        }
    }
}
