﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;

namespace DiagnosisProjects
{
    public class OneInputComponent: Gate
    {
        private Wire input1;
        public Wire Input1
        {
            get
            {
                return input1;
            }
            set
            {
                input1 = value;
                input1.AddOutputComponent(this);
            }
        }
        public OneInputComponent(int id, Type type)
        {
            this.Id = id;
            this.type = type;
        }

        public override List<Wire> getInput() {
            List<Wire> i = new List<Wire>();
            i.Add(input1);
            return i;
             }

        public override bool GetValue()
        {
            if (type == Type.buffer)
                return Input1.Value;
            else if (type == Type.not)
                return !Input1.Value;
            return false;
        }

        public override void AddConstaint()
        {
            ConstraintSystem solver = ConstraintSystemSolver.Instance.Solver;
            CspTerm constraint = null;

            CspTerm inputTerm = Input1.CspTerm;
            CspTerm outputTerm = Output.CspTerm;


            Type consType = type;
            if (IsNotHealthy)
            {
                // In case the gate is Broken (Not Healthy) - we don't want to add any constraint!!!
                return;

                /*
                switch (type)
                {
                    case Type.buffer:
                        consType = Type.not;
                        break;
                    case Type.not:
                        consType = Type.buffer;
                        break;
                }
                */
            }

            lock (ConstraintSystemSolver.Instance.Locker)
            {
                //Debug.WriteLine("SAT IN!");
                switch (consType)
                {
                    case Type.buffer:
                        constraint = solver.Equal(inputTerm, outputTerm);
                        break;
                    case Type.not:
                        constraint = solver.Equal(inputTerm, solver.Not(outputTerm));
                        break;
                }

                solver.AddConstraints(constraint);
                //Debug.WriteLine("SAT OUT!");
            }
        }
    }
}
