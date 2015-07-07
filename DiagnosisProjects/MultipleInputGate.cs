using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    public class MultipleInputComponent:Gate
    {
        public List<Wire> Input;

        public void AddInput(Wire wire)
        {
            Input.Add(wire);
            wire.AddOutputComponent(this);
        }

        public override List<Wire> getInput()
        {
            return this.Input;
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
                case Type.nxor:
                    t=0;
                    foreach (Wire wire in Input)
                    {
                        if (wire.Value == true)
                            t++;
                    }
                    if (t % 2 == 0)
                        return true;
                    return false;
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
                inputTerms[i] = Input[i].CspTerm;
            }

            CspTerm outputTerm = Output.CspTerm;

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
                    case Type.xor:
                        consType = Type.nxor;
                        break;
                    case Type.nxor:
                        consType = Type.xor;
                        break;
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
                    //XOR is also:
                    //http://en.wikipedia.org/wiki/XOR_gate#/media/File:254px_3gate_XOR.jpg

                    CspTerm firstNand = solver.Not(solver.And(inputTerms));
                    CspTerm firstOr = solver.Or(inputTerms);
                    CspTerm secendAnd = solver.And(firstNand, firstOr);

                    constraint = solver.Equal(secendAnd, outputTerm);
                    break;
                case Type.nxor:
                    //XOR is also:
                    //http://en.wikipedia.org/wiki/XOR_gate#/media/File:254px_3gate_XOR.jpg

                    CspTerm firstNand2 = solver.Not(solver.And(inputTerms));
                    CspTerm firstOr2 = solver.Or(inputTerms);
                    CspTerm secendAnd2 = solver.And(firstNand2, firstOr2);

                    constraint = solver.Equal(secendAnd2, solver.Not(outputTerm));
                    break;
            }

            solver.AddConstraints(constraint);
        }
    }
}
