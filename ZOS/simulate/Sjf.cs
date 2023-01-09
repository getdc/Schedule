using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zos
{
    class Sjf
    {
        string name  ; //作业名
        string userName ;//用户名称
        int size;   //作业大小
        int needSize;   //要求内存大小
        int arrivetime; //到达时间
        int servicetime;//服务时间
        int starttime; //开始时间
        int finishtime;//完成时间
        int[] max=new int[Defind.getMaxResourceKind()]; //最大需求矩阵
        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getUserName()
        {
            return userName;
        }

        public void setUserName(string userName)
        {
            this.userName = userName;
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

        public int getArrivetime()
        {
            return arrivetime;
        }

        public void setArrivetime(int arrivetime)
        {
            this.arrivetime = arrivetime;
        }

        public int getServicetime()
        {
            return servicetime;
        }

        public void setServicetime(int servicetime)
        {
            this.servicetime = servicetime;
        }

        public int getStarttime()
        {
            return starttime;
        }

        public void setStarttime(int starttime)
        {
            this.starttime = starttime;
        }

        public int getFinishtime()
        {
            return finishtime;
        }

        public void setFinishtime(int finishtime)
        {
            this.finishtime = finishtime;
        }

        public int[] getMax()
        {
            return max;
        }

        public void setMax(int[] max)
        {
            this.max = max;
        }
    }
}
