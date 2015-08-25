using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class ConstraintSystemSolverMock
    {
        private List<HashSet<int>> Diagnosises;
        private static ConstraintSystemSolverMock instance;

        public static ConstraintSystemSolverMock getInstance()
        {
            return instance ?? (instance = new ConstraintSystemSolverMock());
        }

        private ConstraintSystemSolverMock()
        {
            Diagnosises = new List<HashSet<int>>();
            buildDiagnosisesList();
        }

        private void buildDiagnosisesList()
        {
            FileStream fs = new FileStream(TestingEnvironment.DiagnosisFile, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            string allText = reader.ReadToEnd();
            fs.Close();
            reader.Close();
            char[] delrow = new char[2];
            delrow[0] = '\n';
            delrow[1] = '\r';
            List<string> rows = allText.Split(delrow, StringSplitOptions.RemoveEmptyEntries).ToList();
            char[] del = {' '};
            foreach (string row in rows)
            {
                string[] inputArr = row.Split(del, StringSplitOptions.RemoveEmptyEntries);
                int[] intArr = Array.ConvertAll(inputArr, int.Parse);
                HashSet<int> diagnosis = new HashSet<int>(intArr);
                Diagnosises.Add(diagnosis);
            }
        }

        public bool CheckConsistensy(Observation observation, List<Gate> gates)
        {
            HashSet<int> gatesIdSet = new HashSet<int>();
            foreach (Gate gate in gates)
            {
                gatesIdSet.Add(gate.Id);
            }

            foreach (HashSet<int> diagnosis in Diagnosises)
            {
                if (gatesIdSet.IsSupersetOf(diagnosis))
                {
                    return true;
                }
            }
            return false;
        }

        public List<HashSet<int>> GetDiagnosisSet()
        {
            return Diagnosises;
        }
    }
}
