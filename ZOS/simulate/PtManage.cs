using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class PtManage
    {
        int PT_Address; //页表地址
        Pt[] pt = new Pt[10];

        public Pt[] getPt()
        {
            return pt;
        }

        public void setPt(Pt[] pt)
        {
            this.pt = pt;
        }

       
        public int getPT_Address()
        {
            return PT_Address;
        }

        public void setPT_Address(int PT_Address)
        {
            this.PT_Address = PT_Address;
        }
    }
}
