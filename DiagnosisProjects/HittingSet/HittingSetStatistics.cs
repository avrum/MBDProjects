using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.HittingSet
{
    class HittingSetStatistics
    {
        public string ModelID;
        public int ObservationID;
        public int NumberOfConflicts;
        public double RunTime;
        public string AlgorithmName;
        public int NumberOfDiagnoses;


        public HittingSetStatistics(string ModelID, int ObservationID, int NumberOfConflicts, double RunTime, string AlgorithmName, int NumberOfDiagnoses)
        {
            this.ModelID = ModelID;
            this.ObservationID = ObservationID;
            this.NumberOfConflicts = NumberOfConflicts;
            this.RunTime = RunTime;
            this.AlgorithmName = AlgorithmName;
            this.NumberOfDiagnoses = NumberOfDiagnoses;
        }

    }
}
