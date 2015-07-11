using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.LTMS
{
    /*
     * class LtmsAlgorithm implements LTMS algorithm. 
     * */
    class LtmsAlgorithm
    {
        private Cnf cnf;
        private SystemModel sm;
        public LtmsAlgorithm(SystemModel sm , Observation obs) {
            this.cnf = new Cnf(sm);
            this.sm=sm;
            cnf.assign_diagnose(obs);
            //this.cnf = new Cnf(build_example());
        }
        private List<Clouse> fringe = new List<Clouse>();
        private List<Clouse> conflicts = new List<Clouse>();

        /*
        * findConflicts return  List<List<Gate>> , each  item List<Gate>  is list of Gate is a conflict
        * */
        public List<List<Gate>> findConflicts()
        {

            check_conflicts();
            List<List<Gate>> conf_gates = new List<List<Gate>>();
            foreach (Clouse c in this.conflicts){
                List<Gate> conf=build_conf_list(c.supporting,new List<Gate>());
                conf.Add(sm.Components.Find(g => g.Id==c.c_gate));
                conf_gates.Add(conf);
            }
           // return conf;
            return conf_gates;
        }


        public ConflictSet ConvertGateListToConflict(List<List<Gate>> conflictList)
        {
            ConflictSet conflictSet = new ConflictSet();
            conflictSet.Conflicts = new List<Conflict>(); 

            foreach (List<Gate> conflictGateList in conflictList)
            {
                Conflict conflict = new Conflict(conflictGateList);
                conflictSet.Conflicts.Add(conflict);
            }

            return conflictSet;
        }


        /*
        * check_conflicts implement the ltms algorithm and build this .conflicts by check for conflicts in the cnf 
        * */
        private void check_conflicts() //1-no conflict, 0-conflict
        {
            int len;
            while (true)
            {
                List<Clouse> sat = new List<Clouse>();                
                foreach (Clouse c in this.cnf.formula)
                {
                    if (c.literals.Any(x => ((x.val == 1 && x.not) || (x.val == 0 && !x.not))))
                    {
                        sat.Add(c); //c is satisfied
                    }
                    else if (c.literals.All(x => ((x.val == 1 && !x.not) || (x.val == 0 && x.not))))
                    {
                        this.conflicts.Add(c);  //c is conflict
                    }
                    else if (c.unknown == 1)//fringe
                    {
                        if (!this.fringe.Any(x=>x.unknown_literals.Any(y=>y.id==c.unknown_literals[0].id)))
                             this.fringe.Add(c);
                    }

                }
                this.cnf.formula.RemoveAll(c => sat.Any(y => y.c_id == c.c_id));

                len = this.fringe.Count;
                if (len > 0)
                {
                    foreach (Clouse c in this.fringe)
                    {
                        Atomic atom = c.unknown_literals[0];
                        if (atom.not)
                            atom.val = 1;
                        else
                            atom.val = 0;
                        List <Clouse> update= this.cnf.formula.FindAll(x => x.unknown_literals.Any(y => y.id == atom.id));
                        foreach (Clouse cl in update)
                        {
                            Atomic atom2 = cl.unknown_literals.Find(l => l.id == atom.id);
                            if (atom2 != null)
                            {
                                if (cl.c_gate!=c.c_gate)
                                    cl.supporting.Add(c);
                                cl.unknown--;
                                atom2.val = atom.val;
                                cl.unknown_literals.Remove(atom2);
                            }
                        }
                    }

                }
                else
                    return;
                this.fringe.Clear();
            }
        
    }




        /*
        * build_conf_list traverse over each c.supporting and build the gate list which representing the conflicts
        * */
        private List<Gate> build_conf_list(List<Clouse> c_list, List<Gate> cl)
        {            
            foreach (Clouse c in c_list)
            {
                if (c.supporting.Count > 0)
                    build_conf_list(c.supporting, cl);

                else
                    cl.Add(sm.Components.Find(g => g.Id == c.c_gate));
                
            }
            return cl;
        }

        /*
        * this is some test fot the algorithm relaying on the example from class
        * */
        public List <Clouse> build_example()
        {
            List<Clouse> formula = new List<Clouse>();
            Atomic a = new Atomic("ok", false);
            Atomic a2 = new Atomic("uf", false);
            List<Atomic> l1 = new List<Atomic>();
            l1.Add(a);
            l1.Add(a2);
            Clouse c1 = new Clouse(l1,0);
            c1.unknown = 2;
            c1.unknown_literals.Add(a);
            c1.unknown_literals.Add(a2);
            formula.Add(c1);
            /////

            Atomic a3 = new Atomic("ok", false);
            Atomic a4 = new Atomic("a", true);
            List<Atomic> l2 = new List<Atomic>();
            l2.Add(a3);
            l2.Add(a4);
            Clouse c2 = new Clouse(l2, 0);
            c2.unknown = 2;
            c2.unknown_literals.Add(a3);
            c2.unknown_literals.Add(a4);
            formula.Add(c2);

            /////

            Atomic a5 = new Atomic("nco", true);
            Atomic a6 = new Atomic("ia", false);
            List<Atomic> l3 = new List<Atomic>();
            l3.Add(a5);
            l3.Add(a6);
            Clouse c3 = new Clouse(l3, 0);
            c3.unknown = 2;
            c3.unknown_literals.Add(a5);
            c3.unknown_literals.Add(a6);
            formula.Add(c3);

            /////

            Atomic a7 = new Atomic("a", false);
            Atomic a8 = new Atomic("ia", false);
            List<Atomic> l4 = new List<Atomic>();
            l4.Add(a7);
            l4.Add(a8);
            Clouse c4 = new Clouse(l4, 0);
            c4.unknown = 2;
            c4.unknown_literals.Add(a7);
            c4.unknown_literals.Add(a8);
            formula.Add(c4);
            /////

   

            Atomic a11 = new Atomic("ok", false);
            Atomic a12 = new Atomic("rf", false);
            List<Atomic> l6 = new List<Atomic>();
            l6.Add(a11);
            l6.Add(a12);
            Clouse c6 = new Clouse(l6, 0);
            c6.unknown = 2;
            c6.unknown_literals.Add(a11);
            c6.unknown_literals.Add(a12);
            formula.Add(c6);

            /////

            Atomic a13 = new Atomic("ia", true);
            Atomic a14 = new Atomic("rf", false);
            List<Atomic> l7 = new List<Atomic>();
            l7.Add(a13);
            l7.Add(a14);
            Clouse c7 = new Clouse(l7, 0);
            c7.unknown = 2;
            c7.unknown_literals.Add(a13);
            c7.unknown_literals.Add(a14);
            formula.Add(c7);




            /////

            Atomic a15 = new Atomic("nci", false);
            Atomic a16 = new Atomic("a", false);
            Atomic a17 = new Atomic("nco", true);
            List<Atomic> l8 = new List<Atomic>();
            l8.Add(a15);
            l8.Add(a16);
            l8.Add(a17);
            Clouse c8 = new Clouse(l8, 0);
            c8.unknown = 3;
            c8.unknown_literals.Add(a15);
            c8.unknown_literals.Add(a16);
            c8.unknown_literals.Add(a17);
            formula.Add(c8);

            /////

            Atomic a18 = new Atomic("uf", false);
            Atomic a19 = new Atomic("rf", false);
            List<Atomic> l9 = new List<Atomic>();
            l9.Add(a18);
            l9.Add(a19);
            Clouse c9 = new Clouse(l9, 0);
            c9.unknown = 2;
            c9.unknown_literals.Add(a18);
            c9.unknown_literals.Add(a19);

            formula.Add(c7);

            /////

            Atomic a20 = new Atomic("rf", true);
            List<Atomic> l10 = new List<Atomic>();
            l10.Add(a20);
            Clouse c10 = new Clouse(l10, 0);
            c10.unknown = 1;
            c10.unknown_literals.Add(a20);

            formula.Add(c10);

            /////

            Atomic a21 = new Atomic("ok", true);
            List<Atomic> l11 = new List<Atomic>();
            l11.Add(a21);
            Clouse c11 = new Clouse(l11, 0);
            c11.unknown = 1;
            c11.unknown_literals.Add(a21);

            formula.Add(c11);

            return formula;


        }

       
        

    }
}

