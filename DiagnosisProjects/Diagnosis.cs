using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects
{
    class Diagnosis
    {
        public List<Gate> TheDiagnosis { get; private set; }
        public double Probability { get; private set; }

        public Diagnosis()
        {
            TheDiagnosis = new List<Gate>();
            Probability = 0;
        }
        public Diagnosis(List<Gate> diagnosis)
        {
            if (diagnosis != null)
                TheDiagnosis = diagnosis;
            else
                TheDiagnosis = new List<Gate>();
            Probability = 0;
        }
        public Diagnosis(List<Gate> diagnosis, double probability):this(diagnosis)
        {
            this.Probability = probability;
        }
        public void AddCompsToDiagnosis(List<Gate> comps)
        {
            if (comps != null && comps.Count != 0)
            {
                foreach (Gate comp in comps)
                    AddCompToDiagnosis(comp);
            }
        }
        public void AddCompToDiagnosis(Gate comp)
        {
            if (comp != null && !TheDiagnosis.Contains(comp))
                TheDiagnosis.Add(comp);
        }
        public void CalcAndSetProb()
        { 
            /* double x = 1;
                 foreach (Gate g in diag)
                 {
                     x = x * g.P;
                 } diagProb.Add(x);
                 sum += x;*/
            double x = 0.01; //if we change the prob for each comp to be diff - delete the part below and put the part above
            Probability = Math.Pow(x, TheDiagnosis.Count);

        }
        public void ChangeProbability(double prob)
        {
            if (prob >= 0)
                this.Probability = prob;
        }
    }
}
