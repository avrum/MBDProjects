using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.SwitchingAlgorithm
{
    class MinimizerComponent
    {
        public List<int> ComponentsList;
        public MinimizerComponent Parent;
        public int lastChildIndex;

        public MinimizerComponent(List<int> componentsList)
        {
            this.ComponentsList = componentsList;
            this.lastChildIndex = -1;
        }

   
    }
}
