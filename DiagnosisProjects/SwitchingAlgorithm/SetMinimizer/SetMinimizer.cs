using System.Collections.Generic;

namespace DiagnosisProjects.SwitchingAlgorithm.SetMinimizer
{
    public class SetMinimizer
    {
        // This function get some compSet (diagnosis or conflict) and returns the most minizmize set it can fiund in 'maxSteps' steps.
        // The parameter 'needToBeSatisfied' is to recognize between the search for conflict or for diagnosis: 
        //                          >> for searching diagnosis it will be true and for conflict it will be false
        public static List<Gate> Minimize(Observation observation, List<Gate> compSetComponents, bool needToBeSatisfied, int maxSteps)
        {
            var bestMinimizerComponent = new MinimizerComponent(compSetComponents); 
            var current = new MinimizerComponent(compSetComponents) {Parent = null};
            var numOfSteps = 0;
            //List<Gate> oppositeSet = null;

            while ((current != null) && (numOfSteps < maxSteps) && (bestMinimizerComponent.ComponentsList.Count > 1))
            {
                MinimizerComponent childMinimizerComponent = null;
                numOfSteps++;
                for (var i = current.LastChildIndex+1; i < current.ComponentsList.Count; i++)
                {
                    var component = current.ComponentsList[i];
                    var currentComponents = new List<Gate>(current.ComponentsList);
                    currentComponents.Remove(component);
                   // var setForSatSolver = AssignSetForSatSolver(observation, needToBeSatisfied, currentComponents, ref oppositeSet);
                    var setForSatSolver = needToBeSatisfied ? currentComponents : SwitchingAlgorithm.GetOppositeComponenetsList(observation.TheModel.Components, currentComponents);
                    var isSatisfied = SwitchingAlgorithm.Solver.CheckConsistensy(observation, setForSatSolver);
                    if (isSatisfied != needToBeSatisfied) continue;
                    childMinimizerComponent = new MinimizerComponent(currentComponents) {Parent = current};
                    if (childMinimizerComponent.ComponentsList.Count < bestMinimizerComponent.ComponentsList.Count)
                    {
                        bestMinimizerComponent = childMinimizerComponent;
                    }
                    current.LastChildIndex = i ;
                    break;
                }

                current = childMinimizerComponent ?? current.Parent;
            }
            return bestMinimizerComponent.ComponentsList;
        }

        private static List<Gate> AssignSetForSatSolver(Observation observation, bool needToBeSatisfied, List<Gate> currentComponents, ref List<Gate> oppositeSet)
        {
            List<Gate> setForSatSolver;
            if (needToBeSatisfied)
            {
                setForSatSolver = currentComponents;
            }
            else
            {
                if (oppositeSet == null)
                {
                    oppositeSet = SwitchingAlgorithm.GetOppositeComponenetsList(observation.TheModel.Components,
                        currentComponents);
                }
                setForSatSolver = oppositeSet;
            }
            return setForSatSolver;
        }
    }
}
