using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class Available
    {
        int resource_number; //资源数目
        int work;   //工作向量	
        public int getResource_number()
        {
            return resource_number;
        }

        public void setResource_number(int resource_number)
        {
            this.resource_number = resource_number;
        }

        public int getWork()
        {
            return work;
        }

        public void setWork(int work)
        {
            this.work = work;
        }
    }
}
