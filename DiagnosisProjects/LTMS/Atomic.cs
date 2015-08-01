using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosisProjects.LTMS
{

    /*
    * this class represents literal
    **/
    class Atomic
    {
        public string id;
        public bool not; //not (a) = false, a=true
        public int val;


        public Atomic(string id,bool n)
        {
            this.id = id;
            this.not = n;
            this.val = -1;
        }

        public void set_val(bool _val)
        {
            if (_val)
                this.val = 1;
            else
                this.val = 0;
        }


    }
}
