using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    public class Wire
    {
        public enum WireType { i, o, z };
        public int Id { get; private set; }
        public WireType Type { get; private set; }
        private bool val;

        private CspTerm term;


        public bool Value 
        { 
            get
            {
                return val;
            }
            set
            {
                val = value;
                UpdateTerm();
                if(OutputComponents!=null&&OutputComponents.Count!=0)
                {
                    foreach(Gate comp in OutputComponents)
                        comp.SetValue();
                }
            }
        }
        public Gate InputComponent { get; set; }
            //the Component that the wire is his output 
        public List<Gate> OutputComponents { get; set; }
            //the Component that the wire is his input
        public void ChangeValue(bool value) //no propogation
        {
            val = value;
            UpdateTerm();
        }

        public Wire(int id, WireType type)
        {
            this.Id = id;
            this.Type = type;
            try
            {
                this.term = ConstraintSystemSolver.Instance.Solver.CreateBoolean(id);
            }
            catch
            {
                Debug.WriteLine("Error - Wire with the same ID {id=" +id+ "} has already been created");
                this.term = ConstraintSystemSolver.Instance.Solver.CreateBoolean();
            }
            
        }
        public void AddOutputComponent(Gate Component)
        {
            if (Component != null)
            {
                if (OutputComponents == null)
                    OutputComponents = new List<Gate>();
                OutputComponents.Add(Component);
            }
        }

        private void UpdateTerm()
        {
            if (val)
            {
                term = ConstraintSystemSolver.Instance.Solver.True;
            }
            else
            {
                term = ConstraintSystemSolver.Instance.Solver.False; 
            }
        }


        public CspTerm GetTerm()
        {
            return term;
        }
     
    }
}
