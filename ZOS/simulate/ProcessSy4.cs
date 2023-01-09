using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{

    class ProcessSy4
    {
       
        string name;
        string state;
        int at;     //到达时间
        int st; //service time服务时间
        int ct;     //completion time完成时刻
        int rst;    //剩余服务时间

        int[] max=new int[Defind.getMaxResourceKind()]; //最大需求矩阵
        int[] allocation = new int[Defind.getMaxResourceKind()];  //分配矩阵
        int[] need = new int[Defind.getMaxResourceKind()];    //需求矩阵
        int[] request = new int[Defind.getMaxResourceKind()];
        int size;
        int needSize;
        int ptAddress;//页表地址
        bool finish;    //满足标记
        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getState()
        {
            return state;
        }

        public void setState(string state)
        {
            this.state = state;
        }

        public int getAt()
        {
            return at;
        }

        public void setAt(int at)
        {
            this.at = at;
        }

        public int getSt()
        {
            return st;
        }

        public void setSt(int st)
        {
            this.st = st;
        }

        public int getCt()
        {
            return ct;
        }

        public void setCt(int ct)
        {
            this.ct = ct;
        }

        public int getRst()
        {
            return rst;
        }

        public void setRst(int rst)
        {
            this.rst = rst;
        }

        public int[] getMax()
        {
            return max;
        }

        public void setMax(int[] max)
        {
            this.max = max;
        }

        public int[] getAllocation()
        {
            return allocation;
        }

        public void setAllocation(int[] allocation)
        {
            this.allocation = allocation;
        }

        public int[] getNeed()
        {
            return need;
        }

        public void setNeed(int[] need)
        {
            this.need = need;
        }

        public int[] getRequest()
        {
            return request;
        }

        public void setRequest(int[] request)
        {
            this.request = request;
        }

        public int getSize()
        {
            return size;
        }

        public void setSize(int size)
        {
            this.size = size;
        }

        public int getNeedSize()
        {
            return needSize;
        }

        public void setNeedSize(int needSize)
        {
            this.needSize = needSize;
        }

        public int getPtAddress()
        {
            return ptAddress;
        }

        public void setPtAddress(int ptAddress)
        {
            this.ptAddress = ptAddress;
        }

        public bool getFinish()
        {
            return finish;
        }

        public void setFinish(bool finish)
        {
            this.finish = finish;
        }

    }
}
