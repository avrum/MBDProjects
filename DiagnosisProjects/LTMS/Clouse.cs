using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.LTMS
{
    /*
    * this class representing a Clouse which consist  of list of literals
    * */

    class Clouse
    {
        private static int Counter__ = 0;

        public int c_id;
        public int c_gate;
        public List <Atomic> literals;
        public List<Atomic> assigned_literals;
        public List<Atomic> unknown_literals;
        public List<Clouse> supporting;
        public int num_of_literals;
        public int unknown { get; set; }

        public Clouse(List<Atomic> lst, int gate)
        {
            this.c_gate = gate;
            this.literals = lst;
            this.num_of_literals = this.literals.Count;
            this.unknown = 0;
            this.unknown_literals = new List<Atomic>();
            this.assigned_literals = new List<Atomic>();
            this.supporting = new List<Clouse>();
            c_id = Clouse.Counter__;
            Clouse.Counter__++;
            this.unknown = num_of_literals;
        }


        /*
        * calculate_literals substitute the observation in clouse's literals
        * */
        public void calculate_literals(Observation obs)
        {
            calc(obs, 'i');
            calc(obs, 'o');
            build_unknown_list();
        }

      
        private void calc(Observation obs,char c)
        {
            int count = 1;
            int assigned = 0;
            bool[] vals=null;
            if (c == 'i')
            {
                vals=obs.InputValues;
            }
            else
            {
                vals = obs.OutputValues;
            }
            foreach (bool b in vals)
            {
                string _id = c + count.ToString();
                foreach (Atomic a in this.literals)
                {
                    if (a.id == _id)
                    {
                        a.set_val(b);
                        assigned++;
                        this.unknown--;
                        this.assigned_literals.Add(a);
                    }
                }

                count++;
            }


            
        }

        private void build_unknown_list()
        {            
            foreach (Atomic a in this.literals)
            {
                bool alreadyExists = this.assigned_literals.Any(x => x.id == a.id);
                if (!alreadyExists)
                {
                    this.unknown_literals.Add(a);
                }
            }
        }

    }
}
