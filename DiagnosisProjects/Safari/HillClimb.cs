using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.Safari
{
    class HillClimb
    {
        Trie<Diagnosis> setOfMinimalDiagnoses;
        DiagnosisTools tools;
        List<Gate> components;

        public HillClimb()
        {
            this.setOfMinimalDiagnoses = new Trie<Diagnosis>();
            this.tools = new DiagnosisTools();
        }
        public DiagnosisSet FindDiagnoses(Observation observation)
        {
            components = observation.TheModel.Components;
            safari(observation.TheModel, observation, 2, 6);
            tools.removeDuplicate();
            tools.printMinimalDiagnosis(observation.TheModel.Components);
            return tools.getMinimalDiagnoses();
        }
        /// <summary>
        ///Algorithm for computing multiple-fault diagnoses using stochstic search.
        ///Return a set of minimal diagnoses.
        /// </summary>
        /// <param name="DS">The system model that describe the system.</param>
        /// <param name="obs">The given observation.</param>
        /// <param name="M">Represent the number of returns in improve diagnosis section.</param>
        /// <param name="N">Represent the number of return time of the algorithm.</param>
        public Trie<Diagnosis> safari(SystemModel DS, Observation obs, int M, int N)
        {
            int n = 0;
            int m = 0;
            Diagnosis W = new Diagnosis();
            Diagnosis new_W = new Diagnosis();

            while(n < N)
            {  
                W = tools.RandomDiagnosis(DS,obs);
                m = 0;
                while(m < M)
                {
                    Diagnosis tmp = new Diagnosis(W.TheDiagnosis);
                    new_W = tools.ImproveDiagnosis(tmp, tools.GetRandomTerm(W));
                    if(tools.isConsistency(obs, new_W.TheDiagnosis))
                    {
                        W = new_W;
                        m = 0;
                    }
                    else
                        m++;
                }
                if (!tools.IsSubsumed(setOfMinimalDiagnoses, W))
                {
                    tools.AddToTrie(setOfMinimalDiagnoses, W);
                    tools.RemoveSubsumed(setOfMinimalDiagnoses, W);
                }
                n++;
            }
            return setOfMinimalDiagnoses;
        }

    }
}
