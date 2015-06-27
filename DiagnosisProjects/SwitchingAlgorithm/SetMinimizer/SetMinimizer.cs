using System.Collections.Generic;

namespace DiagnosisProjects.SwitchingAlgorithm.SetMinimizer
{
    class SetMinimizer
    {
        // this function get some compSet (diagnosis or conflict) and returns the most minizmize set it can fiund in 'maxSteps' steps.
        // The parameter 'needToBeSatisfied' is to recognize between the search for conflict or for diagnosis: 
        //                          >> for searching diagnosis it will be true and for conflict it will be false
        public static List<Gate> Minimize(Observation observation, List<Gate> compSetComponents , bool needToBeSatisfied, int maxSteps)
        {
            //List<int> bestMinimizedSetFound = compSet.getComponents();
            MinimizerComponent bestMinimizerComponent = new MinimizerComponent(compSetComponents); 
            MinimizerComponent current = new MinimizerComponent(compSetComponents);
            current.Parent = null;
            int numOfSteps = 0;

            while ((current != null) && (numOfSteps < maxSteps) && (bestMinimizerComponent.ComponentsList.Count > 1))
            {
                MinimizerComponent childMinimizerComponent = null;
                //bool isSmallerSetFound = false;
                numOfSteps++;
                //foreach (int component in current.ComponentsList)
                for (int i = current.LastChildIndex+1; i < current.ComponentsList.Count; i++)
                {
                    Gate component = current.ComponentsList[i];
                    List<Gate> currentComponents = new List<Gate>(current.ComponentsList);
                    currentComponents.Remove(component);
                    /*for debugging*/
                   // Random rand = new Random();
                    bool isSatisfied = ConstraintSystemSolver.Instance.CheckConsistensy(observation, currentComponents);
                    if (isSatisfied == needToBeSatisfied)
                    {
                        childMinimizerComponent = new MinimizerComponent(currentComponents);
                        childMinimizerComponent.Parent = current;
                        if (childMinimizerComponent.ComponentsList.Count < bestMinimizerComponent.ComponentsList.Count)
                        {
                            bestMinimizerComponent = childMinimizerComponent;
                        }
                        current.LastChildIndex = i ;
                        break;
                    }

                }

                if (childMinimizerComponent != null)
                {
                    current = childMinimizerComponent;
                }
                else
                {
                    current = current.Parent;
                }
            }
          
            return bestMinimizerComponent.ComponentsList;
        }
    }
}
