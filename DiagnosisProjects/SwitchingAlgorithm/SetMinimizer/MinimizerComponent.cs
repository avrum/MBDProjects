using System.Collections.Generic;

namespace DiagnosisProjects.SwitchingAlgorithm.SetMinimizer
{
    class MinimizerComponent
    {
        public List<Gate> ComponentsList;
        public MinimizerComponent Parent;
        public int LastChildIndex;

        public MinimizerComponent(List<Gate> componentsList)
        {
            this.ComponentsList = componentsList;
            this.LastChildIndex = -1;
        }

   
    }
}
