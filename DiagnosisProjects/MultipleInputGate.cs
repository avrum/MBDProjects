using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    class MultipleInputComponent:Gate
    {
        public List<Wire> Input;

        public void AddInput(Wire wire)
        {
            Input.Add(wire);
            wire.AddOutputComponent(this);
        }
        public MultipleInputComponent(int id, Type type)
        {
            this.Id = id;
            this.type = type;
            Input = new List<Wire>();
            //if type == not/buffer 
        }

        public override bool GetValue()
        {
            if (Input.Count == 0)
                return false;
            switch(type)
            {
                case Type.and:
                    foreach (Wire wire in Input)
                    {
                        if (wire.Value == false)
                            return false;
                    }
                    return true;
                case Type.nand:
                    foreach(Wire wire in Input)
                    {
                        if (wire.Value == false)
                            return true;
                    }
                    return false;
                case Type.nor:
                    foreach (Wire wire in Input)
                    {
                        if (wire.Value == true)
                            return false;
                    }
                    return true;
                case Type.or:
                    foreach (Wire wire in Input)
                    {
                        if (wire.Value == true)
                            return true;
                    }
                    return false;
                case Type.xor:
                    int t=0;
                    foreach (Wire wire in Input)
                    {
                        if (wire.Value == true)
                            t++;
                    }
                    if (t % 2 == 0)
                        return false;
                    return true;
            }
            return false;

        }

        public override void AddConstaint()
        {
            ConstraintSystem solver = ConstraintSystemSolver.Instance.Solver;
            CspTerm constraint = null;

            CspTerm[] inputTerms = new CspTerm[Input.Count];
            for (int i = 0; i < Input.Count; i++)
            {
                inputTerms[i] = Input[i].GetTerm();
            }

            CspTerm outputTerm = Output.GetTerm();

            Type consType = type;
            if (IsBroken)
            {
                switch (type)
                {
                    case Type.and:
                        consType = Type.nand;
                        break;
                    case Type.nand:
                        consType = Type.and;
                        break;
                    case Type.or:
                        consType = Type.nor;
                        break;
                    case Type.nor:
                        consType = Type.or;
                        break;

                        //TODO: XOR
                        /*
                        case Type.nor:
                        consType = Type.or;
                        break;
                         */
                }
            }

            switch (consType)
            {
                case Type.and:
                    CspTerm allAndInputs = solver.And(inputTerms);
                    constraint = solver.Equal(allAndInputs, outputTerm);
                    break;
                case Type.nand:
                    CspTerm allNandInputs = solver.And(inputTerms);
                    constraint = solver.Equal(allNandInputs, solver.Not(outputTerm));
                    break;
                case Type.nor:
                    CspTerm allNorInputs = solver.Or(inputTerms);
                    constraint = solver.Equal(allNorInputs, solver.Not(outputTerm));
                    break;
                case Type.or:
                    CspTerm allOrInputs = solver.Or(inputTerms);
                    constraint = solver.Equal(allOrInputs, outputTerm);
                    break;
                case Type.xor:
                    //TODO: FIX XOR GATE
                    CspTerm allXorInputs = solver.Or(inputTerms);
                    constraint = solver.Equal(allXorInputs, outputTerm);
                    break;
            }

            solver.AddConstraints(constraint);
        }
    }
}
