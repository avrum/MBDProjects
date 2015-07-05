﻿using System.Collections.Generic;

namespace DiagnosisProjects.SwitchingAlgorithm.SetMinimizer
{
    public class SetMinimizer
    {
        // This function get some compSet (diagnosis or conflict) and returns the most minizmize set it can fiund in 'maxSteps' steps.
        // The parameter 'needToBeSatisfied' is to recognize between the search for conflict or for diagnosis: 
        //                          >> for searching diagnosis it will be true and for conflict it will be false
        public static List<Gate> Minimize(Observation observation, List<Gate> compSetComponents , bool needToBeSatisfied, int maxSteps)
        {
            MinimizerComponent bestMinimizerComponent = new MinimizerComponent(compSetComponents); 
            MinimizerComponent current = new MinimizerComponent(compSetComponents);
            current.Parent = null;
            int numOfSteps = 0;

            while ((current != null) && (numOfSteps < maxSteps) && (bestMinimizerComponent.ComponentsList.Count > 1))
            {
                MinimizerComponent childMinimizerComponent = null;
                numOfSteps++;
                for (int i = current.LastChildIndex+1; i < current.ComponentsList.Count; i++)
                {
                    Gate component = current.ComponentsList[i];
                    List<Gate> currentComponents = new List<Gate>(current.ComponentsList);
                    currentComponents.Remove(component);
                    List<Gate> setForSatSolver = needToBeSatisfied ? currentComponents : SwitchingAlgorithm.GetOppositeComponenetsList(observation.TheModel.Components, currentComponents);
                    bool isSatisfied = SwitchingAlgorithm.Solver.CheckConsistensy(observation, setForSatSolver);
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
