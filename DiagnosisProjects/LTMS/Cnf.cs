using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.LTMS
{

    /*
    * this class represents a cnf which generated from the SystemModel. it is obligated to handle any gate with any number of inputs.
    **/
    class Cnf
    {
        public List<Clouse> formula=new List<Clouse>();

        public Cnf(SystemModel sm)
        {
             
            build_formula(sm);

        }

        public Cnf(List<Clouse> c)
        {

            this.formula = c;

        }

        /*
        * assign the diagnosis to the CNF
        **/
        public void assign_diagnose(Observation obs)
        {
            foreach (Clouse c in formula)
            {
                c.calculate_literals(obs);
            }
        }

        /*
        * build_formula build the cnf according to the system model
        **/
        private void build_formula(SystemModel sm)// change to sm
        {
           foreach (Gate g in sm.Components)
           {
               switch (g.type)
                {
                    case Gate.Type.buffer:
                        {
                            buffer_clouse(g);
                            break;
                        }
                    case Gate.Type.not:
                        {
                            not_clouse(g);
                            break;

                        }
                    case Gate.Type.or:
                        {
                            or_clouse(g);
                            break;

                        }
                    case Gate.Type.and:
                        {
                            and_clouse(g);
                            break;

                        }
                    case Gate.Type.xor:
                        {
                            xor_clouse(g);
                            break;

                        }
                    case Gate.Type.nxor:
                        {
                            nxor_clouse(g);
                            break;

                        }
                    case Gate.Type.nand:
                        {
                            nand_clouse(g);
                            break;

                        }
                    case Gate.Type.nor:
                        {
                            nor_clouse(g);
                            break;

                        }
                }
            }
           addGatesToFormula(sm.Components);
            
        }

        private void addGatesToFormula(List<Gate> comp)
        {
            foreach (Gate g in comp)
            {
                List<Atomic> c = new List<Atomic>();
                c.Add(new Atomic(g.Id.ToString(), true));
                formula.Add(new Clouse(c,g.Id));
            }
        }

        private void buffer_clouse(Gate g)
        {
            string out_id = g.Output.Type.ToString() + g.Output.Id.ToString();
            string in_id = g.getInput()[0].Type.ToString() + g.getInput()[0].Id.ToString();

            List<Clouse> ans = new List<Clouse>();
            List<Atomic> c1 = new List<Atomic>();
            List<Atomic> c2 = new List<Atomic>();

            c1.Add(new Atomic(in_id, false));
            c1.Add(new Atomic(out_id, true));
            c1.Add(new Atomic(g.Id.ToString(), false));

            c2.Add(new Atomic(in_id, true));
            c2.Add(new Atomic(out_id, false));
            c2.Add(new Atomic(g.Id.ToString(), false));

            formula.Add(new Clouse(c1,g.Id));
            formula.Add(new Clouse(c2, g.Id));

        }

        private void not_clouse(Gate g)
        {
            string out_id=g.Output.Type.ToString() + g.Output.Id.ToString();
            string in_id = g.getInput()[0].Type.ToString() + g.getInput()[0].Id.ToString();

            List<Clouse> ans = new List<Clouse>();
            List<Atomic> c1 = new List<Atomic>();
            List<Atomic> c2 = new List<Atomic>();

            c1.Add(new Atomic(in_id,true));
            c1.Add(new Atomic(out_id, true));
            c1.Add(new Atomic(g.Id.ToString(), false));

            c2.Add(new Atomic(in_id, false));
            c2.Add(new Atomic(out_id, false));
            c2.Add(new Atomic(g.Id.ToString(), false));

            formula.Add(new Clouse(c1, g.Id));
            formula.Add(new Clouse(c2, g.Id));

        }


        private void or_clouse(Gate g)
        {
            string out_id = g.Output.Type.ToString() + g.Output.Id.ToString();
            string in_id = "";
            List<Atomic> c2 = new List<Atomic>();
            Atomic health = new Atomic(g.Id.ToString(), false);
            foreach (Wire w in g.getInput())
            {
                List<Atomic> c1 = new List<Atomic>();
                
                in_id = w.Type.ToString() + w.Id.ToString();
                c2.Add(new Atomic(in_id, true));
                c1.Add(new Atomic(in_id, false));
                c1.Add(new Atomic(out_id, true));
                c1.Add(health);
                List<Clouse> ans = new List<Clouse>();
                formula.Add(new Clouse(c1, g.Id));
            }
            c2.Add(new Atomic(out_id, false));
            c2.Add(health);
            formula.Add(new Clouse(c2, g.Id));
            
        }

        private void nor_clouse(Gate g)
        {
            string out_id = g.Output.Type.ToString() + g.Output.Id.ToString();
            string in_id = "";
            List<Atomic> c2 = new List<Atomic>();
            Atomic health = new Atomic(g.Id.ToString(), false);
            foreach (Wire w in g.getInput())
            {
                List<Atomic> c1 = new List<Atomic>();

                in_id = w.Type.ToString() + w.Id.ToString();
                c2.Add(new Atomic(in_id, true));
                c1.Add(new Atomic(in_id, false));
                c1.Add(new Atomic(out_id, false));
                c1.Add(health);
                List<Clouse> ans = new List<Clouse>();
                formula.Add(new Clouse(c1, g.Id));
            }
            c2.Add(new Atomic(out_id, true));
            c2.Add(health);
            formula.Add(new Clouse(c2, g.Id));

        }

        private void and_clouse(Gate g)
        {
            string out_id = g.Output.Type.ToString() + g.Output.Id.ToString();
            string in_id = "";
            List<Atomic> c2 = new List<Atomic>();
            Atomic health = new Atomic(g.Id.ToString(), false);


            foreach (Wire w in g.getInput())
            {
                List<Atomic> c1 = new List<Atomic>();

                in_id = w.Type.ToString() + w.Id.ToString();
                c2.Add(new Atomic(in_id, false));
                c1.Add(new Atomic(in_id, true));
                c1.Add(new Atomic(out_id, false));
                c1.Add(health);
                List<Clouse> ans = new List<Clouse>();
                formula.Add(new Clouse(c1, g.Id));
            }
            c2.Add(new Atomic(out_id, true));
            c2.Add(health);
            formula.Add(new Clouse(c2, g.Id));

        }

        private void nand_clouse(Gate g)
        {
            string out_id = g.Output.Type.ToString() + g.Output.Id.ToString();
            string in_id = "";
            List<Atomic> c2 = new List<Atomic>();
            Atomic health = new Atomic(g.Id.ToString(), false);


            foreach (Wire w in g.getInput())
            {
                List<Atomic> c1 = new List<Atomic>();
                in_id = w.Type.ToString() + w.Id.ToString();
                c2.Add(new Atomic(in_id, false));
                c1.Add(new Atomic(in_id, true));
                c1.Add(new Atomic(out_id, true));
                c1.Add(health);
                List<Clouse> ans = new List<Clouse>();
                formula.Add(new Clouse(c1, g.Id));
            }
            c2.Add(new Atomic(out_id, false));
            c2.Add(health);
            formula.Add(new Clouse(c2, g.Id));

        }



        private void xor_clouse(Gate g)
        {
            /**n inputs
            *2^n clouses:loop
            *n+1 literal each
            *odd number of not
             * */
            Atomic health = new Atomic(g.Id.ToString(), false);
            int n=g.getInput().Count;
            int loop = (int)Math.Pow(2, n);
            int in_loop=n+1;
            int k;
            List<string> all_ids = new List<string>();
            foreach (Wire w in g.getInput())
            {
                all_ids.Add(w.Type.ToString() + w.Id.ToString());
            }

            all_ids.Add( g.Output.Type.ToString() + g.Output.Id.ToString());

            for ( k = 1; k < in_loop; k++)
            {
                List<List<String>> possibles = GenerateSelections(all_ids, k);
                
                foreach (List<String> s in possibles)
                {
                    List<Atomic> c1 = new List<Atomic>();
                    foreach (String s_in in s)
                    {
                        c1.Add(new Atomic(s_in, false));//add not
                        
                    }
                    List<string> add_ids = get_true_literals(all_ids, s);
                    foreach (String t in add_ids)
                        c1.Add(new Atomic(t, true));//add

                    c1.Add(health);
                    formula.Add(new Clouse(c1, g.Id));
                             
                }
                k++;
            }

            ///only if even number of inputs
            if (n % 2 == 0)
            {
                List<Atomic> c2 = new List<Atomic>();
                foreach (String s_in in all_ids)
                {
                    c2.Add(new Atomic(s_in, false));//add 
                }
                c2.Add(health);
                formula.Add(new Clouse(c2, g.Id));
            }
   
                
            
        }

        private void nxor_clouse(Gate g)
        {
            /**n inputs
            *2^n clouses:loop
            *n+1 literal each
            *odd number of not
             * */
            Atomic health = new Atomic(g.Id.ToString(), false);
            int n = g.getInput().Count;
            int loop = (int)Math.Pow(2, n);
            int in_loop = n + 1;
            int k;
            List<string> all_ids = new List<string>();
            foreach (Wire w in g.getInput())
            {
                all_ids.Add(w.Type.ToString() + w.Id.ToString());
            }

            all_ids.Add(g.Output.Type.ToString() + g.Output.Id.ToString());

            for (k = 2; k < in_loop; k++)
            {
                List<List<String>> possibles = GenerateSelections(all_ids, k);

                foreach (List<String> s in possibles)
                {
                    List<Atomic> c1 = new List<Atomic>();
                    foreach (String s_in in s)
                    {
                        c1.Add(new Atomic(s_in, false));//add not

                    }
                    List<string> add_ids = get_true_literals(all_ids, s);
                    foreach (String t in add_ids)
                        c1.Add(new Atomic(t, true));//add

                    c1.Add(health);
                    formula.Add(new Clouse(c1, g.Id));

                }
                k++;
            }

                List<Atomic> c2 = new List<Atomic>();
                foreach (String s_in in all_ids)
                {
                    c2.Add(new Atomic(s_in, true));//add 
                }
                c2.Add(health);
                formula.Add(new Clouse(c2, g.Id));

        }

        private List<string> get_true_literals(List<String> all_ids, List<String> false_str)
        {
            List<string> ans = new List<string>();
            foreach (string s in all_ids)
            {
                Object o=false_str.Find(item=>item==s);
                if (o==null)
                {
                    ans.Add(s);
                }
            }
            return ans;

        }


        private List<List<String>> GenerateSelections(List<String> items, int n)
            {
                // Make an array to tell whether
                // an item is in the current selection.
                bool[] in_selection = new bool[items.Count];
                List<List<String>> results = new List<List<String>>();
                SelectItems(items, in_selection, results, n, 0);
                return results;

                
            }

        private void SelectItems(List<String> items, bool[] in_selection, List<List<String>> results, int n, int first_item)
        {
            if (n == 0)
            {
                // Add the current selection to the results.
                List<String> selection = new List<String>();
                for (int i = 0; i < items.Count; i++)
                {
                    // If this item is selected, add it to the selection.
                    if (in_selection[i]) 
                        selection.Add(items[i]);
                }
                results.Add(selection);
            }
            else
            {
                // Try adding each of the remaining items.
                for (int i = first_item; i < items.Count; i++)
                {
                    // Try adding this item.
                    in_selection[i] = true;

                    // Recursively add the rest of the required items.
                    SelectItems(items, in_selection, results, n - 1, i + 1);

                    // Remove this item from the selection.
                    in_selection[i] = false;
                }
            }
        }


    }

 
}
